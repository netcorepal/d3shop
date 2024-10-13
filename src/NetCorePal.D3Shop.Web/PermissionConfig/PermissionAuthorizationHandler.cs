using Microsoft.AspNetCore.Authorization;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.Permission;

namespace NetCorePal.D3Shop.Web.PermissionConfig
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            var permissions = context.User.Claims
                .Where(claim => claim.Type == AppClaim.Permission
                    && claim.Value == requirement.Permission
                    && claim.Issuer == "LOCAL AUTHORITY");
            if (permissions.Any())
            {
                context.Succeed(requirement);
                await Task.CompletedTask;
            }
        }
    }
}
