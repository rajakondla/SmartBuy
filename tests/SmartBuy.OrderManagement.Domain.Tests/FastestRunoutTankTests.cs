using SmartBuy.OrderManagement.Domain.Services;
using SmartBuy.OrderManagement.Domain.Tests.Helper;
using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
namespace SmartBuy.OrderManagement.Domain.Tests
{
    public class FastestRunoutTankTests : IClassFixture<EstimateOrderDataFixture>
    {
        public readonly List<TankDetail> _tankDetails;
        public FastestRunoutTankTests(EstimateOrderDataFixture orderData)
        {
            _tankDetails = orderData.GasStationDetailEstimates.Where(x =>
            x.GasStationId == orderData.GasStations.First().Id).SelectMany(x => x.TankDetails).ToList();
        }

        [Fact]
        public void ShouldReturnFastestRunoutTankReading()
        {
            var tank1Detail = _tankDetails.First();
            var tank2Detail = _tankDetails.Last();
            var tank1Readings = TankRunout.GetRunoutReadingsByHour(tank1Detail, new DateTime(2020, 10, 7, 0, 0, 0));
            var tank2Readings = TankRunout.GetRunoutReadingsByHour(tank2Detail, new DateTime(2020, 10, 7, 0, 0, 0));
            var lowestTankReadings = TankRunout.GetFastestRunoutTankReading(new List<RunoutReading> {
            new RunoutReading{ TankId = tank1Detail.Id, Bottom = tank1Detail.Measurement.Bottom,
            TankReadings = tank1Readings},
            new RunoutReading{ TankId = tank2Detail.Id, Bottom = tank2Detail.Measurement.Bottom,
            TankReadings = tank2Readings},
            });

            Assert.Equal(250, lowestTankReadings.TankReading.Quantity);
            Assert.Equal(new DateTime(2020, 10, 10, 20, 0, 0), lowestTankReadings.TankReading.ReadingTime);
            Assert.Equal(tank2Detail.Id, lowestTankReadings.TankId);
        }

        [Fact]
        public void ShouldReturnAllTankQuantityByReadingTime()
        {
            var tank1Detail = _tankDetails.First();
            var tank2Detail = _tankDetails.Last();
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
