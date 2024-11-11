using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;

namespace NetCorePal.D3Shop.Admin.Shared.Responses;

public record AdminUserResponse(AdminUserId Id, string Name, string Phone, IEnumerable<AdminUserRole> Roles, IEnumerable<AdminUserPermission> Permissions);