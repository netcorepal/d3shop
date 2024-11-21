using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;

namespace NetCorePal.D3Shop.Admin.Shared.Responses;

public class RoleResponse(RoleId id, string name, string description)
{
    public RoleId Id { get; } = id;
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
}