using NetCorePal.D3Shop.Domain.AggregatesModel.OrderAggregate;

namespace NetCorePal.D3Shop.Web.Application.IntegrationEventHandlers
{
    public record OrderPaidIntegrationEvent(OrderId OrderId);
}
