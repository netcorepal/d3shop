using Microsoft.AspNetCore.Authorization;

namespace NetCorePal.D3Shop.Web.PermissionConfig
{
    public class PermissionRequirement(string permission) : IAuthorizationRequirement
    {
        public string Permission => permission;
    }
}
