using Moq;
using SmartBuy.Common.Utilities.Repository;
using SmartBuy.OrderManagement.Domain;
using SmartBuy.OrderManagement.Rules.Tests.Helper;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SmartBuy.OrderManagement.Rules.Tests
{
    public class BaseOrderLinkRulesTests : IClassFixture<OrderDataFixture>
    {
        private readonly OrderDataFixture _orderData;
        private readonly Mock<IGenericReadRepository<GasStation>> _mockGasStationRepo;

        public BaseOrderLinkRulesTests(OrderDataFixture orderData)
        {
            var mockRepo = new MockRepoHelper(orderData);
            _mockGasStationRepo = mockRepo.MockGasStationsRepo;
            _orderData = orderData;
        }

        [Fact]
        public async Task ShouldPassOrderIfDispatcherGroupIsSame()
        {
            BaseOrderLinkRule baseRule = new BaseOrderLinkRule(_mockGasStationRepo.Object);
            var result = await baseRule.Validate(_orderData.InputOrders.Skip(1).Take(2));

            Assert.True(result);
        }

        [Fact]
        public async Task ShouldPassOrderIfDispatcherGroupAreDifferent()
        {
            BaseOrderLinkRule baseRule = new BaseOrderLinkRule(_mockGasStationRepo.Object);
            var result = await baseRule.Validate(_orderData.InputOrders);

            Assert.False(result);
        }
    }
}
