using Moq;
using System;
using System.Linq.Expressions;
using System.Linq;
using SmartBuy.OrderManagement.Domain;
using SmartBuy.Common.Utilities.Repository;
using SmartBuy.Administration.Domain;

namespace SmartBuy.OrderManagement.Rules.Tests.Helper
{
    public class MockRepoHelper
    {
        public readonly Mock<IGenericReadRepository<GasStation>> MockGasStationsRepo;
        public readonly Mock<IGenericReadRepository<Carrier>> MockCarriersRepo;

        public MockRepoHelper(OrderDataFixture orderData)
        {
            MockGasStationsRepo = new Mock<IGenericReadRepository<GasStation>>();
            MockCarriersRepo = new Mock<IGenericReadRepository<Carrier>>();

            MockGasStationsRepo.Setup(x =>
            x.FindByAsync(It.IsAny<Expression<Func<GasStation, bool>>>()))
                .ReturnsAsync(
                (Expression<Func<GasStation, bool>> predicate) =>
                orderData.GasStations.Where(predicate.Compile()));

            MockCarriersRepo.Setup(x =>
            x.FindByKeyAsync(It.IsAny<object>()))
                .ReturnsAsync(
                (Guid id) =>
                orderData.Carriers.FirstOrDefault(x => x.Id == id));
        }
    }
}
