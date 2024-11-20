using System.Threading;
using NetCorePal.D3Shop.Domain;
using NetCorePal.D3Shop.Domain.AggregatesModel.OrderAggregate;
using NetCorePal.D3Shop.Infrastructure;

namespace NetCorePal.D3Shop.Web.Application.Queries
{
    public class OrderQuery(ApplicationDbContext applicationDbContext)
    {
        public async Task<Order?> QueryOrder(OrderId orderId, CancellationToken cancellationToken)
        {
            return await applicationDbContext.Orders.FindAsync(new object[] { orderId }, cancellationToken);
        }
    }
}
