using MediatR;
using Microsoft.Extensions.Caching.Memory;
using NetCorePal.D3Shop.Domain.DomainEvents.Identity.Admin;
using NetCorePal.D3Shop.Web.Application.Commands.Identity.Admin;
using NetCorePal.D3Shop.Web.Application.Queries.Identity.Admin;
using NetCorePal.D3Shop.Web.Const;
using NetCorePal.Extensions.Domain;

namespace NetCorePal.D3Shop.Web.Application.DomainEventHandlers.Identity;

public class RolePermissionsChangedDomainEventHandler(
    IMediator mediator,
    AdminUserQuery adminUserQuery,
    IMemoryCache memoryCache) : IDomainEventHandler<RolePermissionChangedDomainEvent>
{
    public async Task Handle(RolePermissionChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        var roleId = notification.Role.Id;
        var adminUserIds = await adminUserQuery.GetAdminUserIdsByRoleIdAsync(roleId, cancellationToken);
        var permissionCodes = notification.Role.Permissions
            .Select(p => p.PermissionCode)
            .ToArray();

        foreach (var adminUserId in adminUserIds)
        {
            memoryCache.Remove($"{CacheKeys.AdminUserPermissions}:{adminUserId}");
            await mediator.Send(new UpdateAdminUserRolePermissionsCommand(adminUserId, roleId, permissionCodes),
                cancellationToken);
        }
    }
}