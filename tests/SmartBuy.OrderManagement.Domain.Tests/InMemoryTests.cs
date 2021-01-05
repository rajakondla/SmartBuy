using Microsoft.EntityFrameworkCore;
using SmartBuy.OrderManagement.Infrastructure;
using System.Linq;
using Xunit;
using SmartBuy.Tests.Helper;

namespace SmartBuy.OrderManagement.Domain.Tests
{
    public class InMemoryTests : IClassFixture<OrderDataFixture>
    {
        private DbContextOptionsBuilder<OrderContext> _builder;
        OrderDataFixture _orderData;
        public InMemoryTests(OrderDataFixture orderData)
        {
            _builder = new DbContextOptionsBuilder<OrderContext>();
            _orderData = orderData;
        }

        [Fact]
        public void CanInsertOrderIntoDatabase()
        {
            _builder.UseInMemoryDatabase("InsertOrder");

            using (var context = new OrderContext(_builder.Options))
            {
                var order = _orderData.GetOrders().First();
                context.Orders.Add(order);

                Assert.Equal(EntityState.Added, context.Entry(order).State);
            }
        }

        [Fact]
        public void CanInsertOrderProductsIntoDatabase()
        {
            _builder.UseInMemoryDatabase("InsertOrderProducts");

            using (var context = new OrderContext(_builder.Options))
            {
                var order = _orderData.GetOrders().First();
                context.Orders.Add(order);

                Assert.Equal(2, order.OrderProducts.Count);
            }
        }

        [Fact]
        public void CanInsertAndFetchOrderOrderProductsDatabase()
        {
            _builder.UseInMemoryDatabase("InsertAndFetchOrderOrderProducts");
            var order = _orderData.GetOrders().FirstOrDefault();
            using (var context = new OrderContext(_builder.Options))
            {
                context.Orders.Add(order);

                Assert.Equal(2, order.OrderProducts.Count);
                context.SaveChanges();
            }

            using (var context1 = new OrderContext(_builder.Options))
            {
                var orders = context1.Orders.Where(o => o.Id == order.Id).First();
                var orderProducts = context1.OrderProducts.Where(o => o.OrderId == order.Id).ToList();

                Assert.NotNull(orders);
                Assert.Equal(2, orderProducts.Count);
            }
        }

        [Fact]
        public void CanCreateOrderWithFromTimeAndToTime()
        {
            _builder.UseInMemoryDatabase("InsertOrderWithFromTimeToTime");

            using (var context = new OrderContext(_builder.Options))
            {
                var order = _orderData.GetOrders().First();
                context.Orders.Add(order);

                Assert.Equal(EntityState.Added, context.Entry(order).State);
                Assert.Equal(_orderData.InputOrder.FromTime, order.DispatchDate.Start);
                Assert.Equal(_orderData.InputOrder.ToTime, order.DispatchDate.End);
            }
        }

        //[Fact]
        //public void ShouldFetchDataFromReferenceContext()
        //{
        //    _builder.UseInMemoryDatabase("CanFetchDataFromReference");
        //    InsertGasStationTanksData(_builder);
        //    var referenceContext = new ReferenceContext(_builder.Options);
        //    var gasStation = referenceContext.GasStations.Where(g => g.Id == _gasStationId).FirstOrDefault();

        //    Assert.NotNull(gasStation);
        //}

        //private void InsertGasStationTanksData(DbContextOptionsBuilder builder)
        //{
        //    using (var context = new AdministrationContext(builder.Options))
        //    {
        //        var gasStation = new SmartBuy.Administration.Domain.GasStation();
        //        var tank1 = new SmartBuy.Administration.Domain.Tank();
        //        var tank2 = new SmartBuy.Administration.Domain.Tank();
        //        gasStation.Tanks.Add(tank1);
        //        gasStation.Tanks.Add(tank2);
        //        context.GasStations.Add(gasStation);
        //        context.SaveChanges();
        //        _gasStationId = gasStation.Id;
        //        _tankId1 = tank1.Id;
        //        _tankId2 = tank2.Id;
        //    }
        //}
    }
}