using Moq;
using SmartBuy.Common.Utilities.Repository;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace SmartBuy.OrderManagement.Domain.Tests.Helper
{
    public class EstimateMockRepoHelper
    {
        public EstimateMockRepoHelper(EstimateOrderDataFixture orderData)
        {
            MockGasStationsRepo = new Mock<IGenericReadRepository<GasStation>>();

            MockTankReadingsRepo = new Mock<IGenericReadRepository<TankReading>>();

            MockGasStationsRepo.Setup(x => x.FindByKeyAsync(It.IsAny<object>()))
                .ReturnsAsync(
                (Guid id) =>
                orderData.GasStations.FirstOrDefault(x => x.Id == id));

            MockTankReadingsRepo.Setup(x => x.FindByAsync(It.IsAny<Expression<Func<TankReading, bool>>>())).ReturnsAsync((Expression<Func<TankReading, bool>> predicate) =>
                orderData.TankReadings.Where(predicate.Compile())
             );
        }

        public Mock<IGenericReadRepository<GasStation>> MockGasStationsRepo { get; }

        public Mock<IGenericReadRepository<TankReading>> MockTankReadingsRepo { get; }
    }
}
