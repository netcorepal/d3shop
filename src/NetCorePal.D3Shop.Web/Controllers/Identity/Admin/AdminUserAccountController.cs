using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NetCorePal.D3Shop.Admin.Shared.Requests;
using NetCorePal.D3Shop.Web.Application.Queries.Identity.Admin;
using NetCorePal.D3Shop.Web.Controllers.Identity.Admin.Dto;
using NetCorePal.D3Shop.Web.Helper;
using NetCorePal.Extensions.Dto;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Controllers.Identity.Admin;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AdminUserAccountController(
    AdminUserQuery adminUserQuery,
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
}