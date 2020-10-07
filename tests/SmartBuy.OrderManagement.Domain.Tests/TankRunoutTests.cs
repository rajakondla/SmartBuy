using SmartBuy.OrderManagement.Domain.Services;
using SmartBuy.OrderManagement.Domain.Services.EstimateOrderGenerator;
using SmartBuy.OrderManagement.Domain.Tests.Helper;
using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SmartBuy.OrderManagement.Domain.Tests
{
    public class TankRunoutTests: IClassFixture<EstimateOrderDataFixture>
    {
        public readonly List<TankDetail> _tankDetails;
        public TankRunoutTests(EstimateOrderDataFixture orderData)
        {
            _tankDetails = orderData.GasStationDetailEstimates.SelectMany(x => x.TankDetails).ToList();
        }

        [Fact]
        public void ShouldGetRunoutDateTime()
        {
            var tankDetail = _tankDetails.First();
            var runoutTime = TankRunout.GetRunoutTime(tankDetail, new DateTime(2020, 10, 7, 0, 0, 0));

            Assert.Equal(new DateTime(2020, 10, 15, 19, 0, 0), runoutTime);
        }
    }
}
