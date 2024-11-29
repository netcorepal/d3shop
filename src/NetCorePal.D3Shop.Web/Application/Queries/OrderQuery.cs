using NetCorePal.D3Shop.Domain.AggregatesModel.OrderAggregate;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Queries
{
    public class OrderQuery(ApplicationDbContext applicationDbContext) : IQuery
    {
        public async Task<Order?> QueryOrder(OrderId orderId, CancellationToken cancellationToken)
        {
            return await applicationDbContext.Orders.FindAsync(new object[] { orderId }, cancellationToken);
        }
    }
}
