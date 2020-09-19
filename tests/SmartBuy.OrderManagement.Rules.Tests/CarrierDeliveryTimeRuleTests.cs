using Moq;
using SmartBuy.Common.Utilities.Repository;
using SmartBuy.OrderManagement.Domain;
using SmartBuy.OrderManagement.Rules.Tests.Helper;
using System;
using Xunit;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBuy.OrderManagement.Rules.Tests
{
    public class CarrierDeliveryTimeRuleTests : IClassFixture<OrderDataFixture>
    {
        private readonly OrderDataFixture _orderData;
        private readonly Mock<IGenericReadRepository<Carrier>> _carrierRepo;
        public CarrierDeliveryTimeRuleTests(OrderDataFixture orderData)
        {
            var mockRepo = new MockRepoHelper(orderData);
            _carrierRepo = mockRepo.MockCarriersRepo;
            _orderData = orderData;
        }

        [Fact]
        public void ShouldThrowExceptionWhenNullValuePassed()
        {
            var carrierDeliveryTimeRule = new CarrierDeliveryTimeRule(_carrierRepo.Object);
            Assert.ThrowsAsync<ArgumentException>(
                async () => await carrierDeliveryTimeRule.IsInBetweenDeliveryTime(null, 0)
                );
        }

        [Fact]
        public async Task ShouldAllowWhenAllOrdersFromTimeIsWithInCarrierDeliveryTimeRange()
        {
            var carrierDeliveryTimeRule = new CarrierDeliveryTimeRule(_carrierRepo.Object);
            var carrierId = _orderData.Carriers.FirstOrDefault().Id;
            var gasStationId = _orderData.GasStations.LastOrDefault().Id;

            var result = await carrierDeliveryTimeRule.IsInBetweenDeliveryTime(_orderData.InputOrders
                .Where(x => x.CarrierId == carrierId && x.GasStationId != gasStationId), 0);

            Assert.True(result);
        }

        [Fact]
        public async Task ShouldNotAllowWhenAnyOrdersDeliveryTimeIsNotWithInCarrierDeliveryTimeRange()
        {
            var carrierDeliveryTimeRule = new CarrierDeliveryTimeRule(_carrierRepo.Object);
            var carrierId = _orderData.Carriers.LastOrDefault().Id;
            var gasStationId = _orderData.GasStations.LastOrDefault().Id;

            var result = await carrierDeliveryTimeRule.IsInBetweenDeliveryTime(_orderData.InputOrders
                .Where(x => x.CarrierId == carrierId && x.GasStationId != gasStationId), 0);

            Assert.False(result);
        }

        [Fact]
        public async Task ShouldAllowOrdersWhenFromTimeIsWithInThresholdRange()
        {
            var carrierDeliveryTimeRule = new CarrierDeliveryTimeRule(_carrierRepo.Object);
            var carrierId = _orderData.Carriers.FirstOrDefault().Id;
            var result = await carrierDeliveryTimeRule.IsInBetweenDeliveryTime(_orderData.InputOrders
                .Where(x => x.CarrierId == carrierId), 2);

            Assert.True(result);
        }
    }
}
