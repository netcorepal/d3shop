using FluentValidation;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity;
using NetCorePal.D3Shop.Web.Application.Commands.Identity.Dto;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity;

public record UpdateAdminUserRolesCommand(AdminUserId AdminUserId, List<AssignAdminUserRoleDto> RolesToBeAssigned)
    : ICommand;

public class UpdateAdminUserRolesCommandValidator : AbstractValidator<UpdateAdminUserRolesCommand>
{
    public UpdateAdminUserRolesCommandValidator()
    {
        RuleFor(x => x.AdminUserId).NotEmpty();
    }
}

public class UpdateAdminUserRolesCommandHandler(IAdminUserRepository adminUserRepository)
    : ICommandHandler<UpdateAdminUserRolesCommand>
{
    public async Task Handle(UpdateAdminUserRolesCommand request, CancellationToken cancellationToken)
    {
        var adminUser = await adminUserRepository.GetAsync(request.AdminUserId, cancellationToken)
                        ?? throw new KnownException($"未找到用户，AdminUserId = {request.AdminUserId}");

        List<AdminUserRole> roles = [];
        List<AdminUserPermission> permissions = [];

        foreach (var assignAdminUserRoleDto in request.RolesToBeAssigned)
        {
            roles.Add(new AdminUserRole(assignAdminUserRoleDto.RoleId, assignAdminUserRoleDto.RoleName));
            permissions.AddRange(assignAdminUserRoleDto.Permissions.Select(permission =>
                new AdminUserPermission(permission.PermissionCode, permission.PermissionRemark,
                    assignAdminUserRoleDto.RoleId)));
        }

        adminUser.UpdateRoles(roles, permissions);
    }
}