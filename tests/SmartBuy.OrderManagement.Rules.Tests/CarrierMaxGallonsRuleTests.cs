using Moq;
using SmartBuy.Administration.Domain;
using SmartBuy.Common.Utilities.Repository;
using SmartBuy.OrderManagement.Rules.Tests.Helper;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SmartBuy.OrderManagement.Rules.Tests
{
    public class CarrierMaxGallonsRuleTests : IClassFixture<OrderDataFixture>
    {
        private readonly OrderDataFixture _orderData;
        private readonly Mock<IGenericReadRepository<Carrier>> _carrierRepo;
        public CarrierMaxGallonsRuleTests(OrderDataFixture orderData)
        {
            var mockRepo = new MockRepoHelper(orderData);
            _carrierRepo = mockRepo.MockCarriersRepo;
            _orderData = orderData;
        }

        [Fact]
        public async Task ShouldPassOrderIfTotalOrderGallonsIsLessThanCarrierMaxCapacity()
        {
            CarrierMaxGallonsRule carrierMaxGallonsRule = new CarrierMaxGallonsRule(_carrierRepo.Object);

            var result = await carrierMaxGallonsRule.IsOrderGallonsLessThanOrEqualMaxCapacity(_orderData.InputOrders.Take(2));

            Assert.True(result);
        }

        [Fact]
        public async Task ShouldNotPassOrderIfTotalOrderGallonsIsGreaterThanCarrierMaxCapacity()
        {
            CarrierMaxGallonsRule carrierMaxGallonsRule = new CarrierMaxGallonsRule(_carrierRepo.Object);

            var result = await carrierMaxGallonsRule.IsOrderGallonsLessThanOrEqualMaxCapacity(_orderData.InputOrders.Skip(2).Take(2));

            Assert.False(result);
        }
    }
}
