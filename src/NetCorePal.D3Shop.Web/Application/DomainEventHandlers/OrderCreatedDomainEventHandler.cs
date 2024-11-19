using MediatR;
using NetCorePal.D3Shop.Domain.DomainEvents;
using NetCorePal.D3Shop.Web.Application.Commands;
using NetCorePal.Extensions.Domain;

namespace NetCorePal.D3Shop.Web.Application.DomainEventHandlers
{
    internal class OrderCreatedDomainEventHandler(IMediator mediator) : IDomainEventHandler<OrderCreatedDomainEvent>
    {
        public Task Handle(OrderCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            return mediator.Send(new DeliverGoodsCommand(notification.Order.Id), cancellationToken);
        }
    }
}