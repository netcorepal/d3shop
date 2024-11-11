using Microsoft.AspNetCore.Components.Authorization;
using NetCorePal.D3Shop.Admin.Shared.Const;

namespace NetCorePal.D3Shop.Web.Admin.Client.Auth;

public interface IAccessTokenProvider
{
    Task<string> GetAccessTokenAsync();
};

public class AccessTokenProvider(AuthenticationStateProvider authenticationStateProvider) : IAccessTokenProvider
{
    public async Task<string> GetAccessTokenAsync()
    {
        var authenticationState = await authenticationStateProvider.GetAuthenticationStateAsync();
        if (authenticationState.User.Identity is not { IsAuthenticated: true }) return string.Empty;
        var user = authenticationState.User;
        return user.Claims.Single(c => c.Type == AppClaim.AccessToken).Value;

    }
}