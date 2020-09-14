using Moq;
using System;
using System.Linq.Expressions;
using System.Linq;
using SmartBuy.OrderManagement.Domain;
using SmartBuy.Common.Utilities.Repository;

namespace SmartBuy.OrderManagement.Rules.Tests.Helper
{
    public class MockRepoHelper
    {
        public readonly Mock<IGenericReadRepository<GasStation>> MockGasStationsRepo;

        public MockRepoHelper(OrderDataFixture orderData)
        {
            MockGasStationsRepo = new Mock<IGenericReadRepository<GasStation>>();
            MockGasStationsRepo.Setup(x =>
            x.FindByAsync(It.IsAny<Expression<Func<GasStation, bool>>>()))
                .ReturnsAsync(
                (Expression<Func<GasStation, bool>> predicate) =>
                orderData.GasStations.Where(predicate.Compile()));

        }
    }
}
