using SmartBuy.OrderManagement.Domain;
using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using System;
using System.Collections.Generic;
using Xunit;
using SmartBuy.SharedKernel.ValueObjects;
using SmartBuy.SharedKernel;
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
        private GasStation _gasStation;
        private List<TankReading> _tankReadings;
        private List<TankSale> _tankSales;

        public OrderDataFixture()
        {
            var _lineItems = new List<InputOrderProduct>();
            var date = DateTime.Now;
            var guid = Guid.NewGuid();
            var tanks = new List<Tank> {
                new Tank(1, guid, 1, new Measurement(100,TankMeasurement.Gallons,100, 100, 800)),
                new Tank(2, guid, 2, new Measurement(100,TankMeasurement.Gallons,100, 100, 800))
            };
            _tankReadings = new List<TankReading>
            {
                new TankReading(1,500, new DateTime(2020, 8, 6, 5, 30, 0)),
                new TankReading(2,500, new DateTime(2020, 8, 6, 5, 30, 0))
            };
            _tankSales = new List<TankSale>
            {
                new TankSale(1,500, new DateTime(2020, 8, 6, 5, 30, 0)),
                new TankSale(2,500, new DateTime(2020, 8, 6, 5, 30, 0))
            };
            _gasStation = new GasStation(guid, tanks, new TimeSpan(12, 0, 0), new TimeSpan(23, 59, 0));
            _lineItems.Add(new InputOrderProduct
            {
                TankId = 1,
                Quantity = 100
            });
            _lineItems.Add(new InputOrderProduct
            {
                TankId = 2,
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

        public GasStation GasStation => _gasStation;

        public IEnumerable<TankReading> TankReadings => _tankReadings;

        public IEnumerable<TankSale> TankSales => _tankSales;

        public void Dispose()
        {
            SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SmartBuy;Integrated Security=True");
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(@"delete from Administrator.TankSales
                                    delete from Administrator.TankReadings
                                    delete from Administrator.Products
                                    delete from Administrator.OrderStrategies
                                    delete from Administrator.GasStationTankSchedule
                                    delete from Administrator.GasStationSchedules
                                    delete from Administrator.GasStationScheduleByTimes
                                    delete from Administrator.GasStationScheduleByDays
                                    delete from Administrator.Tanks
                                    delete from  Administrator.gasstations
                                    ", con);
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
