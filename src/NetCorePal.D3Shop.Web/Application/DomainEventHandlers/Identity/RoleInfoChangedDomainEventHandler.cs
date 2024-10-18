using MediatR;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.DomainEvents.Identity;
using NetCorePal.D3Shop.Web.Application.Commands.Identity;
using NetCorePal.D3Shop.Web.Application.Queries.Identity;
using NetCorePal.Extensions.Domain;

namespace NetCorePal.D3Shop.Web.Application.DomainEventHandlers.Identity;

public class RoleInfoChangedDomainEventHandler(IMediator mediator, AdminUserQuery adminUserQuery) : IDomainEventHandler<RoleInfoChangedDomainEvent>
{
    public async Task Handle(RoleInfoChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        var role = notification.Role;
        var adminUsers = await adminUserQuery.GetAdminUserByRoleIdAsync(role.Id, cancellationToken);
        foreach (var adminUser in adminUsers)
        {
            await mediator.Send(new UpdateAdminUserRoleInfoCommand(
                    new AdminUserRole(adminUser.Id, role.Id, role.Name)),
                cancellationToken);
        }
    }
}