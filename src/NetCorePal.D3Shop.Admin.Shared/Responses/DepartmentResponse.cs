using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;

namespace NetCorePal.D3Shop.Admin.Shared.Responses;

public class DepartmentResponse(DeptId id, string name, string description)
{
    public DeptId Id { get; } = id;
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
}