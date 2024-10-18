using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity;

public record UpdateRolePermissionsCommand(RoleId RoleId, IEnumerable<RolePermission> Permissions) : ICommand;

public class UpdateRolePermissionsCommandHandler(IRoleRepository roleRepository) : ICommandHandler<UpdateRolePermissionsCommand>
{
    public async Task Handle(UpdateRolePermissionsCommand request, CancellationToken cancellationToken)
    {
        var role = await roleRepository.GetAsync(request.RoleId, cancellationToken) ??
                   throw new KnownException($"角色不存在，RoleId={request.RoleId}");

        var currentRolePermissionCodes = role.Permissions.Select(p => p.PermissionCode).ToList();
        var targetRolePermissionCodes = request.Permissions.Select(p => p.PermissionCode).ToList();

        var removedPermissionCodes = currentRolePermissionCodes.Except(targetRolePermissionCodes).ToArray();
        if (removedPermissionCodes.Length != 0)
            role.RemoveRolePermissions(removedPermissionCodes);

        role.AddRolePermissions(request.Permissions);
        await roleRepository.UpdateAsync(role, cancellationToken);
    }
}