using System;
using System.Linq;
using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using Xunit;
using SmartBuy.Tests.Helper;

namespace SmartBuy.OrderManagement.Domain.Tests
{
    public class ManageOrderTests : IClassFixture<OrderDataFixture>
    {
        private OrderDataFixture _orderData;

        public ManageOrderTests(OrderDataFixture orderData)
        {
            _orderData = orderData;
        }

        [Fact]
        public void ShouldAllowOrderToAddWhenThereIsNoOverlapDateTime()
        {
            var gasStation = _orderData.GasStations.First();
            var inputOrder = new InputOrder
            {
                CarrierId = Guid.NewGuid(),
                Comments = "Manual Order",
                OrderType = SharedKernel.Enums.OrderType.Manual,
                GasStationId = gasStation.Id,
                FromTime = new DateTime(2020, 12, 8, 7, 0, 0),
                ToTime = new DateTime(2020, 12, 8, 10, 0, 0),
                LineItems = gasStation.Tanks.Select(
                    x => new InputOrderProduct
                    {
                        TankId = x.Id,
                        Quantity = x.Measurement.Quantity
                    })
            };
            Order? entity = Order.Create(inputOrder, gasStation)!.Entity;

            var manageOrder = new ManageOrder(_orderData.Orders);
            manageOrder.Add(entity!);

            Assert.False(manageOrder.Add(entity!).IsConflicting);
            Assert.True(manageOrder.Orders.Count(x => x.GasStationId == entity!.GasStationId) == 3);
        }

        [Fact]
        public void ShouldNotAllowOrderToAddWhenThereIsOverlapDateTime()
        {
            var gasStation = _orderData.GasStations.First();
            var inputOrder = new InputOrder
            {
                CarrierId = Guid.NewGuid(),
                Comments = "Manual Order",
                OrderType = SharedKernel.Enums.OrderType.Manual,
                GasStationId = gasStation.Id,
                FromTime = new DateTime(2020, 11, 8, 7, 0, 0),
                ToTime = new DateTime(2020, 11, 8, 10, 0, 0),
                LineItems = gasStation.Tanks.Select(
                    x => new InputOrderProduct
                    {
                        TankId = x.Id,
                        Quantity = x.Measurement.Quantity
                    })
            };
            Order? entity = Order.Create(inputOrder, gasStation)!.Entity;

            var manageOrder = new ManageOrder(_orderData.Orders);

            Assert.True(manageOrder.Add(entity!).IsConflicting);
            Assert.True(manageOrder.Orders.Count(x => x.GasStationId == entity!.GasStationId) == 3);
        }
    }
}
