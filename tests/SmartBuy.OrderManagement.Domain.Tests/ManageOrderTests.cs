using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using SmartBuy.OrderManagement.Domain;
using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using SmartBuy.OrderManagement.Domain.Tests.Helper;
using SmartBuy.OrderManagement.Infrastructure.Abstractions;
using Xunit;

namespace SmartBuy.OrderManagement.Domain.Tests
{
    public class ManageOrderTests : IClassFixture<ScheduleOrderDataFixture>
    {
        private ScheduleOrderDataFixture _orderData;

        public ManageOrderTests(ScheduleOrderDataFixture orderData)
        {
            _orderData = orderData;
        }

        [Fact]
        public void ShouldThrowNullExceptionWhenNotPassingConstructorParameterAsNull()
        {
            var manageOrder = new ManageOrder(null);

            Assert.Throws<NullReferenceException>(() =>
            {
                manageOrder.ValidateOrders(null);
            });
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenOldOrdersParameterNull()
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

            var manageOrder = new ManageOrder(new[] { entity! });

            Assert.Throws<ArgumentNullException>(() =>
            {
                manageOrder.ValidateOrders(null);
            });
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

            var manageOrder = new ManageOrder(new[] { entity! });

            var result = manageOrder.ValidateOrders(_orderData.GetOrders());

            Assert.True(result.Single().IsSuccess);
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

            var manageOrder = new ManageOrder(new[] { entity! });
            var result = manageOrder.ValidateOrders(_orderData.GetOrders());

            Assert.False(result.Single().IsSuccess);
        }
    }
}
