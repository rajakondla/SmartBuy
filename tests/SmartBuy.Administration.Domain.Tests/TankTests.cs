using System;
using Moq;
using Xunit;

namespace SmartBuy.Administration.Domain.Tests
{
    public class TankTests
    {
        private Guid _gasStationId;
        private int _productId;
        private string _tankName;

        public TankTests()
        {
            _tankName = "T1";
            _gasStationId = new Guid();
            _productId = 1;
        }

        [Fact]
        public void ShouldNotReturnNullGasStationAndProducts()
        {
            var id = 1;
            var tank = new Tank { GasStationId = _gasStationId, ProductId = _productId };

            Assert.Equal(_gasStationId, tank.GasStationId);
            Assert.Equal(_productId, tank.ProductId);
        }

        [Fact]
        public void ShouldMatchTankInformationWithAssignedValues()
        {
            var id = 1;
            var tank = new Tank { GasStationId = _gasStationId, ProductId = _productId };

            tank.Name = _tankName;

            Assert.Equal(_tankName, tank.Name);
            Assert.Equal(default(Guid), tank.GasStationId);
            Assert.Equal(default(int), tank.ProductId);
        }
    }
}
