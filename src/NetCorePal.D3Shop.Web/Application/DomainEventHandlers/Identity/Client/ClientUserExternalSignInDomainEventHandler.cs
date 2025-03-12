using MediatR;
using NetCorePal.D3Shop.Domain.DomainEvents.Identity.Client;
using NetCorePal.D3Shop.Web.Application.Commands.Identity.Client;
using NetCorePal.Extensions.Domain;

namespace NetCorePal.D3Shop.Web.Application.DomainEventHandlers.Identity.Client;

public class ClientUserExternalSignInDomainEventHandler(IMediator mediator)
    : IDomainEventHandler<ClientUserExternalSignInDomainEvent>
{
    public async Task Handle(ClientUserExternalSignInDomainEvent notification, CancellationToken cancellationToken)
    {
        await mediator.Send(new RecordClientUserLoginCommand(notification.UserId, notification.NickName,
            notification.LoginTime,
            notification.LoginMethod, notification.IpAddress, notification.UserAgent), cancellationToken);
    }
}