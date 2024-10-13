using Microsoft.AspNetCore.Authorization;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.Permission;

namespace NetCorePal.D3Shop.Web.Attribute
{
    public class MustHavePermissionAttribute : AuthorizeAttribute
    {
        public MustHavePermissionAttribute(string feature, string action)
            => Policy = Permission.CodeFor(feature, action);        
    }
}
