using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.MenuAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;

namespace NetCorePal.D3Shop.Admin.Shared.Responses;

//public class RoleResponse(RoleId id, string name, string description)
//{
//    public RoleId Id { get; } = id;
//    public string Name { get; set; } = name;
//    public string Description { get; set; } = description;
//}

public class RoleResponse(RoleId id, string name, int status, string description, IEnumerable<MenuId> permissions, DateTimeOffset createTime)
{
    public RoleId Id { get; } = id;
    public string Name { get; set; } = name;
    public int Status { get; set; } = status;

    public string Description { get; set; } = description;

    public IEnumerable<MenuId> Permissions { get; set; } = permissions;

    public string CreateTime { get; set; } = createTime.ToString("yyyy-MM-dd HH:mm:ss");
}