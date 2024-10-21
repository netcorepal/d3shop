using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;

namespace NetCorePal.D3Shop.Web.Controllers.Identity.Responses;

public record AdminUserRolesResponse(RoleId RoleId, string RoleName, string Description, bool IsAssigned);