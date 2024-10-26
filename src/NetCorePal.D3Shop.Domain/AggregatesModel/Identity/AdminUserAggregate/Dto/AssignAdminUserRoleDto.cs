using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;

namespace NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate.Dto
{
    public record AssignAdminUserRoleDto(RoleId RoleId, string RoleName, IEnumerable<AdminUserPermission> Permissions);
}
