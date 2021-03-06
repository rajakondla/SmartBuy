﻿using System;
using Xunit;
using SmartBuy.OrderManagement.Domain.Services.ScheduleOrderGenerator;
using System.Threading.Tasks;
using Moq;
using SmartBuy.Common.Utilities.Repository;
using SmartBuy.SharedKernel.Enums;
using System.Linq;
using System.Linq.Expressions;
using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using SmartBuy.OrderManagement.Infrastructure.Abstractions;
using SmartBuy.Tests.Helper;

namespace SmartBuy.OrderManagement.Domain.Tests
{
    public class ScheduleOrderTests : IClassFixture<OrderDataFixture>
    {
        private readonly OrderDataFixture _orderData;
        private Mock<IReferenceRepository<GasStationSchedule>> _mockGasStationScheduleRepo;
        private Mock<IReferenceRepository<GasStationScheduleByDay>> _mockGasStationScheduleByDayRepo;
        private Mock<IReferenceRepository<GasStationTankSchedule>> _mockGasStationTanksScheduleRepo;
        private Mock<IReferenceRepository<GasStationScheduleByTime>> _mockGasStationScheduleByTimeRepo;
        private Mock<IDayComparable> _mockDayComparable;
        private Mock<ITimeIntervalComparable> _mockTimeIntervalComparable;
        private Mock<IManageOrderRepository> _mockOrderRepository;

        public ScheduleOrderTests(OrderDataFixture orderData)
        {
            MockRepoHelper mockhelper = new MockRepoHelper(orderData);
            _mockGasStationScheduleRepo = mockhelper.MockGasStationScheduleRepo;
            _mockGasStationScheduleByDayRepo = mockhelper.MockGasStationScheduleByDayRepo;
            _mockGasStationTanksScheduleRepo = mockhelper.MockGasStationTanksScheduleRepo;
            _mockGasStationScheduleByTimeRepo = mockhelper.MockGasStationScheduleByTimeRepo;
            _mockDayComparable = mockhelper.MockDayComparable;
            _mockTimeIntervalComparable = mockhelper.MockTimeIntervalComparable;
            _mockOrderRepository = mockhelper.MockManagerOrderRepository;

            _orderData = orderData;
        }

        [Fact]
        public void ShouldThrowErrorWhenPassingInvalidGasStation()
        {
            var scheduleOrder = new ScheduleOrder(
             _mockGasStationScheduleRepo.Object
             , _mockGasStationScheduleByDayRepo.Object
             , _mockGasStationTanksScheduleRepo.Object
             , _mockGasStationScheduleByTimeRepo.Object
             , _mockDayComparable.Object
             , _mockTimeIntervalComparable.Object
             , _mockOrderRepository.Object
             );

            Assert.ThrowsAsync<ArgumentException>(
                async () =>
                     {
                         await scheduleOrder.CreateAsync(
                             (_orderData.GasStations.First(), _orderData.OrderStrategies.First().OrderType));
                     }
            );
        }

        [Fact]
        public async Task ShouldReturnScheduleInputOrderObjectWhenScheduleTypeIsDay()
        {
            _mockDayComparable.Setup(x => x.Compare(It.IsAny<DayOfWeek>())).Returns(true);
            var scheduleOrder = new ScheduleOrder(
            _mockGasStationScheduleRepo.Object
            , _mockGasStationScheduleByDayRepo.Object
            , _mockGasStationTanksScheduleRepo.Object
            , _mockGasStationScheduleByTimeRepo.Object
            , _mockDayComparable.Object
            , _mockTimeIntervalComparable.Object
            , _mockOrderRepository.Object
            );

            var gasStation = _orderData.GasStations.First();
            var result = await scheduleOrder.CreateAsync((gasStation, _orderData.OrderStrategies.First().OrderType));

            Assert.True(result.IsSuccess);
            var lineItemsTankIds = result.Entity!.OrderProducts.Select(x => x.TankId).ToList();
            var gasStationScheduleTanks = _orderData.GasStationTankSchedules.Where(x => x.TankId == 1 || x.TankId == 2).ToList();

            _mockDayComparable.Verify(x => x.Compare(It.IsAny<DayOfWeek>()), Times.Once);
            Assert.Equal(OrderType.Schedule, result.Entity!.OrderType);
            Assert.Equal(2, result.Entity!.OrderProducts.Count());
            Assert.True(lineItemsTankIds.Exists(x => x == gasStationScheduleTanks
                .FirstOrDefault().TankId));
            Assert.True(lineItemsTankIds.Exists(x => x == gasStationScheduleTanks
                .LastOrDefault().TankId));
            Assert.Equal(result.Entity!.OrderProducts.FirstOrDefault(
                x => x.TankId == gasStationScheduleTanks.FirstOrDefault().TankId)
                .Quantity, gasStationScheduleTanks.FirstOrDefault().Quantity);
            Assert.Equal(result.Entity!.OrderProducts.FirstOrDefault(
                x => x.TankId == gasStationScheduleTanks.LastOrDefault().TankId)
                .Quantity, gasStationScheduleTanks.LastOrDefault().Quantity);
        }

