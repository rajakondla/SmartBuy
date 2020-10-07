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

            MockGasStationsRepo.Setup(x => x.FindByKeyAsync(It.IsAny<object>()))
                .ReturnsAsync(
                (Guid id) =>
                orderData.GasStations.FirstOrDefault(x => x.Id == id));
        }

        public Mock<IGenericReadRepository<GasStation>> MockGasStationsRepo { get; }
    }
}
