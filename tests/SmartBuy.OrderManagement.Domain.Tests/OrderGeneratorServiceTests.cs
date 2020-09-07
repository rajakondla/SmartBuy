using Xunit;
using SmartBuy.OrderManagement.Domain.Services;
using Moq;
using System;
using SmartBuy.OrderManagement.Infrastructure.Abstractions;
using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartBuy.SharedKernel.Enums;
using SmartBuy.OrderManagement.Domain.Tests.Helper;

namespace SmartBuy.OrderManagement.Domain.Tests
{
    public class OrderGeneratorServiceTests : IClassFixture<OrderDataFixture>
    {
        private readonly OrderGeneratorService _orderGenService;
        private readonly Mock<IGasStationRepository> _moqGasStationRepo;
        private readonly Guid _gasStationId;

        public OrderGeneratorServiceTests(OrderDataFixture orderData)
        {
            _moqGasStationRepo = new Mock<IGasStationRepository>();
            _gasStationId = orderData.GasStationDetailSchedule.GasStationId;
            _moqGasStationRepo.Setup(x => x.GetGasStationDetailsAsync(It.IsAny<Guid>()))
            .Returns(
                Task.FromResult(
                orderData.GasStationDetailSchedule
                ));
            _orderGenService = new OrderGeneratorService(_moqGasStationRepo.Object);
        }

        [Fact]
        public void ShouldThrowErrorWhenPassInvaildGasStationId()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => { await _orderGenService.RunOrderGenAsync(default(Guid)); });
        }

        [Fact]
        public async Task ShouldGetDefaultInputOrderObjectWhenPassValidGasStationId()
        {
            var inputOrder = await _orderGenService.RunOrderGenAsync(Guid.NewGuid());
            Assert.NotNull(inputOrder);
            Assert.Equal(DefaultOrder.GetInstance.InputOrder, inputOrder);
        }

        [Fact]
        public async Task ShouldGetVaildInputOrderObjectWhenPassValidGasStationid()
        {
            var inputOrder = await _orderGenService.RunOrderGenAsync(_gasStationId);

            _moqGasStationRepo.Verify(repo => repo.GetGasStationDetailsAsync(It.IsAny<Guid>())
            , Times.Once);
            Assert.True(inputOrder.GasStationId == _gasStationId);
        }
    }
}
