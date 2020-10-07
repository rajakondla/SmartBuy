using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using System;
using System.Collections.Generic;
using Xunit;
using SmartBuy.SharedKernel.ValueObjects;
using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;
using SmartBuy.SharedKernel.Enums;

namespace SmartBuy.OrderManagement.Domain.Tests.Helper
{
    [CollectionDefinition("OrderDataCollection")]
    public class OrderDataCollection : ICollectionFixture<ScheduleOrderDataFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }

    public class ScheduleOrderDataFixture
    {
        private InputOrder _inputOrder;
        private GasStation _gasStation1;
        private GasStationDetailDTO _gasStation1DetailSchedule;
        private GasStationSchedule _gasStation1Schedule;
        private GasStationTankSchedule _gasStation1Tank1Schedule;
        private GasStationTankSchedule _gasStation1Tank2Schedule;
        private List<GasStationScheduleByDay> _gasStation1SchedulesByDay;

        private GasStation _gasStation2;
        private GasStationDetailDTO _gasStation2DetailSchedule;
        private GasStationSchedule _gasStation2Schedule;
        private GasStationTankSchedule _gasStation2Tank1Schedule;
        private GasStationTankSchedule _gasStation2Tank2Schedule;
        private List<GasStationScheduleByTime> _gasStation2ScheduleByTime;

        public ScheduleOrderDataFixture()
        {
            var _lineItems = new List<InputOrderProduct>();
            var date = DateTime.Now;
            var guid1 = Guid.NewGuid();
            var guid2 = Guid.NewGuid();
            var guid1tanks = new List<Tank> {
                new Tank(1, guid1, 1, new Measurement(TankMeasurement.Gallons,100, 100, 800), 1000),
                new Tank(2, guid1, 2, new Measurement(TankMeasurement.Gallons,100, 100, 800), 1500)
            };
            _gasStation1 = new GasStation(guid1, guid1tanks,
                new TimeRange(new TimeSpan(12, 0, 0), new TimeSpan(23, 59, 0))
                , Guid.NewGuid());

            var guid2tanks = new List<Tank> {
                new Tank(1, guid2, 1, new Measurement(TankMeasurement.Gallons,100, 100, 800), 500),
                new Tank(2, guid2, 2, new Measurement(TankMeasurement.Gallons,100, 100, 800), 2000)
            };
            _gasStation2 = new GasStation(guid2, guid2tanks,
                new TimeRange(new TimeSpan(12, 0, 0), new TimeSpan(23, 59, 0))
                , Guid.NewGuid());

            _lineItems.Add(new InputOrderProduct
            {
                TankId = 1,
                Quantity = 100
            });
            _inputOrder = new InputOrder
            {
                Comments = "New Order",
                GasStationId = Guid.NewGuid(),
                FromTime = date,
                ToTime = date.AddHours(8),
                LineItems = _lineItems
            };

            _gasStation1DetailSchedule = new GasStationDetailDTO
            {
                GasStationId = _gasStation1.Id,
                FromTime = new TimeSpan(12, 0, 0),
                ToTime = new TimeSpan(23, 59, 0),
                TankDetails = new List<TankDetail>
                     {
                      new TankDetail{ Id=1, NetQuantity = 1000, Quantity =500,
                     Bottom = 100, Top = 100 },
                      new TankDetail{ Id=2, NetQuantity = 1000, Quantity =500,
                     Bottom = 100, Top = 100 }
                     },
                OrderType = OrderType.Schedule
            };

            _gasStation2DetailSchedule = new GasStationDetailDTO
            {
                GasStationId = _gasStation2.Id,
                FromTime = new TimeSpan(12, 0, 0),
                ToTime = new TimeSpan(23, 59, 0),
                TankDetails = new List<TankDetail>
                     {
                      new TankDetail{ Id=3, NetQuantity = 1000, Quantity =500,
                     Bottom = 100, Top = 100 },
                      new TankDetail{ Id=4, NetQuantity = 1000, Quantity =500,
                     Bottom = 100, Top = 100 }
                     },
                OrderType = OrderType.Schedule
            };

            _gasStation1Schedule = new GasStationSchedule(_gasStation1.Id,
                ScheduleType.ByDay);

            _gasStation2Schedule = new GasStationSchedule(_gasStation2.Id,
               ScheduleType.ByTime);

            _gasStation1Tank1Schedule = new GasStationTankSchedule(1, 500);

            _gasStation1Tank2Schedule = new GasStationTankSchedule(2, 500);

            _gasStation2Tank1Schedule = new GasStationTankSchedule(3, 500);

            _gasStation2Tank2Schedule = new GasStationTankSchedule(4, 500);

            _gasStation1SchedulesByDay = new List<GasStationScheduleByDay>
            {
                new GasStationScheduleByDay(_gasStation1.Id, DayOfWeek.Monday),
                new GasStationScheduleByDay(_gasStation1.Id, DayOfWeek.Thursday)
            };

            _gasStation2ScheduleByTime = new List<GasStationScheduleByTime>
            {
                new GasStationScheduleByTime(_gasStation2.Id, new TimeSpan(12))
            };
        }

        public InputOrder InputOrder => _inputOrder;

        public IEnumerable<GasStation> GasStations => new[]
        { _gasStation1, _gasStation2 };

        public IEnumerable<GasStationDetailDTO> GasStationDetailSchedules => new[] { _gasStation1DetailSchedule, _gasStation2DetailSchedule };

        public IEnumerable<GasStationSchedule> GasStationSchedules => new[] { _gasStation1Schedule, _gasStation2Schedule };

        public IEnumerable<GasStationTankSchedule> GasStationTankSchedules => new[] { _gasStation1Tank1Schedule, _gasStation1Tank2Schedule,
        _gasStation2Tank1Schedule, _gasStation2Tank2Schedule};

        public IEnumerable<GasStationScheduleByDay> GasStationSchedulesByDay =>
            _gasStation1SchedulesByDay;

        public IEnumerable<GasStationScheduleByTime> GasStationSchedulesByTime =>
            _gasStation2ScheduleByTime;
    }
}
