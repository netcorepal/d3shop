using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.MenuAggregate;

namespace NetCorePal.D3Shop.Web.Controllers.Identity.VueAdmin.Requests
{
    public class VueCreateRoleRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTimeOffset CreateTime { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; } = string.Empty;

        public IEnumerable<MenuId> Permissions { get; set; } = [];
    }
}
