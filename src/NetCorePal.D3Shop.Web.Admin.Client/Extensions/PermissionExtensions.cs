using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using NetCorePal.D3Shop.Admin.Shared.Const;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;

namespace NetCorePal.D3Shop.Web.Admin.Client.Extensions;

internal static class PermissionExtensions
{
    internal static bool CheckPermission(this AuthenticationState authenticationState, string permission)
    {
        // 系统默认用户不校验权限
        var name = authenticationState.User.Claims.Single(c => c.Type == ClaimTypes.Name).Value;
        return name == AppDefaultCredentials.Name ||
               authenticationState.User.HasClaim(AppClaim.AdminPermission, permission);
    }
}