using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;

namespace NetCorePal.D3Shop.Web.Controllers.Identity.Responses;

public record RoleResponse(RoleId Id, string Name, string Description, IEnumerable<RolePermission> Permissions);