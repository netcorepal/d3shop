using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.MenuAggregate;

namespace NetCorePal.D3Shop.Web.Controllers.Identity.VueAdmin.Requests
{
    public class VueUpdateRoleRequest
    {
        public string Name { get; set; } = string.Empty;
        public int Status { get; set; }
        public string Remark { get; set; } = string.Empty;
        public IEnumerable<MenuId> Permissions { get; set; } = [];
    }
}