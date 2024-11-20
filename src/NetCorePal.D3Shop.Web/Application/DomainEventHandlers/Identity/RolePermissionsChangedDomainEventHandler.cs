using MediatR;
using Microsoft.Extensions.Caching.Memory;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.DomainEvents.Identity;
using NetCorePal.D3Shop.Web.Application.Commands.Identity;
using NetCorePal.D3Shop.Web.Application.Queries.Identity;
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
        var adminUsers = await adminUserQuery.GetAdminUserByRoleIdAsync(roleId, cancellationToken);
        var permissions = notification.Role.Permissions
            .Select(p => new AdminUserPermission(p.PermissionCode, p.PermissionRemark))
            .ToArray();
        await Task.WhenAll(
            adminUsers.Select(async adminUser =>
            {
                memoryCache.Remove($"{CacheKeys.AdminUserPermissions}:{adminUser.Id}");
                await mediator.Send(new UpdateAdminUserRolePermissionsCommand(adminUser.Id, roleId, permissions),
                    cancellationToken);
            })
        );
    }
}