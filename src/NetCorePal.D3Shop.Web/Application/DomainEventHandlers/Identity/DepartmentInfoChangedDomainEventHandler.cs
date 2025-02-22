using MediatR;
using NetCorePal.D3Shop.Domain.DomainEvents.Identity.Admin;
using NetCorePal.D3Shop.Web.Application.Commands.Identity.Admin;
using NetCorePal.D3Shop.Web.Application.Queries.Identity.Admin;
using NetCorePal.Extensions.Domain;

namespace NetCorePal.D3Shop.Web.Application.DomainEventHandlers.Identity;

public class DepartmentInfoChangedDomainEventHandler(IMediator mediator, AdminUserQuery adminUserQuery)
    : IDomainEventHandler<DepartmentInfoChangedDomainEvent>
{
    public async Task Handle(DepartmentInfoChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        var department = notification.Department;
        var adminUserIds = await adminUserQuery.GetUserIdsByDeptIdAsync(department.Id, cancellationToken);
        foreach (var adminUserId in adminUserIds)
            await mediator.Send(new UpdateUserDeptInfoCommand(adminUserId, department.Id, department.Name),
                cancellationToken);
    }
}