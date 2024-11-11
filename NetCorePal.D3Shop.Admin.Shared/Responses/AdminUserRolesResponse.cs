using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;

namespace NetCorePal.D3Shop.Admin.Shared.Responses;

public record AdminUserRolesResponse(RoleId RoleId, string RoleName, string Description, bool IsAssigned);