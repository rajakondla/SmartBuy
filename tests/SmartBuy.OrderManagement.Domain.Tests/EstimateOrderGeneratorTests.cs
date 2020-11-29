using Moq;
using SmartBuy.Common.Utilities.Repository;
using SmartBuy.OrderManagement.Domain.Services.EstimateOrderGenerator;
using SmartBuy.OrderManagement.Domain.Tests.Helper;
using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SmartBuy.OrderManagement.Domain.Tests
{
    public class EstimateOrderGeneratorTests : IClassFixture<EstimateOrderDataFixture>
    {
        private readonly EstimateOrderDataFixture _orderData;

        public EstimateOrderGeneratorTests(EstimateOrderDataFixture orderData)
        {
            _orderData = orderData;
        }

        [Fact]
        public void ShouldThrowErrorWhenPassingInvalidGasStation()
        {
            var estimateOrder = new EstimateOrder();

            Assert.Throws<ArgumentNullException>(() =>
            {
                estimateOrder.CreateOrder(null, DateTime.Now);
            });
        }

        [Fact]
        public void ShouldGenerateOrderWhenTanksHaveEstimatedDaySaleGreaterThanZero()
        {
            var estimateOrder = new EstimateOrder();
            var gasStation = _orderData.GasStations.FirstOrDefault();
            var gasStationDetails = _orderData.GasStationDetailEstimates.First(x =>
               x.GasStationId == gasStation.Id);

            var inputOrder = estimateOrder.CreateOrder(
               gasStationDetails, new DateTime(2020, 10, 7)
            );

            var tank1 = gasStationDetails.TankDetails.First();
            var tank2 = gasStationDetails.TankDetails.Last();

            Assert.True(gasStation.Id == inputOrder.GasStationId);
            Assert.Equal(2, gasStationDetails.TankDetails.Count());
            Assert.Equal(new DateTime(2020, 10, 10, 20, 0, 0), inputOrder.FromTime);
            Assert.Equal(tank1.Measurement.NetQuantity - 1167,
                inputOrder.LineItems.First(x => x.TankId == tank1.Id).Quantity);
            Assert.Equal(tank2.Measurement.NetQuantity - 250,
                inputOrder.LineItems.First(x => x.TankId == tank2.Id).Quantity);
        }
    }
}
