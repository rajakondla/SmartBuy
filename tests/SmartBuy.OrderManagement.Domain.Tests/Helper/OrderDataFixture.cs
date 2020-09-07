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
    public class OrderDataCollection : ICollectionFixture<OrderDataFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }

    public class OrderDataFixture
    {
        private InputOrder _inputOrder;
        private GasStation _gasStation;
        private GasStationDetailDTO _gasStationDetailSchedule;
        private GasStationSchedule _gasStationSchedule;
        private GasStationTankSchedule _gasStationTank1Schedule;
        private GasStationTankSchedule _gasStationTank2Schedule;
        private List<GasStationScheduleByDay> _gasStationSchedulesByDay;
        private List<GasStationScheduleByTime> _gasStationScheduleByTime;

        public OrderDataFixture()
        {
            var _lineItems = new List<InputOrderProduct>();
            var date = DateTime.Now;
            var guid = Guid.NewGuid();
            var tanks = new List<Tank> {
                new Tank(1, guid, 1, new Measurement(100,TankMeasurement.Gallons,100, 100, 800)),
                new Tank(2, guid, 2, new Measurement(100,TankMeasurement.Gallons,100, 100, 800))
            };
            _gasStation = new GasStation(guid, tanks, new TimeSpan(12, 0, 0), new TimeSpan(23, 59, 0));
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

            _gasStationDetailSchedule = new GasStationDetailDTO
            {
                GasStationId = _gasStation.Id,
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

            _gasStationSchedule = new GasStationSchedule(_gasStation.Id,
                ScheduleType.ByDay);

            _gasStationTank1Schedule = new GasStationTankSchedule(1, 500);

            _gasStationTank2Schedule = new GasStationTankSchedule(2, 500);

            _gasStationSchedulesByDay = new List<GasStationScheduleByDay>
            {
                new GasStationScheduleByDay(_gasStation.Id, DayOfWeek.Monday),
                new GasStationScheduleByDay(_gasStation.Id, DayOfWeek.Thursday)
            };

            _gasStationScheduleByTime = new List<GasStationScheduleByTime>
            {
                new GasStationScheduleByTime(_gasStation.Id, new TimeSpan(12))
            };
        }

        public InputOrder InputOrder => _inputOrder;

        public GasStation GasStation => _gasStation;

        public GasStationDetailDTO GasStationDetailSchedule => _gasStationDetailSchedule;

        public GasStationSchedule GasStationSchedule => _gasStationSchedule;

        public GasStationTankSchedule GasStationTank1Schedule => _gasStationTank1Schedule;

        public GasStationTankSchedule GasStationTank2Schedule => _gasStationTank2Schedule;

        public IEnumerable<GasStationScheduleByDay> GasStationSchedulesByDay =>
            _gasStationSchedulesByDay;

        public IEnumerable<GasStationScheduleByTime> GasStationScheduleByTime =>
            _gasStationScheduleByTime;
    }
}
