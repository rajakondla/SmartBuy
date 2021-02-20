using SmartBuy.OrderManagement.Domain.Services;
using SmartBuy.OrderManagement.Domain.Tests.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
namespace SmartBuy.OrderManagement.Domain.Tests
{
    public class FastestRunoutTankTests : IClassFixture<EstimateOrderDataFixture>
    {
        public readonly List<Tank> _tanks;
        public FastestRunoutTankTests(EstimateOrderDataFixture orderData)
        {
            _tanks = orderData.GasStations.First().Tanks.ToList();
        }   

        [Fact]
        public void ShouldReturnFastestRunoutTankReading()
        {
            var tank1 = _tanks.First();
            var tank2 = _tanks.Last();
            var tank1Readings = TankRunout.GetRunoutReadingsByHour(tank1, new DateTime(2020, 10, 7, 0, 0, 0));
            var tank2Readings = TankRunout.GetRunoutReadingsByHour(tank2, new DateTime(2020, 10, 7, 0, 0, 0));
            var lowestTankReadings = TankRunout.GetFastestRunoutTankReading(new List<RunoutReading> {
            new RunoutReading{ TankId = tank1.Id, Bottom = tank1.Measurement.Bottom,
            TankReadings = tank1Readings},
            new RunoutReading{ TankId = tank2.Id, Bottom = tank2.Measurement.Bottom,
            TankReadings = tank2Readings},
            });

            Assert.Equal(250, lowestTankReadings.TankReading.Quantity);
            Assert.Equal(new DateTime(2020, 10, 10, 20, 0, 0), lowestTankReadings.TankReading.ReadingTime);
            Assert.Equal(tank2.Id, lowestTankReadings.TankId);
        }

        [Fact]
        public void ShouldReturnAllTankQuantityByReadingTime()
        {
            var tank1Detail = _tanks.First();
            var tank2Detail = _tanks.Last();
            var tank1Readings = TankRunout.GetRunoutReadingsByHour(tank1Detail, new DateTime(2020, 10, 7, 0, 0, 0));
            var tank2Readings = TankRunout.GetRunoutReadingsByHour(tank2Detail, new DateTime(2020, 10, 7, 0, 0, 0));
            var tankReadings = new List<RunoutReading> {
            new RunoutReading{ TankId = tank1Detail.Id, Bottom = tank1Detail.Measurement.Bottom,
            TankReadings = tank1Readings},
            new RunoutReading{ TankId = tank2Detail.Id, Bottom = tank2Detail.Measurement.Bottom,
            TankReadings = tank2Readings},
            };
            var lowestTankReadings = TankRunout.GetFastestRunoutTankReading(tankReadings);

            var allTankQuantities = TankRunout.GetTanksQuantityByReadingTime(tankReadings
                , lowestTankReadings.TankReading.ReadingTime);
            var tank1 = allTankQuantities.First(x=>x.TankId == tank1Detail.Id);
            var tank2 = allTankQuantities.First(x => x.TankId == tank2Detail.Id);

            Assert.Equal(1166.67, tank1.TankReading.Quantity);
            Assert.Equal(new DateTime(2020, 10, 10, 20, 0, 0), tank1.TankReading.ReadingTime);
            Assert.Equal(250, tank2.TankReading.Quantity);
            Assert.Equal(new DateTime(2020, 10, 10, 20, 0, 0), tank2.TankReading.ReadingTime);
        }
    }
}
