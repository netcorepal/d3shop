using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;

namespace NetCorePal.D3Shop.Admin.Shared.Responses;

public class AdminUserRoleResponse(RoleId roleId, string roleName, bool isAssigned)
{
    public RoleId RoleId { get; } = roleId;
    public string RoleName { get; } = roleName;
    public bool IsAssigned { get; set; } = isAssigned;
};