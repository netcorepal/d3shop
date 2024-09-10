using NetCorePal.Extensions.Domain;
using NetCorePal.D3Shop.Domain.AggregatesModel.OrderAggregate;

namespace NetCorePal.D3Shop.Domain.DomainEvents
{
    public record OrderCreatedDomainEvent(Order Order) : IDomainEvent;
}
