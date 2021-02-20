using SmartBuy.OrderManagement.Domain.Services;
using SmartBuy.OrderManagement.Domain.Tests.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SmartBuy.OrderManagement.Domain.Tests
{
    public class TankRunoutTests : IClassFixture<EstimateOrderDataFixture>
    {
        public readonly List<Tank> _tanks;
        public TankRunoutTests(EstimateOrderDataFixture orderData)
        {
            _tanks = orderData.GasStations.First().Tanks.ToList();
        }

        [Fact]
        public void ShouldGetRunoutDateTime()
        {
            var tank1 = _tanks.First();
            var tank2 = _tanks.Last();
            var tank1Readings = TankRunout.GetRunoutReadingsByHour(tank1, new DateTime(2020, 10, 7, 0, 0, 0));
            var tank2Readings = TankRunout.GetRunoutReadingsByHour(tank2, new DateTime(2020, 10, 7, 0, 0, 0));
            Assert.Equal(new DateTime(2020, 10, 11, 20, 0, 0), tank1Readings.LastOrDefault().ReadingTime);
            Assert.Equal(new DateTime(2020, 10, 10, 21, 0, 0), tank2Readings.LastOrDefault().ReadingTime);
        }

        [Fact]
        public void ShouldMatchRunTimeAndRunoutTimeWhenTankQtyIsBottomQty()
        {
            var tank = _tanks.First();
            tank.Measurement.UpdateQuantity(tank.Measurement.Bottom);
            var readings = TankRunout.GetRunoutReadingsByHour(tank, new DateTime(2020, 10, 7, 0, 0, 0));
            Assert.Equal(new DateTime(2020, 10, 7, 0, 0, 0), readings.LastOrDefault().ReadingTime);
        }

        [Fact]
        public void ShouldMatchRunTimeAndRunoutTimeWhenTankQtyIsLessThanBottomQty()
        {
            var tank = _tanks.First();
            tank.Measurement.UpdateQuantity(tank.Measurement.Bottom - 50);
            var readings = TankRunout.GetRunoutReadingsByHour(tank, new DateTime(2020, 10, 7, 0, 0, 0));
            Assert.Equal(new DateTime(2020, 10, 7, 0, 0, 0), readings.LastOrDefault().ReadingTime);
        }
    }
}
