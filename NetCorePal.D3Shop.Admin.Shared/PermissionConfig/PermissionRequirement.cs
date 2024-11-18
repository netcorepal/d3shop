using Microsoft.AspNetCore.Authorization;

namespace NetCorePal.D3Shop.Admin.Shared.PermissionConfig
{
    public class PermissionRequirement(string permissionCode) : IAuthorizationRequirement
    {
        public string PermissionCode => permissionCode;
    }
}
