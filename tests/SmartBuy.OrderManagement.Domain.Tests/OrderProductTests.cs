using SmartBuy.OrderManagement.Domain;
using System.Collections.Generic;
using Moq;
using System;
using Xunit;

namespace SmartBuy.OrderManagement.Domain.Tests
{
    public class OrderProductTests
    {
        private int _tankId;
        private int _quantity;
        public OrderProductTests()
        {
            _tankId = 1;
            _quantity = 100;
        }

        [Fact]
        public void ShouldReturnOrderProduct()
        {
            var ordProduct = OrderProduct.Create(_tankId, _quantity);

            Assert.NotNull(ordProduct);
            Assert.Equal(_tankId, ordProduct.TankId);
            Assert.Equal(_quantity, ordProduct.Quantity);
        }
    }
}