        [Fact]
        public async Task ShouldGenerateExceptionWhenDayConfigurationNotExist()
        {
            _mockDayComparable.Setup(x => x.Compare(It.IsAny<DayOfWeek>())).Returns(true);
            _mockGasStationScheduleByDayRepo.Setup(x => x.FindByAsync(It.IsAny<Expression<Func<GasStationScheduleByDay, bool>>>())).ReturnsAsync(
               Enumerable.Empty<GasStationScheduleByDay>()
              );

            var scheduleOrder = new ScheduleOrder(
            _mockGasStationScheduleRepo.Object
            , _mockGasStationScheduleByDayRepo.Object
            , _mockGasStationTanksScheduleRepo.Object
            , _mockGasStationScheduleByTimeRepo.Object
            , _mockDayComparable.Object
            , _mockTimeIntervalComparable.Object
            , _mockOrderRepository.Object
            );

            await Assert.ThrowsAsync<ScheduleOrder.DayConfugurationException>(async () =>
            {
                await scheduleOrder.CreateAsync((_orderData.GasStations.First(), _orderData.OrderStrategies.First().OrderType));
            });
        }

        [Fact]
        public async Task ShouldGenerateExceptionWhenTankConfigurationNotExist()
        {
            _mockDayComparable.Setup(x => x.Compare(It.IsAny<DayOfWeek>())).Returns(true);
            _mockGasStationTanksScheduleRepo.Setup(x => x.FindByAsync(It.IsAny<Expression<Func<GasStationTankSchedule, bool>>>())).ReturnsAsync(
               Enumerable.Empty<GasStationTankSchedule>()
              );

            var scheduleOrder = new ScheduleOrder(
            _mockGasStationScheduleRepo.Object
            , _mockGasStationScheduleByDayRepo.Object
            , _mockGasStationTanksScheduleRepo.Object
            , _mockGasStationScheduleByTimeRepo.Object
            , _mockDayComparable.Object
            , _mockTimeIntervalComparable.Object
            , _mockOrderRepository.Object
            );

            await Assert.ThrowsAsync<ScheduleOrder.TankConfugurationException>(async () =>
            {
                await scheduleOrder.CreateAsync((_orderData.GasStations.First(), _orderData.OrderStrategies.First().OrderType));
            });
        }

        [Fact]
        public async Task ShouldReturnScheduleInputOrderObjectWhenScheduleTypeIsTimeInterval()
        {
            _mockTimeIntervalComparable.Setup(x => x.Compare(It.IsAny<TimeSpan>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(true);
            var scheduleOrder = new ScheduleOrder(
            _mockGasStationScheduleRepo.Object
            , _mockGasStationScheduleByDayRepo.Object
            , _mockGasStationTanksScheduleRepo.Object
            , _mockGasStationScheduleByTimeRepo.Object
            , _mockDayComparable.Object
            , _mockTimeIntervalComparable.Object
            , _mockOrderRepository.Object
            );
            var result = await scheduleOrder.CreateAsync((_orderData.GasStations.Last(), _orderData.OrderStrategies.Last().OrderType));

            Assert.True(result.IsSuccess);
            var lineItemsTankIds = result.Entity!.OrderProducts.Select(x => x.TankId).ToList();
            var gasStationScheduleTanks = _orderData.GasStationTankSchedules.Where(x => x.TankId == 3 || x.TankId == 4).ToList();

            _mockTimeIntervalComparable.Verify(x => x.Compare(It.IsAny<TimeSpan>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
            Assert.Equal(OrderType.Schedule, result.Entity!.OrderType);
            Assert.Equal(2, result.Entity!.OrderProducts.Count());
            Assert.True(lineItemsTankIds.Exists(x => x == gasStationScheduleTanks
                .FirstOrDefault().TankId));
            Assert.True(lineItemsTankIds.Exists(x => x == gasStationScheduleTanks
                .LastOrDefault().TankId));
            Assert.Equal(result.Entity!.OrderProducts.FirstOrDefault(
                x => x.TankId == gasStationScheduleTanks.FirstOrDefault().TankId)
                .Quantity, gasStationScheduleTanks.FirstOrDefault().Quantity);
            Assert.Equal(result.Entity!.OrderProducts.FirstOrDefault(
                x => x.TankId == gasStationScheduleTanks.LastOrDefault().TankId)
                .Quantity, gasStationScheduleTanks.LastOrDefault().Quantity);
        }

        [Fact]
        public async Task ShouldGenerateExceptionWhenTimeIntervalConfigurationNotExist()
        {
            _mockTimeIntervalComparable.Setup(x => x.Compare(It.IsAny<TimeSpan>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(true);
            _mockGasStationScheduleByTimeRepo.Setup(x => x.FindByAsync(It.IsAny<Expression<Func<GasStationScheduleByTime, bool>>>())).ReturnsAsync(
               Enumerable.Empty<GasStationScheduleByTime>()
              );

            var scheduleOrder = new ScheduleOrder(
            _mockGasStationScheduleRepo.Object
            , _mockGasStationScheduleByDayRepo.Object
            , _mockGasStationTanksScheduleRepo.Object
            , _mockGasStationScheduleByTimeRepo.Object
            , _mockDayComparable.Object
            , _mockTimeIntervalComparable.Object
            , _mockOrderRepository.Object
            );

            await Assert.ThrowsAsync<ScheduleOrder.TimIntervalConfigurationException>(async () =>
            {
                await scheduleOrder.CreateAsync((_orderData.GasStations.Last(), _orderData.OrderStrategies.Last().OrderType));
            });
        }
    }
}
