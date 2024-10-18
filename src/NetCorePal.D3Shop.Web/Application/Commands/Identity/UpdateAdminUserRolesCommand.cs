using FluentValidation;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate.Dto;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity;

public record UpdateAdminUserRolesCommand(AdminUserId AdminUserId, List<AssignAdminUserRoleDto> RolesToBeAssigned) : ICommand;

public class UpdateAdminUserRolesCommandValidator : AbstractValidator<UpdateAdminUserRolesCommand>
{
    public UpdateAdminUserRolesCommandValidator()
    {
        RuleFor(x => x.AdminUserId).NotEmpty();
        RuleFor(x => x.RolesToBeAssigned).NotEmpty();
    }
}

public class UpdateAdminUserRolesCommandHandler(IAdminUserRepository adminUserRepository) : ICommandHandler<UpdateAdminUserRolesCommand>
{
    public async Task Handle(UpdateAdminUserRolesCommand request, CancellationToken cancellationToken)
    {
        var adminUser = await adminUserRepository.GetAsync(request.AdminUserId, cancellationToken)
                        ?? throw new KnownException($"未找到用户，AdminUserId = {request.AdminUserId}");

        var targetRoleIds = request.RolesToBeAssigned.Select(r => r.RoleId).ToArray();
        var currentRoleIds = adminUser.Roles.Select(r => r.RoleId).ToArray();

        var roleIdsToBeRemove = currentRoleIds.Except(targetRoleIds).ToArray();
        if (roleIdsToBeRemove.Length != 0) adminUser.RemoveRoles(roleIdsToBeRemove);

        adminUser.AddRoles(request.RolesToBeAssigned);

        await adminUserRepository.UpdateAsync(adminUser, cancellationToken);
    }

}