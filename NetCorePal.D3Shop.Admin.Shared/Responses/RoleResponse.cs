using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;

namespace NetCorePal.D3Shop.Admin.Shared.Responses;

public class RoleResponse(RoleId id, string name, string description, IEnumerable<RolePermission> permissions)
{
    public RoleId Id { get; } = id;
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
    public IEnumerable<RolePermission> Permissions { get; set; } = permissions;
}