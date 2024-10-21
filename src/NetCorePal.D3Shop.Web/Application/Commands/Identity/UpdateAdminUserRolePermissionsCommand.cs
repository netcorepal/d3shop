using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity;

public record UpdateAdminUserRolePermissionsCommand(AdminUserId AdminUserId, RoleId RoleId, IEnumerable<AdminUserPermission> Permissions) : ICommand;

public class UpdateAdminUserRolePermissionsCommandHandler(IAdminUserRepository adminUserRepository)
    : ICommandHandler<UpdateAdminUserRolePermissionsCommand>
{
    public async Task Handle(UpdateAdminUserRolePermissionsCommand request, CancellationToken cancellationToken)
    {
        var adminUser = await adminUserRepository.GetAsync(request.AdminUserId, cancellationToken) ??
                        throw new KnownException($"用户不存在，AdminUserId={request.AdminUserId}");

        adminUser.UpdateRolePermissions(request.RoleId, request.Permissions);
    }
}