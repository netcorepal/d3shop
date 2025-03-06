using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;
using NetCorePal.D3Shop.Web.Application.Commands.Identity.Client;
using NetCorePal.D3Shop.Web.Application.Queries.Identity.Client;
using NetCorePal.D3Shop.Web.Auth;
using NetCorePal.D3Shop.Web.Controllers.Identity.Client.Requests;
using NetCorePal.D3Shop.Web.Controllers.Identity.Client.Responses;
using NetCorePal.D3Shop.Web.Helper;
using NetCorePal.Extensions.Dto;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Controllers.Identity.Client;

[Route("api/[controller]/[action]")]
[ApiController]
[AllowAnonymous]
public class ClientUserAccountController(
    IMediator mediator,
    ClientUserQuery clientUserQuery,
    TokenGenerator tokenGenerator,
    ICurrentClientUser currentUser) : ControllerBase
{
    [HttpPost]
    public async Task<ResponseData<ClientUserLoginResponse>> Login([FromBody] ClientUserLoginRequest request)
    {
        var userAuthInfo =
            await clientUserQuery.RetrieveClientWithAuthInfoByPhoneAsync(request.Phone, HttpContext.RequestAborted);
        var passwordHash = NewPasswordHasher.HashPassword(request.Password, userAuthInfo.PasswordSalt);

        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
        var refreshToken = TokenGenerator.GenerateRefreshToken();
        var loginResult = await mediator.Send(new ClientUserLoginCommand(
            userAuthInfo.UserId,
            passwordHash,
            DateTime.UtcNow,
            request.LoginMethod,
            ipAddress,
            request.UserAgent,
            refreshToken
        ));

        if (!loginResult.IsSuccess)
            return ClientUserLoginResponse.Failure(loginResult.FailedMessage).AsResponseData();

        var token = await tokenGenerator.GenerateJwtAsync([
            new Claim(ClaimTypes.NameIdentifier, userAuthInfo.UserId.ToString())
        ]);
        return ClientUserLoginResponse.Success(token, refreshToken).AsResponseData();
    }

    [HttpPost]
    public async Task<ResponseData<ClientUserId>> Register([FromBody] ClientUserRegisterRequest request)
    {
        var (passwordHash, passwordSalt) = NewPasswordHasher.HashPassword(request.Password);

        return await mediator.Send(new CreateClientUserCommand(
            request.NickName,
            request.Avatar,
            request.Phone,
            passwordHash,
            passwordSalt,
            request.Email
        )).AsResponseData();
    }

    [HttpPut]
    public async Task<ResponseData<ClientUserGetRefreshTokenResponse>> GetRefreshToken(
        [FromBody] ClientUserGetRefreshTokenRequest request)
    {
        var userPrincipal = await tokenGenerator.GetPrincipalFromExpiredToken(request.Token);

        var userIdStr = userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier) ??
                        throw new KnownException("Invalid Token:There is no Id in the token");
        if (!long.TryParse(userIdStr, out var userId))
            throw new KnownException("Invalid Token:There is no Id in the token");

        var refreshToken = TokenGenerator.GenerateRefreshToken();

        var loginExpiryDate = await mediator.Send(
            new UpdateClientUserRefreshTokenCommand(new ClientUserId(userId), request.RefreshToken, refreshToken));

        var token = await tokenGenerator.GenerateJwtAsync([
            new Claim(ClaimTypes.NameIdentifier, userIdStr)
        ]);
        var response = new ClientUserGetRefreshTokenResponse(
            token,
            refreshToken,
            loginExpiryDate);
        return response.AsResponseData();
    }
}