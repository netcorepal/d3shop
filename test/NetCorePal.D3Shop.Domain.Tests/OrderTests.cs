using NetCorePal.D3Shop.Domain.AggregatesModel.OrderAggregate;
namespace NetCorePal.D3Shop.Domain.Tests
{
    public class OrderTests
    {
        [Fact]
        public void OrderPaid_Test()
        {
            Order order = new("test", 1);
            Assert.False(order.Paid);
            order.OrderPaid();
            Assert.True(order.Paid);
        }
    }
}