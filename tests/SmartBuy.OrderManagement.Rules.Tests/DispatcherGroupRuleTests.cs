using Moq;
using SmartBuy.Common.Utilities.Repository;
using SmartBuy.OrderManagement.Domain;
using SmartBuy.OrderManagement.Rules.Tests.Helper;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace SmartBuy.OrderManagement.Rules.Tests
{
    public class DispatcherGroupRuleTests : IClassFixture<OrderDataFixture>
    {
        private readonly OrderDataFixture _orderData;
        private readonly Mock<IGenericReadRepository<GasStation>> _mockGasStationRepo;

        public DispatcherGroupRuleTests(OrderDataFixture orderData)
        {
            var mockRepo = new MockRepoHelper(orderData);
            _mockGasStationRepo = mockRepo.MockGasStationsRepo;
            _orderData = orderData;
        }

        [Fact]
        public async Task ShouldPassOrdersIfDispatcherGroupIsSame()
        {
            DispatcherGroupRule dispatcherGroupRule = new DispatcherGroupRule(_mockGasStationRepo.Object);

            var result = await dispatcherGroupRule.IsDispatcherSame(
                _orderData.InputOrders.Take(2));

            _mockGasStationRepo.Verify(x =>
            x.FindByAsync(It.IsAny<Expression<Func<GasStation, bool>>>()), Times.Once);
            Assert.True(result);
        }

        [Fact]
        public async Task ShouldNotPassOrdersIfDispatcherGroupAreDifferent()
        {
            DispatcherGroupRule dispatcherGroupRule = new DispatcherGroupRule(_mockGasStationRepo.Object);
            var result = await dispatcherGroupRule.IsDispatcherSame(_orderData.InputOrders);

            Assert.False(result);
        }
    }
}
