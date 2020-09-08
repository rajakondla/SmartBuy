using Xunit;
using System;
using SmartBuy.Administration.Domain;

namespace SmartBuy.Administration.Domain.Tests
{    
    public class GasStationTests
    {
        private GasStation _gasStation;
        private Guid _id;
        private string _gasStationName;
        private string _address;
        public GasStationTests()
        {
            _gasStationName = "G1";
            _address = "111";
            _id = Guid.NewGuid();
            _gasStation = new GasStation(_id);
            _gasStation.Name = _gasStationName;
            _gasStation.Address = _address;
        }

        [Fact]
        public void ShouldNotReturnNullTanks()
        {
            var tanks = _gasStation.Tanks;

            Assert.NotNull(tanks);
        }

        [Fact]
        public void ShouldMatchAssignedIdAndGasStationId()
        {
            Assert.Equal(_id, _gasStation.Id);
        }

        [Fact]
        public void ShouldMatchGasStationInformationWithAssignedValues()
        {
            Assert.Equal(_gasStationName, _gasStation.Name);
            Assert.Equal(_address, _gasStation.Address);
        }
    }
}