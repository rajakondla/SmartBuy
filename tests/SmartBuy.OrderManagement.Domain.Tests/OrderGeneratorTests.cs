using Xunit;
using SmartBuy.OrderManagement.Domain.Services;
using Moq;
using System;
using SmartBuy.OrderManagement.Infrastructure.Abstractions;
using System.Threading.Tasks;
using SmartBuy.OrderManagement.Domain.Tests.Helper;
using System.Linq;
using SmartBuy.OrderManagement.Domain.Services.ScheduleOrderGenerator;
using SmartBuy.OrderManagement.Domain.Services.EstimateOrderGenerator;
using SmartBuy.Tests.Helper;
using SmartBuy.SharedKernel.Enums;

namespace SmartBuy.OrderManagement.Domain.Tests
{
    public class OrderGeneratorTests : IClassFixture<OrderDataFixture>
    {
        private readonly OrderGenerator _orderGen;
        private readonly Mock<IGasStationRepository> _moqGasStationRepo;
        private readonly Guid _gasStationId;
        private readonly OrderDataFixture _orderData;

        public OrderGeneratorTests(OrderDataFixture orderData)
        {
            var mock = new MockRepoHelper(orderData);

            MockRepoHelper mockhelper = new MockRepoHelper(orderData);
            _orderGen = new OrderGenerator(mock.MockGasStationCustomRepository.Object,
                new ScheduleOrder(
                    mockhelper.MockGasStationScheduleRepo.Object,
                    mockhelper.MockGasStationScheduleByDayRepo.Object,
                    mockhelper.MockGasStationTanksScheduleRepo.Object,
                    mockhelper.MockGasStationScheduleByTimeRepo.Object,
                    mockhelper.MockDayComparable.Object,
                    mockhelper.MockTimeIntervalComparable.Object,
                    mockhelper.MockManagerOrderRepository.Object
                    ),
                new EstimateOrder(),
                mock.MockManagerOrderRepository.Object);
            _orderData = orderData;
        }

        [Fact]
        public void ShouldThrowErrorWhenPassInvaildGasStationId()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => { await _orderGen.AutoOrderGenAsync(default(Guid)); });
        }

        [Fact]
        public async Task ShouldGetEmptyListWhenNoGasStationIdFound()
        {
            var inputOrders = await _orderGen.AutoOrderGenAsync(Guid.NewGuid());
            Assert.Empty(inputOrders);
        }

        [Fact]
        public async Task ShouldGetVaildInputOrderObjectWhenPassValidGasStationid()
        {
            var inputOrder = await _orderGen.AutoOrderGenAsync(_gasStationId);

            _moqGasStationRepo.Verify(repo => repo.GetGasStationIncludeTankOrderStrategyAsync(It.IsAny<Guid>())
            , Times.Once);
            Assert.True(inputOrder.First().IsSuccess);
            Assert.True(inputOrder.First().Entity!.GasStationId == _gasStationId);
        }

        [Fact]
        public void ShouldCreateOrderAndReturnOrderId()
        {
            var order = _orderData.GetOrders().First();

          //  var inputOrder = await _orderGen.CreateOrder(order);
        }
    }
}
