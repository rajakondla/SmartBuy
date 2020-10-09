using SmartBuy.OrderManagement.Domain.Services;
using SmartBuy.OrderManagement.Domain.Tests.Helper;
using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
namespace SmartBuy.OrderManagement.Domain.Tests
{
    public class FastestRunoutTankTests
    {
        public readonly List<TankDetail> _tankDetails;
        public FastestRunoutTankTests(EstimateOrderDataFixture orderData)
        {
            _tankDetails = orderData.GasStationDetailEstimates.Where(x =>
            x.GasStationId == orderData.GasStations.First().Id).SelectMany(x => x.TankDetails).ToList();
        }

        [Fact]
        public void ShouldReturnLowestTankReadingRunoutTimesOfAllTanks()
        {
            var tank1Detail = _tankDetails.First();
            var tank2Detail = _tankDetails.Last();
            var tank1Readings = TankRunout.GetRunoutReadingsByHour(tank1Detail, new DateTime(2020, 10, 7, 0, 0, 0));
            var tank2Readings = TankRunout.GetRunoutReadingsByHour(tank2Detail, new DateTime(2020, 10, 7, 0, 0, 0));
            var lowestTankReadings = TankRunout.GetFastRunoutReadings(new List<FastRunoutReading> { 
            new FastRunoutReading{ TankId = tank1Detail.Id, Bottom = tank1Detail.Bottom,
            TankReadings = tank1Readings},
            new FastRunoutReading{ TankId = tank2Detail.Id, Bottom = tank2Detail.Bottom,
            TankReadings = tank2Readings},
            });
            Assert.Equal(1166.64, tank1Readings.LastOrDefault().Quantity);
            Assert.Equal(250, tank2Readings.LastOrDefault().Quantity);
        }
    }
}
