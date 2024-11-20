using DotNetCore.CAP;
using MediatR;
using NetCorePal.D3Shop.Web.Application.Commands;
using NetCorePal.Extensions.DistributedTransactions;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.IntegrationEventHandlers
{
    public class OrderPaidIntegrationEventHandler(IMediator mediator) : IIntegrationEventHandler<OrderPaidIntegrationEvent>
    {
        public Task HandleAsync(OrderPaidIntegrationEvent eventData, CancellationToken cancellationToken = default)
        {
            var cmd = new OrderPaidCommand(eventData.OrderId);
            return mediator.Send(cmd, cancellationToken);
        }
    }
}