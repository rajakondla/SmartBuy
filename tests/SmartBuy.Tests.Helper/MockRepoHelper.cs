using Moq;
using SmartBuy.Common.Utilities.Repository;
using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using SmartBuy.OrderManagement.Infrastructure.Abstractions;
using SmartBuy.SharedKernel.Enums;
using System;
using System.Linq;
using System.Linq.Expressions;
using SmartBuy.OrderManagement.Domain;
using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;

namespace SmartBuy.Tests.Helper
{
    public class MockRepoHelper
    {
        public MockRepoHelper(IOrderDataFixture orderData)
        {
            MockGasStationScheduleRepo = new Mock<IGenericReadRepository<GasStationSchedule>>();

            MockGasStationScheduleByDayRepo = new Mock<IGenericReadRepository<GasStationScheduleByDay>>();

            MockGasStationTanksScheduleRepo = new Mock<IGenericReadRepository<GasStationTankSchedule>>();

            MockDayComparable = new Mock<IDayComparable>();

            MockTimeIntervalComparable = new Mock<ITimeIntervalComparable>();

            MockGasStationScheduleByTimeRepo = new Mock<IGenericReadRepository<GasStationScheduleByTime>>();



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

            MockManagerOrderRepository = new Mock<IManageOrderRepository>();

            MockManagerOrderRepository.Setup(x => x.GetOrdersByGasStationIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<OrderType>())).ReturnsAsync((Guid gasStationId, OrderType orderType) =>
                {
                    var manageOrder = new ManageOrder();
                    foreach (var ord in orderData.GetOrders().Where(x => x.GasStationId == gasStationId
                    && x.OrderType == orderType))
                        manageOrder.Add(ord);
                    return manageOrder;
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

        public Mock<IGenericReadRepository<GasStationSchedule>> MockGasStationScheduleRepo { get; }

        public Mock<IGenericReadRepository<GasStationScheduleByDay>> MockGasStationScheduleByDayRepo { get; }

        public Mock<IGenericReadRepository<GasStationTankSchedule>> MockGasStationTanksScheduleRepo { get; }

        public Mock<IGenericReadRepository<GasStationScheduleByTime>> MockGasStationScheduleByTimeRepo { get; }

        public Mock<IGenericReadRepository<GasStation>> MockGasStationsRepo { get; }

        public Mock<IDayComparable> MockDayComparable { get; }

        public Mock<ITimeIntervalComparable> MockTimeIntervalComparable { get; }

        public Mock<IManageOrderRepository> MockManagerOrderRepository { get; }

        public Mock<IGasStationRepository> MockGasStationCustomRepository { get; }
    }
}
