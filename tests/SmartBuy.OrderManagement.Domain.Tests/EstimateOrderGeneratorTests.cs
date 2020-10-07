using Moq;
using SmartBuy.Common.Utilities.Repository;
using SmartBuy.OrderManagement.Domain.Services.EstimateOrderGenerator;
using SmartBuy.OrderManagement.Domain.Tests.Helper;
using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SmartBuy.OrderManagement.Domain.Tests
{
    public class EstimateOrderGeneratorTests : IClassFixture<EstimateOrderDataFixture>
    {
        private readonly EstimateOrderDataFixture _orderData;
        private readonly Mock<IGenericReadRepository<GasStation>> _mockGasStationsRepo;

        public EstimateOrderGeneratorTests(EstimateOrderDataFixture orderData)
        {
            _orderData = orderData;
            var mockHelper = new EstimateMockRepoHelper(_orderData);
            _mockGasStationsRepo = mockHelper.MockGasStationsRepo;
        }

        [Fact]
        public void ShouldThrowErrorWhenPassingInvalidGasStation()
        {
            var estimateOrder = new EstimateOrder(_mockGasStationsRepo.Object);

            Assert.ThrowsAsync<ArgumentNullException>(
                async () =>
                {
                    await estimateOrder.CreateOrderAsync(null);
                }
            );
        }

        [Fact]
        public async Task ShouldGenerateOrderWhenTanksHaveEstimatedDaySaleGreaterThanZero()
        {
            var estimateOrder = new EstimateOrder(_mockGasStationsRepo.Object);
            var gasStation = _orderData.GasStations.FirstOrDefault();

            var inputOrder = await estimateOrder.CreateOrderAsync(_orderData.GasStationDetailEstimates.Where(x =>
            x.GasStationId == gasStation.Id).FirstOrDefault());

            _mockGasStationsRepo.Verify(x =>
            x.FindByKeyAsync(It.IsAny<object>()), Times.Once);
            Assert.True(gasStation.Id == inputOrder.GasStationId);
        }
    }
}
