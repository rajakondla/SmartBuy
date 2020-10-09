using SmartBuy.OrderManagement.Domain.Services;
using SmartBuy.OrderManagement.Domain.Tests.Helper;
using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SmartBuy.OrderManagement.Domain.Tests
{
    public class TankRunoutTests : IClassFixture<EstimateOrderDataFixture>
    {
        public readonly List<TankDetail> _tankDetails;
        public TankRunoutTests(EstimateOrderDataFixture orderData)
        {
            _tankDetails = orderData.GasStationDetailEstimates.Where(x=>
            x.GasStationId == orderData.GasStations.First().Id).SelectMany(x => x.TankDetails).ToList();
        }

        [Fact]
        public void ShouldGetRunoutDateTime()
        {
            var tank1Detail = _tankDetails.First();
            var tank2Detail = _tankDetails.Last();
            var tank1Readings = TankRunout.GetRunoutReadingsByHour(tank1Detail, new DateTime(2020, 10, 7, 0, 0, 0));
            var tank2Readings = TankRunout.GetRunoutReadingsByHour(tank2Detail, new DateTime(2020, 10, 7, 0, 0, 0));
            Assert.Equal(new DateTime(2020, 10, 11, 20, 0, 0), tank1Readings.LastOrDefault().ReadingTime);
            Assert.Equal(new DateTime(2020, 10, 10, 21, 0, 0), tank2Readings.LastOrDefault().ReadingTime);
        }

        [Fact]
        public void ShouldMatchRunTimeAndRunoutTimeWhenTankQtyIsBottomQty()
        {
            var tankDetail = _tankDetails.First();
            tankDetail.Quantity = tankDetail.Bottom;
            var readings = TankRunout.GetRunoutReadingsByHour(tankDetail, new DateTime(2020, 10, 7, 0, 0, 0));
            Assert.Equal(new DateTime(2020, 10, 7, 0, 0, 0), readings.LastOrDefault().ReadingTime);
        }

        [Fact]
        public void ShouldMatchRunTimeAndRunoutTimeWhenTankQtyIsLessThanBottomQty()
        {
            var tankDetail = _tankDetails.First();
            tankDetail.Quantity = tankDetail.Bottom - 50;
            var readings = TankRunout.GetRunoutReadingsByHour(tankDetail, new DateTime(2020, 10, 7, 0, 0, 0));
            Assert.Equal(new DateTime(2020, 10, 7, 0, 0, 0), readings.LastOrDefault().ReadingTime);
        }
    }
}
