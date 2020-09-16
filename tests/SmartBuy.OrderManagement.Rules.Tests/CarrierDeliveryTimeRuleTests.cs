using Moq;
using SmartBuy.Common.Utilities.Repository;
using SmartBuy.OrderManagement.Domain;
using SmartBuy.OrderManagement.Rules.Tests.Helper;
using SmartBuy.OrderManagement.Rules
using System;
using Xunit;

namespace SmartBuy.OrderManagement.Rules.Tests
{
    public class CarrierDeliveryTimeRuleTests
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
            Assert.Throws<ArgumentException>(() => carrierDeliveryTimeRule
            .IsInBetweenDeliveryTime(null));
        }
    }
}
