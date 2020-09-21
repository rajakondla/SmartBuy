using Xunit;
using SmartBuy.OrderManagement.Domain.Services;
using Moq;
using System;
using SmartBuy.OrderManagement.Infrastructure.Abstractions;
using System.Threading.Tasks;
using SmartBuy.OrderManagement.Domain.Tests.Helper;
using System.Linq;
using SmartBuy.OrderManagement.Domain.Services.ScheduleOrderGenerator;

namespace SmartBuy.OrderManagement.Domain.Tests
{
    public class OrderGeneratorServiceTests : IClassFixture<ScheduleOrderDataFixture>
    {
        private readonly OrderGeneratorService _orderGenService;
        private readonly Mock<IGasStationRepository> _moqGasStationRepo;
        private readonly Guid _gasStationId;

        public OrderGeneratorServiceTests(ScheduleOrderDataFixture orderData)
        {
            _moqGasStationRepo = new Mock<IGasStationRepository>();
            _gasStationId = orderData.GasStationSchedules.FirstOrDefault().GasStationId;
            _moqGasStationRepo.Setup(x => x.GetGasStationDetailsAsync(It.IsAny<Guid>()))
            .Returns(
                Task.FromResult(
                orderData.GasStationDetailSchedules.FirstOrDefault(x => x.GasStationId == orderData.GasStationSchedules.FirstOrDefault().GasStationId)
                ));
            ScheduleMockRepoHelper mockhelper = new ScheduleMockRepoHelper(orderData);
            _orderGenService = new OrderGeneratorService(_moqGasStationRepo.Object,
                new ScheduleOrder(
                    mockhelper.MockGasStationScheduleRepo.Object,
                    mockhelper.MockGasStationScheduleByDayRepo.Object,
                    mockhelper.MockGasStationTanksScheduleRepo.Object,
                    mockhelper.MockGasStationScheduleByTimeRepo.Object,
                    mockhelper.MockDayComparable.Object,
                    mockhelper.MockTimeIntervalComparable.Object,
                    mockhelper.MockOrderRepository.Object
                    ));
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
