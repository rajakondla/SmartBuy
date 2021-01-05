using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using System;
using System.Collections.Generic;
using Xunit;
using SmartBuy.SharedKernel.ValueObjects;
using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;
using SmartBuy.SharedKernel.Enums;
using Moq.Language.Flow;
using System.Linq;

namespace SmartBuy.OrderManagement.Domain.Tests.Helper
{
    public class EstimateOrderDataFixture
    {
        private GasStation _gasStation1;

        private GasStation _gasStation2;

        private List<TankReading> _tankReadings;

        public EstimateOrderDataFixture()
        {
            var date = DateTime.Now;
            var guid1 = Guid.NewGuid();
            var guid2 = Guid.NewGuid();

            var guid1tanks = new List<Tank> {
                new Tank(1, guid1, 1, new Measurement(TankMeasurement.Gallons, 200, 200, 5000), 1000),
                new Tank(2, guid1, 2, new Measurement(TankMeasurement.Gallons, 200, 200, 6000), 1500)
            };

            _tankReadings = new List<TankReading> {
                new TankReading(1, 7000, new DateTime(2020, 9, 21, 8, 0, 0)),
                new TankReading(1, 6000, new DateTime(2020, 9, 21, 9, 0, 0)),
                new TankReading(1, 6000, new DateTime(2020, 9, 21, 9, 30, 0)),
                new TankReading(1, 5500, new DateTime(2020, 9, 21, 10, 30, 0)),
                new TankReading(1, 4000, new DateTime(2020, 9, 21, 6, 0, 0)),
                                                           
                new TankReading(2, 6000, new DateTime(2020, 9, 21, 8, 0, 0)),
                new TankReading(2, 5000, new DateTime(2020, 9, 21, 9, 0, 0)),
                new TankReading(2, 4500, new DateTime(2020, 9, 21, 9, 30, 0)),
                new TankReading(2, 4500, new DateTime(2020, 9, 21, 10, 30, 0)),
                new TankReading(2, 3000, new DateTime(2020, 9, 21, 6, 0, 0)),
                                                            
                 new TankReading(3, 7000, new DateTime(2020,9, 21, 8, 0, 0)),
                new TankReading(3, 6000, new DateTime(2020, 9, 21, 9, 0, 0)),
                new TankReading(3, 6000, new DateTime(2020, 9, 21, 9, 30, 0)),
                new TankReading(3, 5500, new DateTime(2020, 9, 21, 10, 30, 0)),
                new TankReading(3, 4000, new DateTime(2020, 9, 21, 6, 0, 0)),
                                                            
                new TankReading(4, 6000, new DateTime(2020, 9, 21, 8, 0, 0)),
                new TankReading(4, 5000, new DateTime(2020, 9, 8, 9, 0, 0)),
                new TankReading(4, 4500, new DateTime(2020, 9, 8, 9, 30, 0)),
                new TankReading(4, 4500, new DateTime(2020, 9, 8, 10, 30, 0)),
                new TankReading(4, 3000, new DateTime(2020, 9, 8, 6, 0, 0))
            };

            _gasStation1 = new GasStation(guid1, guid1tanks,
                new TimeRange(new TimeSpan(12, 0, 0), new TimeSpan(23, 59, 0))
                , Guid.NewGuid());

            var guid2tanks = new List<Tank> {
                new Tank(3, guid2, 1, new Measurement(TankMeasurement.Gallons,100, 100, 5000), 1000),
                new Tank(4, guid2, 2, new Measurement(TankMeasurement.Gallons,100, 100, 5500), 2000)
            };

            _gasStation2 = new GasStation(guid2, guid2tanks,
                new TimeRange(new TimeSpan(12, 0, 0), new TimeSpan(23, 59, 0))
                , Guid.NewGuid());
        }

        public IEnumerable<GasStation> GasStations => new[]
        { _gasStation1, _gasStation2 };

        public IEnumerable<TankReading> TankReadings => _tankReadings;
    }
}

