using NetCorePal.D3Shop.Domain.AggregatesModel.OrderAggregate;
using NetCorePal.D3Shop.Web.Application.Commands;
using NetCorePal.Extensions.Mappers;

namespace NetCorePal.D3Shop.Web.Application.Mappers
{
    public class CreateOrderCommandToOrder : IMapper<CreateOrderCommand, Order>
    {
        public Order To(CreateOrderCommand from)
        {
            return new Order(from.Name, from.Count);
        }
    }
}
