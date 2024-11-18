using Microsoft.AspNetCore.Authorization;
using NetCorePal.D3Shop.Admin.Shared.PermissionConfig;

namespace NetCorePal.D3Shop.Admin.Shared.Attribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AdminPermissionAttribute(params string[] permissionCodes)
        : AuthorizeAttribute, IAuthorizationRequirementData
    {
        private IReadOnlyList<string> PermissionCodes { get; } = permissionCodes;

        public IEnumerable<IAuthorizationRequirement> GetRequirements()
        {
            return PermissionCodes.Select(code => new PermissionRequirement(code));
        }
    }
}