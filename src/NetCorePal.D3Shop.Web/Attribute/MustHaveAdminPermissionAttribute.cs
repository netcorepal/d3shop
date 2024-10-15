using Microsoft.AspNetCore.Authorization;
using NetCorePal.D3Shop.Web.Const;

namespace NetCorePal.D3Shop.Web.Attribute
{
    public class MustHaveAdminPermissionAttribute : AuthorizeAttribute
    {
        public MustHaveAdminPermissionAttribute(string permissionCode)
            => Policy = $"{AppClaim.AdminPermission}.{permissionCode}";
    }
}
