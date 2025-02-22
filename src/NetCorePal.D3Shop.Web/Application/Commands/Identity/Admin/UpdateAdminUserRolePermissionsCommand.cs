using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Admin;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Admin;

public record UpdateAdminUserRolePermissionsCommand(
    AdminUserId AdminUserId,
    RoleId RoleId,
    IEnumerable<string> PermissionCodes) : ICommand;

public class UpdateAdminUserRolePermissionsCommandHandler(IAdminUserRepository adminUserRepository)
    : ICommandHandler<UpdateAdminUserRolePermissionsCommand>
{
    public async Task Handle(UpdateAdminUserRolePermissionsCommand request, CancellationToken cancellationToken)
    {
        var adminUser = await adminUserRepository.GetAsync(request.AdminUserId, cancellationToken) ??
                        throw new KnownException($"用户不存在，AdminUserId={request.AdminUserId}");

        var permissions = request.PermissionCodes.Select(code => new AdminUserPermission(code, request.RoleId));

        adminUser.UpdateRolePermissions(request.RoleId, permissions);
    }
}