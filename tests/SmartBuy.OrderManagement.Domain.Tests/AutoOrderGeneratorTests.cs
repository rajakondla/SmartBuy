using Xunit;
using SmartBuy.OrderManagement.Domain.Services;
using Moq;
using System;
using SmartBuy.OrderManagement.Infrastructure.Abstractions;
using System.Threading.Tasks;
using System.Linq;
using SmartBuy.OrderManagement.Domain.Services.ScheduleOrderGenerator;
using SmartBuy.OrderManagement.Domain.Services.EstimateOrderGenerator;
using SmartBuy.Tests.Helper;

namespace SmartBuy.OrderManagement.Domain.Tests
{
    public class AutoOrderGeneratorTests : IClassFixture<OrderDataFixture>
    {
        private readonly AutoOrderGenerator _orderGen;
        private readonly Mock<IGasStationRepository> _moqGasStationRepo;
        private readonly Mock<IManageOrderRepository> _manageOrderRepo;
        private readonly Guid _gasStationId;
        private readonly OrderDataFixture _orderData;

        public AutoOrderGeneratorTests(OrderDataFixture orderData)
        {
            var mock = new MockRepoHelper(orderData);

            _moqGasStationRepo = mock.MockGasStationCustomRepository;
            _manageOrderRepo = mock.MockManagerOrderRepository;

            _orderGen = new AutoOrderGenerator(_moqGasStationRepo.Object,
                new ScheduleOrder(
                    mock.MockGasStationScheduleRepo.Object,
                    mock.MockGasStationScheduleByDayRepo.Object,
                    mock.MockGasStationTanksScheduleRepo.Object,
                    mock.MockGasStationScheduleByTimeRepo.Object,
                    mock.MockDayComparable.Object,
                    mock.MockTimeIntervalComparable.Object,
                    mock.MockManagerOrderRepository.Object
                    ),
                new EstimateOrder(),
               _manageOrderRepo.Object);
            _orderData = orderData;
        }

        [Fact]
        public void ShouldThrowErrorWhenPassInvaildGasStationId()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => { await _orderGen.RunAsync(default(Guid)); });
        }

        [Fact]
        public async Task ShouldGetEmptyListWhenNoGasStationIdFound()
        {
            var inputOrders = await _orderGen.RunAsync(Guid.NewGuid());
            Assert.Empty(inputOrders);
        }

        [Fact]
        public async Task ShouldGetVaildInputOrderObjectWhenPassValidGasStationid()
        {
            var inputOrder = await _orderGen.RunAsync(_gasStationId);

            _moqGasStationRepo.Verify(repo => repo.GetGasStationIncludeTankOrderStrategyAsync(It.IsAny<Guid>())
            , Times.Once);
            Assert.True(inputOrder.First().IsSuccess);
            Assert.True(inputOrder.First().Entity!.GasStationId == _gasStationId);
        }

        [Fact]
        public async Task ShouldCreateOrderAndReturnOrderWithoutConflicting()
        {
            var order = _orderData.Orders.First();

            var newOrder = Order.Create(new Services.Abstractions.InputOrder
            {
                CarrierId = order.CarrierId,
                Comments = order.Comments,
                FromTime = new DateTime(2021, 1, 9, 8, 0, 0),
                ToTime = new DateTime(2021, 1, 9, 11, 0, 0),
                GasStationId = order.GasStationId,
                LineItems = order.OrderProducts.Select(x =>
                new Services.Abstractions.InputOrderProduct
                {
                    Quantity = x.Quantity,
                    TankId = x.TankId
                }).ToList(),
                OrderType = order.OrderType
            }, _orderData.GasStations.First());

            order = await _orderGen.CreateOrder(newOrder.Entity!);
            _manageOrderRepo.Verify(repo => repo.GetOrdersByGasStationIdAsync(It.IsAny<Guid>()),
                Times.Once);

            Assert.False(order.IsConflicting);
        }
    }
}
