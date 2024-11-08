using Microsoft.AspNetCore.Authorization;

namespace NetCorePal.D3Shop.Admin.Shared.PermissionConfig
{
    public class PermissionRequirement(string permission) : IAuthorizationRequirement
    {
        public string Permission => permission;
    }
}
