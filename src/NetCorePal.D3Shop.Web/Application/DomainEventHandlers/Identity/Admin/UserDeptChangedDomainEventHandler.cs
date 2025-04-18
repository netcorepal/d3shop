using MediatR;
using NetCorePal.D3Shop.Admin.Shared.Permission;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.D3Shop.Domain.DomainEvents.Identity.Admin;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Admin;
using NetCorePal.D3Shop.Web.Application.Commands.Identity.Admin;
using NetCorePal.D3Shop.Web.Application.Queries.Identity.Admin;
using NetCorePal.D3Shop.Web.Const;
using NetCorePal.Extensions.Domain;

namespace NetCorePal.D3Shop.Web.Application.DomainEventHandlers.Identity.Admin;
public class UserDeptChangedDomainEventHandler(
    IMediator mediator,
    UserDeptQuery userDeptQuery) : IDomainEventHandler<UserDeptChangedDomainEvent>
{
    public async Task Handle(UserDeptChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        var deptId = notification.UserDept.DeptId;
        var userCount = await userDeptQuery.GetUserCount(deptId, cancellationToken);
        await mediator.Send(new UpdateDeptUserCountCommand(deptId, userCount), cancellationToken);
    }
}