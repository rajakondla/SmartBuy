using Moq;
using SmartBuy.OrderManagement.Domain;
using SmartBuy.Common.Utilities.Repository;
using SmartBuy.OrderManagement.Rules.Tests.Helper;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SmartBuy.OrderManagement.Rules.Tests
{
    internal class CarrierDeliveryTimeRuleTests
    {
        private readonly OrderDataFixture _orderData;
        private readonly Mock<IGenericReadRepository<Carrier>> _carrierRepo;

        public CarrierDeliveryTimeRuleTests(OrderDataFixture orderData)
        {
            var mockRepo = new MockRepoHelper(orderData);
            _carrierRepo = mockRepo.MockCarriersRepo;
            _orderData = orderData;
        }

        
    }
}
