using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using System;
using System.Collections.Generic;
using Xunit;
using SmartBuy.SharedKernel.ValueObjects;
using SmartBuy.SharedKernel.Enums;
using System.Linq;
using SmartBuy.OrderManagement.Domain;
using Microsoft.Data.SqlClient;

namespace SmartBuy.OrderManagement.Infrastructure.Tests
{
    [CollectionDefinition("OrderDataCollection")]
    public class OrderDataCollection : ICollectionFixture<OrderDataFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }

    public class OrderDataFixture : IDisposable
    {
        private InputOrder _inputOrder;
        private GasStation _gasStation1;
        private GasStation _gasStation2;

        public OrderDataFixture()
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
        }

        public InputOrder InputOrder => _inputOrder;

        public IEnumerable<GasStation> GasStations => new[]
        { _gasStation1, _gasStation2 };

        public IEnumerable<Order> GetOrders()
        {
            var order1 = Order.Create(
                    new InputOrder
                    {
                        Comments = "Test Order",
                        CarrierId = Guid.NewGuid(),
                        FromTime = new DateTime(2020, 9, 8, 18, 0, 0),
                        ToTime = new DateTime(2020, 9, 9, 5, 0, 0),
                        GasStationId = GasStations.First().Id,
                        LineItems = GasStations.First().Tanks.Select(x => new InputOrderProduct
                        {
                            Quantity = x.Measurement.Quantity,
                            TankId = x.Id
                        }),
                        OrderType = OrderType.Manual
                    },
                    GasStations.First()
                )!.Entity;

            var order2 = Order.Create(
                    new InputOrder
                    {
                        Comments = "Test Order",
                        CarrierId = Guid.NewGuid(),
                        FromTime = new DateTime(2020, 9, 8, 18, 0, 0),
                        ToTime = new DateTime(2020, 9, 9, 5, 0, 0),
                        GasStationId = GasStations.Last().Id,
                        LineItems = GasStations.Last().Tanks.Select(x => new InputOrderProduct
                        {
                            Quantity = x.Measurement.Quantity,
                            TankId = x.Id
                        }),
                        OrderType = OrderType.Manual
                    },
                    GasStations.Last()
                )!.Entity;

            var order3 = Order.Create(
                    new InputOrder
                    {
                        Comments = "Test Order",
                        CarrierId = Guid.NewGuid(),
                        FromTime = new DateTime(2020, 11, 8, 6, 0, 0),
                        ToTime = new DateTime(2020, 11, 8, 9, 0, 0),
                        GasStationId = GasStations.First().Id,
                        LineItems = GasStations.First().Tanks.Select(x => new InputOrderProduct
                        {
                            Quantity = x.Measurement.Quantity,
                            TankId = x.Id
                        }),
                        OrderType = OrderType.Manual
                    },
                    GasStations.First()
                )!.Entity;

            return new List<Order>
            {
               order1!,
               order2!,
               order3!
            };
        }
        public void Dispose()
        {
            SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SmartBuy;Integrated Security=True");
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"delete from OrderManagement.Orders", con);
                cmd.ExecuteNonQuery();
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
    }
}
