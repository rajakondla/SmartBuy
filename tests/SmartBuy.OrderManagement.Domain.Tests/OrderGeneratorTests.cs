using Xunit;
using SmartBuy.OrderManagement.Domain.Services;
using Moq;
using System;
using SmartBuy.OrderManagement.Infrastructure.Abstractions;
using System.Threading.Tasks;
using System.Linq;
using SmartBuy.Tests.Helper;

namespace SmartBuy.OrderManagement.Domain.Tests
{
    public class OrderGeneratorTests : IClassFixture<OrderDataFixture>
    {
        private readonly OrderGenerator _orderGen;
        private readonly Mock<IManageOrderRepository> _manageOrderRepo;
        private readonly OrderDataFixture _orderData;

        public OrderGeneratorTests(OrderDataFixture orderData)
        {
            var mock = new MockRepoHelper(orderData);
            _manageOrderRepo = mock.MockManagerOrderRepository;
            _orderGen = new OrderGenerator(_manageOrderRepo.Object);
            _orderData = orderData;
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

            order = await _orderGen.SaveAsync(newOrder.Entity!);
            _manageOrderRepo.Verify(repo => repo.GetOrdersByGasStationIdAsync(It.IsAny<Guid>()),
                Times.Once);

            Assert.False(order.IsConflicting);
        }
    }
}
