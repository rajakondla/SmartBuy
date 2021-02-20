using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using System;
using System.Collections.Generic;
using SmartBuy.SharedKernel.ValueObjects;
using SmartBuy.SharedKernel.Enums;
using System.Linq;
using SmartBuy.OrderManagement.Domain;

namespace SmartBuy.Tests.Helper
{
    //[CollectionDefinition("OrderDataCollection")]
    //public class OrderDataCollection : ICollectionFixture<OrderDataFixture>
    //{
    //    // This class has no code, and is never created. Its purpose is simply
    //    // to be the place to apply [CollectionDefinition] and all the
    //    // ICollectionFixture<> interfaces.
    //}

    public class OrderDataFixture : IOrderDataFixture
    {
        private InputOrder _inputOrder;
        private GasStation _gasStation1;
        private GasStationSchedule _gasStation1Schedule;
        private GasStationTankSchedule _gasStation1Tank1Schedule;
        private GasStationTankSchedule _gasStation1Tank2Schedule;
        private List<GasStationScheduleByDay> _gasStation1SchedulesByDay;
        private OrderStrategy _gasStation1Strategy;


        private GasStation _gasStation2;
        private GasStationSchedule _gasStation2Schedule;
        private GasStationTankSchedule _gasStation2Tank1Schedule;
        private GasStationTankSchedule _gasStation2Tank2Schedule;
        private List<GasStationScheduleByTime> _gasStation2ScheduleByTime;
        private OrderStrategy _gasStation2Strategy;

        private List<Carrier> _carriers;
        private List<Order> _orders;

        public OrderDataFixture()
        {
            var _lineItems = new List<InputOrderProduct>();
            var date = DateTime.Now;
            var guid1 = Guid.NewGuid();
            var guid2 = Guid.NewGuid();
            var carrierId = Guid.NewGuid();
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

            _carriers = new List<Carrier>
            {
                new Carrier(carrierId, 5000, new TimeRange(new TimeSpan(6, 0,0), new TimeSpan(18, 0, 0)))
            };

            _gasStation1Strategy = new OrderStrategy(_gasStation1.Id, OrderType.Schedule);

            _gasStation2Strategy = new OrderStrategy(_gasStation2.Id, OrderType.Schedule);

            var order1 = Order.Create(
                    new InputOrder
                    {
                        Comments = "Test Order",
                        CarrierId = Guid.NewGuid(),
                        FromTime = new DateTime(2020, 9, 8, 18, 0, 0),
                        ToTime = new DateTime(2020, 9, 9, 5, 0, 0),
                        GasStationId = GasStations.FirstOrDefault().Id,
                        LineItems = GasStations.FirstOrDefault().Tanks.Select(x => new InputOrderProduct
                        {
                            Quantity = x.Measurement.Quantity,
                            TankId = x.Id
                        }),
                        OrderType = OrderType.Manual
                    },
                    GasStations.FirstOrDefault()
                )!.Entity;

            var order2 = Order.Create(
                    new InputOrder
                    {
                        Comments = "Test Order",
                        CarrierId = Guid.NewGuid(),
                        FromTime = new DateTime(2020, 9, 8, 18, 0, 0),
                        ToTime = new DateTime(2020, 9, 9, 5, 0, 0),
                        GasStationId = GasStations.LastOrDefault().Id,
                        LineItems = GasStations.LastOrDefault().Tanks.Select(x => new InputOrderProduct
                        {
                            Quantity = x.Measurement.Quantity,
                            TankId = x.Id
                        }),
                        OrderType = OrderType.Manual
                    },
                    GasStations.LastOrDefault()
                )!.Entity;

            var order3 = Order.Create(
                    new InputOrder
                    {
                        Comments = "Test Order",
                        CarrierId = Guid.NewGuid(),
                        FromTime = new DateTime(2020, 11, 8, 6, 0, 0),
                        ToTime = new DateTime(2020, 11, 8, 9, 0, 0),
                        GasStationId = GasStations.FirstOrDefault().Id,
                        LineItems = GasStations.FirstOrDefault().Tanks.Select(x => new InputOrderProduct
                        {
                            Quantity = x.Measurement.Quantity,
                            TankId = x.Id
                        }),
                        OrderType = OrderType.Manual
                    },
                    GasStations.FirstOrDefault()
                )!.Entity;

            _orders = new List<Order> {
               order1!,
               order2!,
               order3!
            };
        }

        public InputOrder InputOrder => _inputOrder;

        public IEnumerable<GasStation> GasStations => new[]
        { _gasStation1, _gasStation2 };

        public IEnumerable<GasStationSchedule> GasStationSchedules => new[] { _gasStation1Schedule, _gasStation2Schedule };

        public IEnumerable<GasStationTankSchedule> GasStationTankSchedules => new[] { _gasStation1Tank1Schedule, _gasStation1Tank2Schedule,
        _gasStation2Tank1Schedule, _gasStation2Tank2Schedule};

        public IEnumerable<GasStationScheduleByDay> GasStationSchedulesByDay =>
            _gasStation1SchedulesByDay;

        public IEnumerable<GasStationScheduleByTime> GasStationSchedulesByTime =>
            _gasStation2ScheduleByTime;

        public IEnumerable<Order> Orders => _orders;

        public IEnumerable<Carrier> Carriers => _carriers;

        public IEnumerable<OrderStrategy> OrderStrategies => new[] {
            _gasStation1Strategy,
            _gasStation2Strategy
        };

        public void AddOrder(Order order)
        {
            typeof(Order).GetProperty("Id").SetValue(order, new Random().Next(10000), null);
            _orders.Add(order);
        }
    }
}
