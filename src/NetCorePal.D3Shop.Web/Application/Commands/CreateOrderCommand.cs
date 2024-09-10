using NetCorePal.D3Shop.Domain.AggregatesModel.OrderAggregate;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands
{
    public record CreateOrderCommand(string Name, int Price, int Count) : ICommand<OrderId>;
}
