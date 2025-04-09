using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;

namespace NetCorePal.D3Shop.Web.Controllers.Identity.VueAdmin.Responses
{
    public record VueAdminUserResponse(AdminUserId Id, string UserName, string RealName, IEnumerable<string> Roles,string HomePath);
  
}
