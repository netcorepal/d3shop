using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Admin.Dto;

public record AssignAdminUserRoleDto(RoleId RoleId, string RoleName, IEnumerable<string> PermissionCodes);