using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity;
using NetCorePal.D3Shop.Web.Application.Commands.Identity.Dto;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity;

public record UpdateRolePermissionsCommand(RoleId RoleId, IEnumerable<RolePermissionDto> Permissions) : ICommand;

public class UpdateRolePermissionsCommandHandler(IRoleRepository roleRepository)
    : ICommandHandler<UpdateRolePermissionsCommand>
{
    public async Task Handle(UpdateRolePermissionsCommand request, CancellationToken cancellationToken)
    {
        var role = await roleRepository.GetAsync(request.RoleId, cancellationToken) ??
                   throw new KnownException($"角色不存在，RoleId={request.RoleId}");

        var permissions = request.Permissions
            .Select(p => new RolePermission(p.PermissionCode, p.PermissionRemark));

        role.UpdateRolePermissions(permissions);
    }
}