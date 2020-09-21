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
        private GasStationDetailDTO _gasStation1DetailEstimate;

        private GasStation _gasStation2;
        private GasStationDetailDTO _gasStation2DetailEstimate;

        private List<TankReading> _tankReadings;

        public EstimateOrderDataFixture()
        {
            var date = DateTime.Now;
            var guid1 = Guid.NewGuid();
            var guid2 = Guid.NewGuid();
            var guid1tanks = new List<Tank> {
                new Tank(1, guid1, 1, new Measurement(10000,TankMeasurement.Gallons,200, 200, 5000), 1000),
                new Tank(10000, guid1, 2, new Measurement(100,TankMeasurement.Gallons,200, 200, 6000), 1500)
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
                new Tank(3, guid2, 1, new Measurement(100,TankMeasurement.Gallons,100, 100, 800), 500),
                new Tank(4, guid2, 2, new Measurement(100,TankMeasurement.Gallons,100, 100, 800), 2000)
            };
            _gasStation2 = new GasStation(guid2, guid2tanks,
                new TimeRange(new TimeSpan(12, 0, 0), new TimeSpan(23, 59, 0))
                , Guid.NewGuid());

            TankDetail CreateTankDetail(Tank tank)
            {
                return new TankDetail
                {
                    Id = tank.Id,
                    Bottom = tank.Measurement.Bottom,
                    Top = tank.Measurement.Top,
                    EstimatedDaySale = tank.EstimatedDaySale,
                    NetQuantity = tank.NetQuantity,
                    Quantity = tank.Measurement.Quantity
                };
            }

            _gasStation1DetailEstimate = new GasStationDetailDTO
            {
                GasStationId = _gasStation1.Id,
                FromTime = new TimeSpan(12, 0, 0),
                ToTime = new TimeSpan(23, 59, 0),
                TankDetails = guid1tanks.Select(x => CreateTankDetail(x)),
                OrderType = OrderType.Estimate
            };

            _gasStation2DetailEstimate = new GasStationDetailDTO
            {
                GasStationId = _gasStation2.Id,
                FromTime = new TimeSpan(12, 0, 0),
                ToTime = new TimeSpan(23, 59, 0),
                TankDetails = guid2tanks.Select(x => CreateTankDetail(x)),
                OrderType = OrderType.Estimate
            };
        }

        public IEnumerable<GasStation> GasStations => new[]
        { _gasStation1, _gasStation2 };

        public IEnumerable<GasStationDetailDTO> GasStationDetailEstimates => new[] { _gasStation1DetailEstimate, _gasStation2DetailEstimate };

        public IEnumerable<TankReading> TankReadings => _tankReadings;
    }
}

