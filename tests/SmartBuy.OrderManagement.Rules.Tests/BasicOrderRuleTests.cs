using Moq;
using SmartBuy.Common.Utilities.Repository;
using SmartBuy.OrderManagement.Domain;
using SmartBuy.OrderManagement.Rules.Tests.Helper;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SmartBuy.OrderManagement.Rules.Tests
{
    public class BasicOrderRuleTests : IClassFixture<OrderDataFixture>
    {
        private readonly OrderDataFixture _orderData;
        private readonly Mock<IGenericReadRepository<Carrier>> _mockCarrierRepo;
        private readonly Mock<IGenericReadRepository<GasStation>> _mockGasStationRepo;

        public BasicOrderRuleTests(OrderDataFixture orderData)
        {
            var mockRepo = new MockRepoHelper(orderData);
            _orderData = orderData;
            _mockCarrierRepo = mockRepo.MockCarriersRepo;
            _mockGasStationRepo = mockRepo.MockGasStationsRepo;
        }

        [Fact]
        public async Task ShouldAllowOrdersWhenAllRulesPass()
        {
            var carrierId = _orderData.Carriers.FirstOrDefault().Id;
            var gasStationId = _orderData.GasStations.LastOrDefault().Id;
            var basicOrderRule = new BasicOrderRule(_mockGasStationRepo.Object,
                _mockCarrierRepo.Object);

            var result = await basicOrderRule.ValidateOrders(_orderData.InputOrders
                .Where(x => x.CarrierId == carrierId && x.GasStationId != gasStationId), 2);

            Assert.True(result);
        }


        [Fact]
        public async Task ShouldNotAllowOrdersIfDispatcherGroupIsDifferent()
        {
            var basicOrderRule = new BasicOrderRule(_mockGasStationRepo.Object,
                _mockCarrierRepo.Object);

            var result = await basicOrderRule.ValidateOrders(_orderData.InputOrders, 2);

            Assert.False(result);
        }

        [Fact]
        public async Task ShouldNotAllowOrdersIfCarrierIsDifferent()
        {
            var carrierId1 = _orderData.Carriers.FirstOrDefault().Id;
            var carrierId2 = _orderData.Carriers.Skip(1).FirstOrDefault().Id;
            var basicOrderRule = new BasicOrderRule(_mockGasStationRepo.Object,
                _mockCarrierRepo.Object);

            var result = await basicOrderRule.ValidateOrders(_orderData.InputOrders
                .Where(x => x.CarrierId == carrierId1 || x.CarrierId == carrierId2), 2);

            Assert.False(result);
        }

        [Fact]
        public async Task ShouldNotAllowOrdersIfCarrierMaxGallonsExceeds()
        {
            var carrierId = _orderData.Carriers.LastOrDefault().Id;
            var basicOrderRule = new BasicOrderRule(_mockGasStationRepo.Object,
                _mockCarrierRepo.Object);

            var result = await basicOrderRule.ValidateOrders(_orderData.InputOrders
                .Where(x => x.CarrierId == carrierId), 2);

            Assert.False(result);
        }

        [Fact]
        public async Task ShouldNotAllowOrdersIfOrderDeliveryTimeIsNotWithInCarrierDeliveryTimeRange()
        {
            var carrierId = _orderData.Carriers.Skip(1).FirstOrDefault().Id;

            var basicOrderRule = new BasicOrderRule(_mockGasStationRepo.Object,
                _mockCarrierRepo.Object);

            var result = await basicOrderRule.ValidateOrders(_orderData.InputOrders
                .Where(x => x.CarrierId == carrierId)
                , 0);

            Assert.False(result);
        }

        [Fact]
        public async Task ShouldAllowOrdersIfOrderDeliveryTimeIsNotWithInCarrierDeliveryTimeRangeAndTreshold()
        {
            var carrierId = _orderData.Carriers.Skip(1).FirstOrDefault().Id;

            var basicOrderRule = new BasicOrderRule(_mockGasStationRepo.Object,
                _mockCarrierRepo.Object);

            var result = await basicOrderRule.ValidateOrders(_orderData.InputOrders
                .Where(x => x.CarrierId == carrierId)
                , 2);

            Assert.True(result);
        }
    }
}
