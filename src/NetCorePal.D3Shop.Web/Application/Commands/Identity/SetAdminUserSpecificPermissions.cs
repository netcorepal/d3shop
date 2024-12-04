using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity;
using NetCorePal.D3Shop.Web.Application.Commands.Identity.Dto;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity;

public record SetAdminUserSpecificPermissions(AdminUserId Id, IEnumerable<AdminUserPermissionDto> Permissions)
    : ICommand;

public class SetAdminUserSpecificPermissionsCommandHandler(IAdminUserRepository adminUserRepository)
    : ICommandHandler<SetAdminUserSpecificPermissions>
{
    public async Task Handle(SetAdminUserSpecificPermissions request, CancellationToken cancellationToken)
    {
        var adminUser = await adminUserRepository.GetAsync(request.Id, cancellationToken) ??
                        throw new KnownException($"用户不存在，AdminUserId={request.Id}");

        var permissions = request.Permissions
            .Select(p => new AdminUserPermission(p.PermissionCode, p.PermissionRemark));

        adminUser.SetSpecificPermissions(permissions);
    }
}