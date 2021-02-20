using Moq;
using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using SmartBuy.OrderManagement.Infrastructure.Abstractions;
using SmartBuy.SharedKernel.Enums;
using System;
using System.Linq;
using System.Linq.Expressions;
using SmartBuy.OrderManagement.Domain;
using SmartBuy.SharedKernel.ValueObjects;
using System.Threading.Tasks;

namespace SmartBuy.Tests.Helper
{
    public class MockRepoHelper
    {
        public MockRepoHelper(IOrderDataFixture orderData)
        {
            MockGasStationScheduleRepo = new Mock<IReferenceRepository<GasStationSchedule>>();

            MockGasStationScheduleByDayRepo = new Mock<IReferenceRepository<GasStationScheduleByDay>>();

            MockGasStationTanksScheduleRepo = new Mock<IReferenceRepository<GasStationTankSchedule>>();

            MockDayComparable = new Mock<IDayComparable>();

            MockTimeIntervalComparable = new Mock<ITimeIntervalComparable>();

            MockGasStationScheduleByTimeRepo = new Mock<IReferenceRepository<GasStationScheduleByTime>>();

            MockGasStationsRepo = new Mock<IReferenceRepository<GasStation>>();

            MockManagerOrderRepository = new Mock<IManageOrderRepository>();

            MockGasStationCustomRepository = new Mock<IGasStationRepository>();

            MockGasStationScheduleRepo.Setup(x => x.FindByAsync(It.IsAny<Expression<Func<GasStationSchedule, bool>>>())).ReturnsAsync(
                (Expression<Func<GasStationSchedule, bool>> predicate) => orderData.GasStationSchedules.Where(predicate.Compile()));

            MockGasStationScheduleByDayRepo.Setup(x => x.FindByAsync(It.IsAny<Expression<Func<GasStationScheduleByDay, bool>>>()))
                .ReturnsAsync((Expression<Func<GasStationScheduleByDay, bool>> predicate) =>
                orderData.GasStationSchedulesByDay.Where(predicate.Compile())
             );

            MockGasStationTanksScheduleRepo.Setup(x => x.FindByAsync(It.IsAny<Expression<Func<GasStationTankSchedule, bool>>>()))
                .ReturnsAsync((Expression<Func<GasStationTankSchedule, bool>> predicate) =>
                orderData.GasStationTankSchedules.Where(predicate.Compile())
             );

            MockGasStationScheduleByTimeRepo.Setup(x => x.FindByAsync(It.IsAny<Expression<Func<GasStationScheduleByTime, bool>>>()))
                .ReturnsAsync((Expression<Func<GasStationScheduleByTime, bool>> predicate) =>
                orderData.GasStationSchedulesByTime.Where(predicate.Compile())
             );

            MockManagerOrderRepository.Setup(x => x.GetOrdersByGasStationIdAsync(
                It.IsAny<Guid>())).ReturnsAsync((Guid gasStationId) =>
                {
                    var manageOrder = new ManageOrder();
                    foreach (var ord in orderData.Orders.Where(x => x.GasStationId == gasStationId))
                        manageOrder.Add(ord);
                    return manageOrder;
                });

            MockManagerOrderRepository.Setup(x => x.GetOrderByGasStationIdDeliveryDateAsync(
               It.IsAny<Guid>(),
               It.IsAny<DateTimeRange>())).ReturnsAsync((Guid gasStationId, DateTimeRange dispatchDate) =>
               {
                   return orderData.Orders.Where(x => x.GasStationId == gasStationId
                   && x.DispatchDate == dispatchDate).First();

               });

            MockManagerOrderRepository.Setup(x => x.UpsertAsync(
               It.IsAny<ManageOrder>())).Returns((ManageOrder manageOrder) =>
               {
                   void insertOrder(Order order)
                   {
                       order.CreatedDate = DateTime.UtcNow;
                       order.ModifiedDate = DateTime.UtcNow;

                       orderData.AddOrder(order);
                   }

                   void updateOrder(Order order)
                   {
                       order.ModifiedDate = DateTime.UtcNow;
                   }

                   foreach (var order in manageOrder.Orders)
                   {
                       if (order.State == TrackingState.Added)
                           insertOrder(order);
                       else if (order.State == TrackingState.Modified)
                           updateOrder(order);
                   }

                   return Task.CompletedTask;
               });

            MockGasStationCustomRepository
                .Setup(x => x.GetGasStationIncludeTankOrderStrategyAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Guid gasStationId) =>
              (orderData.GasStations.First(), orderData.OrderStrategies.First().OrderType)
           );

            MockGasStationsRepo.Setup(x => x.FindByAsync(It.IsAny<Expression<Func<GasStation, bool>>>()))
                 .ReturnsAsync((Expression<Func<GasStation, bool>> predicate) =>
                 orderData.GasStations.Where(predicate.Compile())
              );

            MockGasStationsRepo.Setup(x => x.FindByKeyAsync(It.IsAny<object>()))
                .ReturnsAsync((object id) =>
                orderData.GasStations.Where(x => x.Id == (Guid)id).First()
             );
        }

        public Mock<IReferenceRepository<GasStationSchedule>> MockGasStationScheduleRepo { get; }

        public Mock<IReferenceRepository<GasStationScheduleByDay>> MockGasStationScheduleByDayRepo { get; }

        public Mock<IReferenceRepository<GasStationTankSchedule>> MockGasStationTanksScheduleRepo { get; }

        public Mock<IReferenceRepository<GasStationScheduleByTime>> MockGasStationScheduleByTimeRepo { get; }

        public Mock<IReferenceRepository<GasStation>> MockGasStationsRepo { get; }

        public Mock<IDayComparable> MockDayComparable { get; }

        public Mock<ITimeIntervalComparable> MockTimeIntervalComparable { get; }

        public Mock<IManageOrderRepository> MockManagerOrderRepository { get; }

        public Mock<IGasStationRepository> MockGasStationCustomRepository { get; }
    }
}
