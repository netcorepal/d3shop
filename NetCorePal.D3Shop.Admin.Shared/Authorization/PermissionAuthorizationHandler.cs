using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using NetCorePal.D3Shop.Admin.Shared.Const;

namespace NetCorePal.D3Shop.Admin.Shared.Authorization
{
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

            //超级管理员
            if (context.User.Claims.Any(
                    claim => claim is { Type: ClaimTypes.Role, Value: AppClaim.SuperAdminRole }))
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
}