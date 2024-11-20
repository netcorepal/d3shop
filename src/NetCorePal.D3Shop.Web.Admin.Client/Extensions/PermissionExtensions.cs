using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using NetCorePal.D3Shop.Admin.Shared.Const;

namespace NetCorePal.D3Shop.Web.Admin.Client.Extensions;

internal static class PermissionExtensions
{
    internal static bool CheckPermission(this AuthenticationState authenticationState, string permission)
    {
        return authenticationState.User.Claims
                   .Any(claim => claim is { Type: ClaimTypes.Role, Value: AppClaim.SuperAdminRole })
               || authenticationState.User.HasClaim(AppClaim.AdminPermission, permission);
    }
}