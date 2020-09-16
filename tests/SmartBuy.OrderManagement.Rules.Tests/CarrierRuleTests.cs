using SmartBuy.OrderManagement.Rules.Tests.Helper;
using System.Linq;
using Xunit;

namespace SmartBuy.OrderManagement.Rules.Tests
{
    public class CarrierRuleTests : IClassFixture<OrderDataFixture>
    {
        private readonly OrderDataFixture _orderData;
        public CarrierRuleTests(OrderDataFixture orderData)
        {
            _orderData = orderData;
        }

        [Fact]
        public void ShouldPassOrdersIfCarrierIsSame()
        {
            CarrierRule carrierRule = new CarrierRule();
            var result = carrierRule.IsCarrierSame(_orderData.InputOrders.Take(2));

            Assert.True(result);
        }

        [Fact]
        public void ShouldNotPassOrdersIfCarrierAreDifferent()
        {
            CarrierRule carrierRule = new CarrierRule();
            var result = carrierRule.IsCarrierSame(_orderData.InputOrders);

            Assert.False(result);
        }
    }
}
