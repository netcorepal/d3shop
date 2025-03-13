using FluentValidation;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Admin;
using NetCorePal.D3Shop.Web.Application.Commands.Identity.Admin.Dto;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Admin;

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
            permissions.AddRange(assignAdminUserRoleDto.PermissionCodes.Select(code =>
                new AdminUserPermission(code, assignAdminUserRoleDto.RoleId)));
        }

        adminUser.UpdateRoles(roles, permissions);
    }
}