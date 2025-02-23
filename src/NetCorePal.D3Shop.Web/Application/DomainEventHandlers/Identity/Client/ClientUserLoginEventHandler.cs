using MediatR;
using NetCorePal.D3Shop.Domain.DomainEvents.Identity.Client;
using NetCorePal.D3Shop.Web.Application.Commands.Identity.Client;
using NetCorePal.Extensions.Domain;

namespace NetCorePal.D3Shop.Web.Application.DomainEventHandlers.Identity.Client;

public class ClientUserLoginEventHandler(IMediator mediator)
    : IDomainEventHandler<ClientUserLoginEvent>
{
    public async Task Handle(ClientUserLoginEvent notification, CancellationToken cancellationToken)
    {
        await mediator.Send(new RecordClientUserLoginCommand(notification.UserId, notification.NickName,
            notification.LoginTime,
            notification.LoginMethod, notification.IpAddress, notification.UserAgent), cancellationToken);
    }
}