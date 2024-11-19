using Microsoft.AspNetCore.Authorization;

namespace NetCorePal.D3Shop.Admin.Shared.Authorization
{
    public class PermissionRequirement(string permissionCode) : IAuthorizationRequirement
    {
        public string PermissionCode { get; } = permissionCode;
    }
}
