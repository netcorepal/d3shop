using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;

namespace NetCorePal.D3Shop.Admin.Shared.Authorization;

public class PermissionAuthorizationHandler(IPermissionChecker permissionChecker)
    : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        if (context.User.Identity?.IsAuthenticated is null or false)
        {
            context.Fail();
            return;
        }

        // 系统默认用户不校验权限
        var name = context.User.Claims.Single(c => c.Type == ClaimTypes.Name).Value;
        if (name == AppDefaultCredentials.Name)
        {
            context.Succeed(requirement);
            return;
        }

        // 检查用户是否拥有指定权限
        var hasPermission =
            await permissionChecker.HasPermissionAsync(context.User, requirement.PermissionCode);
        if (hasPermission)
        {
            context.Succeed(requirement);
        }
    }
}