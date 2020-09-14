using Microsoft.EntityFrameworkCore;
using SmartBuy.Administration.Domain;
using SmartBuy.SharedKernel.ValueObjects;
using System;
using System.Linq;
using Xunit;
using SmartBuy.SharedKernel.Enums;

namespace SmartBuy.Administration.Infrastructure.Tests
{
    public class InMemoryTests
    {
        private DbContextOptionsBuilder<AdministrationContext> _builder;

        public InMemoryTests()
        {
            _builder = new DbContextOptionsBuilder<AdministrationContext>();
        }

        [Fact]
        public void CanInsertGasStationIntoDatabase()
        {
            _builder.UseInMemoryDatabase("CanInsertGasStation");

            using (var context = new AdministrationContext(_builder.Options))
            {
                var gasStation = new GasStation(Guid.NewGuid());
                context.GasStations.Add(gasStation);

                Assert.NotEqual(default(Guid), gasStation.Id);
                Assert.Equal(EntityState.Added, context.Entry(gasStation).State);
            }
        }

        [Fact]
        public void CanInsertTanksAndGetTanksFromDatabase()
        {
            _builder.UseInMemoryDatabase("CanInsertTanksAndGetTanks");

            using (var context = new AdministrationContext(_builder.Options))
            {
                var guid = Guid.NewGuid();
                var gasStation = new GasStation(guid);
                var tank = new Tank();
                tank.GasStationId = guid;
                tank.Number = 1;
                tank.ProductId = 1;
                tank.Name = "Tank1";
                tank.AddMeasurement(new Measurement(100, TankMeasurement.Gallons, 100, 100, 800));
                gasStation.Tanks.Add(tank);
                context.GasStations.Add(gasStation);

                Assert.NotEqual(default(Guid), gasStation.Id);
                Assert.NotEqual(default(Guid), tank.GasStationId);
                Assert.Equal(EntityState.Added, context.Entry(tank).State);
                context.SaveChanges();

                var tanks = context.Tanks.Where(t => t.GasStationId == gasStation.Id).ToList();
                Assert.Single(tanks);
            }
        }

        [Fact]
        public void CanInsertProductAndGetProductFromDatabase()
        {
            _builder.UseInMemoryDatabase("CanInsertTanksAndGetTanks");

            using (var context = new AdministrationContext(_builder.Options))
            {
                var product = new Product();
                context.Products.Add(product);

                Assert.NotEqual(default(int), product.Id);
                Assert.Equal(EntityState.Added, context.Entry(product).State);
                context.SaveChanges();

                product = context.Products.Where(p => p.Id == product.Id).FirstOrDefault();
                Assert.NotNull(product);
            }
        }

        [Fact]
        public void CanInsertGasStationTanksIntoDatabase()
        {
            _builder.UseInMemoryDatabase("CanInsertGasStationTanks");
            var guid = Guid.NewGuid();
            var gasStation = new GasStation(guid);
            using (var context1 = new AdministrationContext(_builder.Options))
            {
                var tank1 = new SmartBuy.Administration.Domain.Tank();
                tank1.GasStationId = guid;
                tank1.Number = 1;
                tank1.ProductId = 1;
                tank1.Name = "Tank1";
                tank1.AddMeasurement(new Measurement(100, TankMeasurement.Gallons, 100, 100, 800));
                var tank2 = new SmartBuy.Administration.Domain.Tank();
                tank2.GasStationId = guid;
                tank2.Number = 1;
                tank2.ProductId = 1;
                tank2.Name = "Tank1";
                tank2.AddMeasurement(new Measurement(100, TankMeasurement.Gallons, 100, 100, 800));
                gasStation.Tanks.Add(tank1);
                gasStation.Tanks.Add(tank2);
                context1.GasStations.Add(gasStation);
                Assert.NotEqual(default(Guid), gasStation.Id);
                Assert.NotEqual(default(Guid), tank1.GasStationId);
                Assert.NotEqual(default(Guid), tank2.GasStationId);
                context1.SaveChanges();
            }

            using (var context2 = new AdministrationContext(_builder.Options))
            {
                var gasStationQuery = context2.GasStations.Include(g => g.Tanks).Where(g => g.Id == gasStation.Id);

                Assert.NotNull(gasStationQuery.FirstOrDefault());
                Assert.Equal(2, gasStationQuery.FirstOrDefault().Tanks.Count());
            }
        }
    }
}
