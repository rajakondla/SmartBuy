using SmartBuy.OrderManagement.Domain.Services.EstimateOrderGenerator;
using SmartBuy.OrderManagement.Domain.Tests.Helper;
using SmartBuy.SharedKernel.Enums;
using System;
using System.Linq;
using Xunit;

namespace SmartBuy.OrderManagement.Domain.Tests
{
    public class EstimateOrderTests : IClassFixture<EstimateOrderDataFixture>
    {
        private readonly EstimateOrderDataFixture _orderData;

        public EstimateOrderTests(EstimateOrderDataFixture orderData)
        {
            _orderData = orderData;
        }

        [Fact]
        public void ShouldGenerateOrderWhenTanksHaveEstimatedDaySaleGreaterThanZero()
        {
            var estimateOrder = new EstimateOrder();
            var gasStation = _orderData.GasStations.First();

            var result = estimateOrder.Create(
               (gasStation, OrderType.Estimate), new DateTime(2020, 10, 7)
            );

            var tank1 = gasStation.Tanks.First();
            var tank2 = gasStation.Tanks.Last();

            Assert.True(result.IsSuccess);
            Assert.True(gasStation.Id == result.Entity!.GasStationId);
            Assert.Equal(2, gasStation.Tanks.Count());
            Assert.Equal(new DateTime(2020, 10, 10, 20, 0, 0), result.Entity!.DispatchDate.Start);
            Assert.Equal(tank1.Measurement.NetQuantity - 1167,
                result.Entity!.OrderProducts.First(x => x.TankId == tank1.Id).Quantity);
            Assert.Equal(tank2.Measurement.NetQuantity - 250,
                result.Entity!.OrderProducts.First(x => x.TankId == tank2.Id).Quantity);
        }
    }
}
