using System.Security.Claims;
using AntDesign;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using NetCorePal.D3Shop.Admin.Shared.Requests;
using NetCorePal.D3Shop.Web.Application.Queries.Identity.Admin;
using NetCorePal.D3Shop.Web.Controllers.Identity.Admin.Dto;
using NetCorePal.D3Shop.Web.Helper;

namespace NetCorePal.D3Shop.Web.Blazor.Components.Account;

public partial class Login
{
    [SupplyParameterFromForm(FormName = "loginForm")]
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
    private AdminUserLoginRequest LoginModel { get; set; } = new();

    [CascadingParameter] private HttpContext HttpContext { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private AdminUserQuery AdminUserQuery { get; set; } = default!;
    [Inject] private IOptions<AppConfiguration> AppConfiguration { get; set; } = default!;
    [Inject] private MessageService Message { get; set; } = default!;

    public async Task HandleSubmit()
    {
        var user = await AdminUserQuery.GetUserInfoForLoginAsync(LoginModel.Name, CancellationToken.None);
        if (user is null)
        {
            _ = Message.Error("Invalid Credentials.");
            return;
        }

        if (!PasswordHasher.VerifyHashedPassword(LoginModel.Password, user.Password))
        {
            _ = Message.Error("Invalid Credentials.");
            return;
        }

        var claims = GetClaimsAsync(user);
        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            AllowRefresh = true,
            ExpiresUtc = DateTime.UtcNow.AddMinutes(AppConfiguration.Value.TokenExpiryInMinutes),
            IsPersistent = LoginModel.IsPersistent
        };
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        NavigationManager.NavigateTo("/admin", forceLoad: true);
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