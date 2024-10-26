using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using NetCorePal.D3Shop.Web.Const;

namespace NetCorePal.D3Shop.Web.PermissionConfig
{
    public class PermissionPolicyProvider(IOptions<AuthorizationOptions> options) : DefaultAuthorizationPolicyProvider(options)
    {
        public override Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (!policyName.StartsWith(AppClaim.AdminPermission, StringComparison.CurrentCultureIgnoreCase))
                return GetDefaultPolicyAsync()!;

            var policy = new AuthorizationPolicyBuilder();
            policy.AddRequirements(new PermissionRequirement(policyName.Split('.')[1]));
            return Task.FromResult(policy.Build())!;
        }
    }
}
