using SmartBuy.OrderManagement.Domain;
using System.Collections.Generic;
using Moq;
using System;
using Xunit;
using System.Linq;
using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using SmartBuy.OrderManagement.Domain.Tests.Helper;
using SmartBuy.Tests.Helper;

namespace SmartBuy.OrderManagement.Domain.Tests
{
    //[CollectionDefinition("OrderDataCollection")]
    public class OrderTests : IClassFixture<OrderDataFixture>
    {
        OrderDataFixture _orderData;
        public OrderTests(OrderDataFixture orderData)
        {
            _orderData = orderData;
        }

        [Fact]
        public void ShouldReturnOrder()
        {
            var order = Order.Create(_orderData.InputOrder, _orderData.GasStations.FirstOrDefault());

            Assert.NotNull(order);
            Assert.Equal(_orderData.InputOrder.GasStationId, order.Entity!.GasStationId);
            Assert.Equal(_orderData.InputOrder.Comments, order.Entity.Comments);
        }

        [Fact]
        public void ShouldReturnOrderProductFromOrder()
        {

            var order = Order.Create(_orderData.InputOrder, _orderData.GasStations.FirstOrDefault());

            Assert.NotNull(order.Entity);
            Assert.Equal(_orderData.InputOrder.GasStationId, order.Entity!.GasStationId);
            Assert.Equal(_orderData.InputOrder.Comments, order.Entity!.Comments);
            Assert.Single(order.Entity!.OrderProducts);
            Assert.Equal(_orderData.InputOrder.LineItems.FirstOrDefault().TankId, order.Entity!.OrderProducts.FirstOrDefault().TankId);
        }

        [Fact]
        public void FromTimeToTimeShouldNotBeSame()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _orderData.InputOrder.ToTime = _orderData.InputOrder.FromTime;
                Order.Create(_orderData.InputOrder, _orderData.GasStations.FirstOrDefault());
            });
        }

        [Fact]
        public void ShouldCreateOrderWithFromTimeAndToTime()
        {
            var order = Order.Create(_orderData.InputOrder, _orderData.GasStations.FirstOrDefault());

            Assert.Equal(_orderData.InputOrder.FromTime, order.Entity!.DispatchDate.Start);
            Assert.Equal(_orderData.InputOrder.ToTime, order.Entity!.DispatchDate.End);
        }

        [Fact]
        public void ShouldNotCreateOrderWithoutProducts()
        {
            OrderDataFixture orderData = _orderData;
            orderData.InputOrder.LineItems = new List<InputOrderProduct>();
            var order = Order.Create(_orderData.InputOrder, _orderData.GasStations.FirstOrDefault());

            Assert.False(order.IsSuccess);
            Assert.Contains("No line items for this order", order.ErrorMessages);
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionWhenGasStationIdIsNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => Order.Create(_orderData.InputOrder, null));
        }
    }
}