using Microsoft.AspNetCore.Authorization;
using NetCorePal.D3Shop.Admin.Shared.Const;

namespace NetCorePal.D3Shop.Admin.Shared.Attribute
{
    public class MustHaveAdminPermissionAttribute : AuthorizeAttribute
    {
        public MustHaveAdminPermissionAttribute(string permissionCode)
            => Policy = $"{AppClaim.AdminPermission}.{permissionCode}";
    }
}
