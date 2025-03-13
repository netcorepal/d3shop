using MediatR;
using NetCorePal.D3Shop.Domain.DomainEvents.Identity.Admin;
using NetCorePal.D3Shop.Web.Application.Commands.Identity.Admin;
using NetCorePal.D3Shop.Web.Application.Queries.Identity.Admin;
using NetCorePal.Extensions.Domain;

namespace NetCorePal.D3Shop.Web.Application.DomainEventHandlers.Identity.Admin;

public class RoleInfoChangedDomainEventHandler(IMediator mediator, AdminUserQuery adminUserQuery)
    : IDomainEventHandler<RoleInfoChangedDomainEvent>
{
    public async Task Handle(RoleInfoChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        var role = notification.Role;
        var adminUserIds = await adminUserQuery.GetAdminUserIdsByRoleIdAsync(role.Id, cancellationToken);
        foreach (var adminUserId in adminUserIds)
            await mediator.Send(new UpdateAdminUserRoleInfoCommand(adminUserId, role.Id, role.Name),
                cancellationToken);
    }
}