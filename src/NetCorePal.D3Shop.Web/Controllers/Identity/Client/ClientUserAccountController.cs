using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCorePal.D3Shop.Web.Application.Commands.Identity.Client;
using NetCorePal.D3Shop.Web.Application.Queries.Identity.Client;
using NetCorePal.D3Shop.Web.Application.Queries.Identity.Client.Dto;
using NetCorePal.D3Shop.Web.Auth;
using NetCorePal.D3Shop.Web.Controllers.Identity.Client.Requests;
using NetCorePal.D3Shop.Web.Controllers.Identity.Client.Responses;
using NetCorePal.D3Shop.Web.Helper;
using NetCorePal.Extensions.Dto;

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
        var loginResult = await mediator.Send(new ClientUserLoginCommand(
            userAuthInfo.UserId,
            passwordHash,
            DateTime.UtcNow,
            request.LoginMethod,
            ipAddress,
            request.UserAgent
        ));

        if (!loginResult.IsSuccess)
            return ClientUserLoginResponse.Failure(loginResult.FailedMessage).AsResponseData();

        var token = await tokenGenerator.GenerateJwtAsync([
            new Claim(ClaimTypes.NameIdentifier, userAuthInfo.UserId.ToString())
        ]);
        return ClientUserLoginResponse.Success(token).AsResponseData();
    }

    [HttpPost]
    public async Task<ResponseData<string>> Register([FromBody] ClientUserRegisterRequest request)
    {
        var (passwordHash, passwordSalt) = NewPasswordHasher.HashPassword(request.Password);

        var userId = await mediator.Send(new CreateClientUserCommand(
            request.NickName,
            request.Avatar,
            request.Phone,
            passwordHash,
            passwordSalt,
            request.Email
        ));

        var token = await tokenGenerator.GenerateJwtAsync([
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        ]);
        return token.AsResponseData();
    }

    [HttpGet]
    public async Task<ResponseData<ClientUserInfo>> GetClientUserInfo()
    {
        var userInfo = await clientUserQuery.GetClientUserInfoByIdAsync(currentUser.UserId, HttpContext.RequestAborted);
        return userInfo.AsResponseData();
    }
}