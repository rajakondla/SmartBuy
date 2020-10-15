using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using SmartBuy.Administration.Domain;
using SmartBuy.Administration.Infrastructure;
using SmartBuy.SharedKernel.Enums;
using SmartBuy.SharedKernel.ValueObjects;

namespace SmartBuy.OrderManagement.Infrastructure.Tests.Helper
{
    public class AdministrationDataFixture : IDisposable
    {
        private GasStation _gasStation;
        private List<TankReading> _tankReadings;
        private List<TankSale> _tankSales;
        private OrderStrategy _orderStrategy;
        private List<Tank> _tanks;

        public GasStation GasStation => _gasStation;

        public IEnumerable<TankReading> TankReadings => _tankReadings;

        public IEnumerable<TankSale> TankSales => _tankSales;

        public IEnumerable<Tank> Tanks => _tanks;

        public OrderStrategy OrderStrategy => _orderStrategy;

        public AdministrationDataFixture()
        {
            var _gasStationId = Guid.NewGuid();
            var _dateTime1 = new DateTime(2020, 8, 1, 9, 30, 0);
            var _dateTime2 = new DateTime(2020, 8, 2, 9, 30, 0);
            _gasStation = new GasStation(_gasStationId);
            var tank1 = new Tank();
            var product = new Product();
            _orderStrategy = new OrderStrategy();
            _tanks = new List<Tank>();
            _tankSales = new List<TankSale>();
            tank1.GasStationId = _gasStationId;
            tank1.Number = 1;
            tank1.ProductId = 1;
            tank1.Name = "Tank1";

            tank1.AddMeasurement(new Measurement(TankMeasurement.Gallons, 100, 100, 500));
            var tank2 = new Tank();
            tank2.GasStationId = _gasStationId;
            tank2.Number = 1;
            tank2.Name = "Tank2";
            tank2.AddMeasurement(new Measurement(TankMeasurement.Gallons, 100, 100, 500));
            var tank1Sale1 = new TankSale();
            tank1Sale1.Quantity = 100;
            tank1Sale1.SaleTime = _dateTime1;
            var tank1Sale2 = new TankSale();
            tank1Sale2.Quantity = 100;
            tank1Sale2.SaleTime = _dateTime2;
            var tank2Sale1 = new TankSale();
            tank2Sale1.Quantity = 100;
            tank2Sale1.SaleTime = _dateTime1;
            var tank2Sale2 = new TankSale();
            tank2Sale2.Quantity = 100;
            tank2Sale2.SaleTime = _dateTime2;
            using (var context = new AdministrationContext())
            {
                _gasStation.Tanks.Add(tank1);
                _gasStation.Tanks.Add(tank2);

                if (context.GasStations.Any())
                {
                    context.GasStations.RemoveRange(context.GasStations);
                    context.SaveChanges();
                }
                if (context.Products.Any())
                {
                    context.Products.RemoveRange(context.Products);
                    context.SaveChanges();
                }
                if (context.TankSales.Any())
                {
                    context.TankSales.RemoveRange(context.TankSales);
                    context.SaveChanges();
                }
                context.Products.Add(product);
                context.SaveChanges();
                tank1.ProductId = product.Id;
                tank2.ProductId = product.Id;
                context.GasStations.Add(_gasStation);
                context.SaveChanges();
                _tanks.AddRange(new[] { tank1, tank2 });
                tank1Sale1.TankId = tank1.Id;
                tank1Sale2.TankId = tank1.Id;
                tank2Sale1.TankId = tank2.Id;
                tank2Sale2.TankId = tank2.Id;
                context.TankSales.AddRange(new[] { tank1Sale1, tank1Sale2, tank2Sale1, tank2Sale2 });
                _orderStrategy.GasStationId = _gasStation.Id;
                _orderStrategy.OrderType = OrderType.Schedule;
                context.OrderStrategies.Add(_orderStrategy);
                context.SaveChanges();
                _tankSales.AddRange(new[] { tank1Sale1, tank1Sale2, tank2Sale1, tank2Sale2 });
            }
        }

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
