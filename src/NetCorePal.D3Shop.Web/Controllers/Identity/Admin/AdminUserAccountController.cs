using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NetCorePal.D3Shop.Admin.Shared.Requests;
using NetCorePal.D3Shop.Admin.Shared.Responses;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.D3Shop.Web.Application.Queries.Identity.Admin;
using NetCorePal.D3Shop.Web.Controllers.Identity.Admin.Dto;
using NetCorePal.D3Shop.Web.Helper;
using NetCorePal.Extensions.Dto;
using NetCorePal.Extensions.Jwt;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Controllers.Identity.Admin;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AdminUserAccountController(
    AdminUserQuery adminUserQuery,
    IJwtProvider jwtProvider,
    IOptions<AppConfiguration> appConfiguration) : ControllerBase
{
    private AppConfiguration AppConfiguration => appConfiguration.Value;

    [HttpPost("login")]
    public async Task<ResponseData> LoginAsync([FromBody] AdminUserLoginRequest request)
    {
        var user = await adminUserQuery.GetUserInfoForLoginAsync(request.Name, HttpContext.RequestAborted);
        if (user is null)
            throw new KnownException("Invalid Credentials.");

        if (!PasswordHasher.VerifyHashedPassword(request.Password, user.Password))
            throw new KnownException("Invalid Credentials.");

        var claims = GetClaimsAsync(user);
        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            AllowRefresh = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(AppConfiguration.TokenExpiryInMinutes),
            IsPersistent = request.IsPersistent
        };
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
        return new ResponseData();
    }

    private static List<Claim> GetClaimsAsync(AuthenticationUserInfo user)
    {
        var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.MobilePhone, user.Phone)
            };

        return claims;
    }

    /// <summary>
    /// vue-admin登录
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="KnownException"></exception>
    [HttpPost("auth/login")]
    public async Task<ResponseData<AdminUserLoginResponse>> Login([FromBody] AdminUserLoginRequest request)
    {
        //重定向地址
        var redirectUri = HttpContext.Request?.Query["redirect"].FirstOrDefault();
        if (string.IsNullOrEmpty(redirectUri))
        {
            redirectUri = "/analytics";
        }
        var authInfo =
            await adminUserQuery.GetUserInfoForLoginAsync(request.Name, HttpContext.RequestAborted);
        if (authInfo is null)
            throw new KnownException("无效的用户", -1);

        if (!PasswordHasher.VerifyHashedPassword(request.Password, authInfo.Password))
            throw new KnownException("密码错误", -1);

        var refreshToken = TokenGenerator.GenerateRefreshToken();
        ICollection<RoleId> roles = await adminUserQuery.GetAssignedRoleIdsAsync(authInfo.Id, HttpContext.RequestAborted);
        var tokenExpiryTime = DateTimeOffset.Now.AddMinutes(appConfiguration.Value.TokenExpiryInMinutes);
        var token = await jwtProvider.GenerateJwtToken(new JwtData("issuer-x", "audience-y",
         [new Claim(ClaimTypes.NameIdentifier, authInfo.Id.ToString()), new Claim(ClaimTypes.Name, authInfo.Name)],
         DateTimeOffset.Now.DateTime,
         tokenExpiryTime.DateTime));
        return AdminUserLoginResponse.Success(token, refreshToken, authInfo.Id, authInfo.Name, authInfo.Name, roles, redirectUri).AsResponseData();
    }


    /// <summary>
    /// vue-admin退出登录
    /// </summary>
    /// <returns></returns>
    [HttpPost("/api/auth/logout")]
    public ActionResult<object> Logout()
    {
        try
        {
            // 清除Refresh Token Cookie
            Response.Cookies.Delete("refreshToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            return Ok(new { code = 0, data = new { }, error = "", message = "ok" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { code = -1, message = "服务器内部错误", error = ex.Message });
        }
    }

}