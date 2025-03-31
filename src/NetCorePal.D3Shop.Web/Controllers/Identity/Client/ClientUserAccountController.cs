using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;
using NetCorePal.D3Shop.Web.Application.Commands.Identity.Client;
using NetCorePal.D3Shop.Web.Application.Queries.Identity.Client;
using NetCorePal.D3Shop.Web.Controllers.Identity.Client.Dto;
using NetCorePal.D3Shop.Web.Controllers.Identity.Client.Requests;
using NetCorePal.D3Shop.Web.Controllers.Identity.Client.Responses;
using NetCorePal.D3Shop.Web.Helper;
using NetCorePal.Extensions.Dto;
using NetCorePal.Extensions.Jwt;
using NetCorePal.Extensions.Primitives;
using OpenIddict.Abstractions;
using OpenIddict.Client;
using OpenIddict.Client.AspNetCore;

namespace NetCorePal.D3Shop.Web.Controllers.Identity.Client;

[Route("api/[controller]/[action]")]
[ApiController]
[AllowAnonymous]
public class ClientUserAccountController(
    IMediator mediator,
    ClientUserQuery clientUserQuery,
    OpenIddictClientService openIddictClientService,
    IMemoryCache memoryCache,
    IJwtProvider jwtProvider,
    IOptions<AppConfiguration> appConfiguration) : ControllerBase
{
    [HttpPost]
    public async Task<ResponseData<ClientUserLoginResponse>> Login([FromBody] ClientUserLoginRequest request)
    {
        var userAuthInfo =
            await clientUserQuery.RetrieveClientWithAuthInfoByPhoneAsync(request.Phone, HttpContext.RequestAborted);
        var passwordHash = NewPasswordHasher.HashPassword(request.Password, userAuthInfo.PasswordSalt);

        var ipAddress = HttpContext.GetRemoteIpAddress()?.ToString() ?? string.Empty;
        var userAgent = HttpContext.Request.Headers.UserAgent.ToString();
        var refreshToken = TokenGenerator.GenerateRefreshToken();
        var loginResult = await mediator.Send(new ClientUserLoginCommand(
            userAuthInfo.UserId,
            passwordHash,
            DateTime.UtcNow,
            request.LoginMethod,
            ipAddress,
            userAgent,
            refreshToken
        ));

        if (!loginResult.IsSuccess)
            throw new KnownException(loginResult.FailedMessage);

        var tokenExpiryTime = DateTime.Now.AddMinutes(appConfiguration.Value.TokenExpiryInMinutes);
        var jwt = await jwtProvider.GenerateJwtToken(new JwtData("issuer-x", "audience-y",
            [new Claim(ClaimTypes.NameIdentifier, userAuthInfo.UserId.ToString())],
            DateTime.Now,
            tokenExpiryTime));
        return new ClientUserLoginResponse(jwt, refreshToken, tokenExpiryTime).AsResponseData();
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
        [FromBody] string refreshToken)
    {
        var userId =
            await clientUserQuery.GetClientUserIdByRefreshTokenAsync(refreshToken, HttpContext.RequestAborted);

        var newRefreshToken = TokenGenerator.GenerateRefreshToken();
        await mediator.Send(
            new UpdateClientUserRefreshTokenCommand(userId, refreshToken, newRefreshToken));

        var tokenExpiryTime = DateTime.Now.AddMinutes(appConfiguration.Value.TokenExpiryInMinutes);
        var jwt = await jwtProvider.GenerateJwtToken(new JwtData("issuer-x", "audience-y",
            [new Claim(ClaimTypes.NameIdentifier, userId.ToString())],
            DateTime.Now,
            tokenExpiryTime));
        var response = new ClientUserGetRefreshTokenResponse(
            jwt,
            refreshToken,
            tokenExpiryTime);
        return response.AsResponseData();
    }

    [HttpGet]
    public async Task<ActionResult> ExternalLogIn(string provider, string? returnUrl)
    {
        // 注意：OpenIddict在处理挑战操作时始终会验证指定的提供者名称，
        // 但也可以在更早的阶段验证提供者，以返回错误页面或特定的HTTP错误代码。
        var registrations = await openIddictClientService.GetClientRegistrationsAsync();
        if (!registrations.Any(registration =>
                string.Equals(registration.ProviderName, provider, StringComparison.Ordinal)))
            return BadRequest($"The provider:{provider} does not supported");

        var properties = new AuthenticationProperties(new Dictionary<string, string?>
        {
            // 注意：当客户端选项中只注册了一个客户端时，
            // 指定颁发者URI或提供者名称不是必需的。
            [OpenIddictClientAspNetCoreConstants.Properties.ProviderName] = provider
        })
        {
            RedirectUri = returnUrl ?? "/"
        };

        // 请求OpenIddict客户端中间件将用户代理重定向到身份提供者。
        return Challenge(properties, OpenIddictClientAspNetCoreDefaults.AuthenticationScheme);
    }

// 注意：该控制器对所有提供者使用相同的回调操作，
// 但对于更喜欢为每个提供者使用不同操作的用户，
// 可以将以下操作拆分为多个操作。
    [HttpGet("/api/[controller]/callback/login/{provider}")]
    [HttpPost("/api/[controller]/callback/login/{provider}")]
    [IgnoreAntiforgeryToken]
    public async Task<ResponseData<ClientUserExternalLoginResponse>> LogInCallback(ThirdPartyProvider provider)
    {
        // 检索由OpenIddict在回调处理过程中验证的授权数据。
        var result = await HttpContext.AuthenticateAsync(OpenIddictClientAspNetCoreDefaults.AuthenticationScheme);

        if (result.Principal is not { Identity.IsAuthenticated: true })
            throw new InvalidOperationException("外部授权数据无法用于身份验证。");

        var openId = result.Principal.GetClaim(ClaimTypes.NameIdentifier) ??
                     throw new InvalidOperationException("无效的令牌：令牌中没有Id");

        var ipAddress = HttpContext.GetRemoteIpAddress()?.ToString() ?? string.Empty;
        var userAgent = HttpContext.Request.Headers.UserAgent.ToString();

        var userId = await clientUserQuery.GetClientUserIdByOpenIdAsync(openId, HttpContext.RequestAborted);
        if (userId is null)
        {
            var signupToken = TokenGenerator.GenerateCryptographicallySecureGuid();

            var registration = await openIddictClientService
                .GetClientRegistrationByProviderNameAsync(provider.ToString());
            var clientId = registration.ClientId ?? string.Empty;

            var cacheData = new ThirdPartySignupCache(
                provider,
                clientId,
                openId,
                ipAddress,
                userAgent,
                DateTime.Now);

            memoryCache.Set(signupToken, cacheData);

            return ClientUserExternalLoginResponse.NeedSignUp(
                signupToken
            ).AsResponseData();
        }

        var refreshToken = TokenGenerator.GenerateRefreshToken();
        await mediator.Send(new ClientUserExternalLoginCommand(userId, DateTime.Now, provider.ToString(), ipAddress,
            userAgent,
            refreshToken));

        var tokenExpiryTime = DateTime.Now.AddMinutes(appConfiguration.Value.TokenExpiryInMinutes);
        var jwt = await jwtProvider.GenerateJwtToken(new JwtData("issuer-x", "audience-y",
            [new Claim(ClaimTypes.NameIdentifier, userId.ToString())],
            DateTime.Now,
            tokenExpiryTime));
        return ClientUserExternalLoginResponse.Success(jwt, refreshToken, tokenExpiryTime).AsResponseData();
    }

    [HttpPost]
    public async Task<ResponseData<ClientUserExternalSignUpResponse>> ExternalSignUp(
        [FromBody] ClientUserExternalSignUpRequest request)
    {
        var thirdPartySignupCache = memoryCache.Get<ThirdPartySignupCache>(request.SignupToken)
                                    ?? throw new InvalidOperationException("无效的缓存令牌");
        memoryCache.Remove(request.SignupToken);

        var (passwordHash, passwordSalt) = NewPasswordHasher.HashPassword(request.Password);
        var refreshToken = TokenGenerator.GenerateRefreshToken();

        var userId = await mediator.Send(new ClientUserExternalSignUpCommand(
            DateTime.Now,
            request.Phone,
            passwordHash,
            passwordSalt,
            thirdPartySignupCache.ThirdPartyProvider,
            thirdPartySignupCache.AppId,
            thirdPartySignupCache.OpenId,
            refreshToken,
            thirdPartySignupCache.IpAddress,
            thirdPartySignupCache.UserAgent
        ));
        var tokenExpiryTime = DateTime.Now.AddMinutes(appConfiguration.Value.TokenExpiryInMinutes);
        var jwt = await jwtProvider.GenerateJwtToken(new JwtData("issuer-x", "audience-y",
            [new Claim(ClaimTypes.NameIdentifier, userId.ToString())],
            DateTime.Now,
            tokenExpiryTime));
        return new ClientUserExternalSignUpResponse(jwt, refreshToken, tokenExpiryTime).AsResponseData();
    }

// 注意：该控制器对所有提供者使用相同的回调操作，
// 但对于更喜欢为每个提供者使用不同操作的用户，
// 可以将以下操作拆分为多个操作。
    [HttpGet("/api/[controller]/callback/logout/{provider}")]
    [HttpPost("/api/[controller]/callback/logout/{provider}")]
    [IgnoreAntiforgeryToken]
    public async Task<ActionResult> LogOutCallback()
    {
        // 检索由OpenIddict在触发注销时创建的状态令牌中存储的数据。
        var result = await HttpContext.AuthenticateAsync(OpenIddictClientAspNetCoreDefaults.AuthenticationScheme);

        // 在此示例中，本地身份验证cookie始终在将用户代理重定向到授权服务器之前被移除。
        // 如果应用程序希望延迟移除本地cookie，可以从注销操作中删除相应的代码，并在此操作中移除身份验证cookie。
        return Redirect(result!.Properties!.RedirectUri ?? throw new InvalidOperationException());
    }
}