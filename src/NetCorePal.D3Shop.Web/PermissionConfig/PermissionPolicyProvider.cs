using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.Permission;

namespace NetCorePal.D3Shop.Web.PermissionConfig
{
    public class PermissionPolicyProvider(IOptions<AuthorizationOptions> options) : DefaultAuthorizationPolicyProvider(options)
    {
        public override Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (!policyName.StartsWith(AppClaim.Permission, StringComparison.CurrentCultureIgnoreCase))
                return GetDefaultPolicyAsync()!;

            var policy = new AuthorizationPolicyBuilder();
            policy.AddRequirements(new PermissionRequirement(policyName));
            return Task.FromResult(policy.Build())!;
        }
    }
}
