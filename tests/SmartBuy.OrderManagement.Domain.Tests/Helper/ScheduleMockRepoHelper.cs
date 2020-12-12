using Moq;
using SmartBuy.Common.Utilities.Repository;
using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using SmartBuy.OrderManagement.Infrastructure.Abstractions;
using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;
using SmartBuy.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SmartBuy.OrderManagement.Domain.Tests.Helper
{
    public class ScheduleMockRepoHelper
    {
        public ScheduleMockRepoHelper(ScheduleOrderDataFixture orderData)
        {
            MockGasStationScheduleRepo = new Mock<IGenericReadRepository<GasStationSchedule>>();

            MockGasStationScheduleByDayRepo = new Mock<IGenericReadRepository<GasStationScheduleByDay>>();

            MockGasStationTanksScheduleRepo = new Mock<IGenericReadRepository<GasStationTankSchedule>>();

            MockDayComparable = new Mock<IDayComparable>();

            MockTimeIntervalComparable = new Mock<ITimeIntervalComparable>();

            MockGasStationScheduleByTimeRepo = new Mock<IGenericReadRepository<GasStationScheduleByTime>>();

            MockGasStationScheduleRepo.Setup(x => x.FindByAsync(It.IsAny<Expression<Func<GasStationSchedule, bool>>>())).ReturnsAsync(
                (Expression<Func<GasStationSchedule, bool>> predicate) => orderData.GasStationSchedules.Where(predicate.Compile()));

            MockGasStationScheduleByDayRepo.Setup(x => x.FindByAsync(It.IsAny<Expression<Func<GasStationScheduleByDay, bool>>>())).ReturnsAsync((Expression<Func<GasStationScheduleByDay, bool>> predicate) =>
                orderData.GasStationSchedulesByDay.Where(predicate.Compile())
             );

            MockGasStationTanksScheduleRepo.Setup(x => x.FindByAsync(It.IsAny<Expression<Func<GasStationTankSchedule, bool>>>())).ReturnsAsync((Expression<Func<GasStationTankSchedule, bool>> predicate) =>
                orderData.GasStationTankSchedules.Where(predicate.Compile())
             );

            MockGasStationScheduleByTimeRepo.Setup(x => x.FindByAsync(It.IsAny<Expression<Func<GasStationScheduleByTime, bool>>>())).ReturnsAsync((Expression<Func<GasStationScheduleByTime, bool>> predicate) =>
                orderData.GasStationSchedulesByTime.Where(predicate.Compile())
             );

            MockOrderRepository = new Mock<IManageOrderRepository>();
            MockOrderRepository.Setup(x => x.GetOrdersByGasStationIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<OrderType>())).ReturnsAsync((Guid gasStationId, OrderType orderType) =>
                {
                    return new ManageOrder(orderData.GetOrders());
                });

            MockOrderRepository.Setup(x => x.GetOrdersByGasStationIdsAsync(
                It.IsAny<IEnumerable<Guid>>())).ReturnsAsync((IEnumerable<Guid> gasStationIds) =>
                {
                    return new ManageOrder(orderData.GetOrders());
                });
        }

        public Mock<IGenericReadRepository<GasStationSchedule>> MockGasStationScheduleRepo { get; }

        public Mock<IGenericReadRepository<GasStationScheduleByDay>> MockGasStationScheduleByDayRepo { get; }

        public Mock<IGenericReadRepository<GasStationTankSchedule>> MockGasStationTanksScheduleRepo { get; }

        public Mock<IGenericReadRepository<GasStationScheduleByTime>> MockGasStationScheduleByTimeRepo { get; }

        public Mock<IDayComparable> MockDayComparable { get; }

        public Mock<ITimeIntervalComparable> MockTimeIntervalComparable { get; }

        public Mock<IManageOrderRepository> MockOrderRepository { get; }
    }
}
