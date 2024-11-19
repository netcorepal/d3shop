using NetCorePal.D3Shop.Admin.Shared.Const;
using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;

namespace NetCorePal.D3Shop.Web.Admin.Client.Auth;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ClientPermissionAttribute : AuthorizeAttribute
{
    public ClientPermissionAttribute(string permissionCode)
        => Policy = $"{AppClaim.AdminPermission}.{permissionCode}";
}