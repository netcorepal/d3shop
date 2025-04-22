using AntDesign.Charts;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.MenuAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;

namespace NetCorePal.D3Shop.Web.Controllers.Identity.VueAdmin.Responses
{
    public class VueRoleResponse(RoleId id, string name, int status, string remark, IEnumerable<MenuId> permissions, DateTimeOffset createTime) 
    {
        public RoleId Id { get; } = id;
        public string Name { get; set; } = name;
        public int Status { get; set; } = status;

        public string Remark { get; set; } = remark;


        public IEnumerable<MenuId> Permissions { get; set; } = permissions;
        public string CreateTime { get; set; } = createTime.ToString("yyyy-MM-dd HH:mm:ss");
    }
    
}
