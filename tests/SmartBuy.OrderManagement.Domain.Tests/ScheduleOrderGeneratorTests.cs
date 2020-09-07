using System;
using Xunit;
using SmartBuy.OrderManagement.Domain.Services.ScheduleOrderGenerator;
using SmartBuy.OrderManagement.Domain.Tests.Helper;
using System.Threading.Tasks;
using Moq;
using Repository;
using System.Collections.Generic;
using SmartBuy.SharedKernel.Enums;
using System.Linq;
using System.Linq.Expressions;
using SmartBuy.OrderManagement.Domain.Services.Abstractions;

namespace SmartBuy.OrderManagement.Domain.Tests
{
    public class ScheduleOrderGeneratorTests : IClassFixture<OrderDataFixture>
    {
        private OrderDataFixture _orderData;
        public Mock<IGenericReadRepository<GasStationSchedule>> _mockGasStationScheduleRepo;
        public Mock<IGenericReadRepository<GasStationScheduleByDay>> _mockGasStationScheduleByDayRepo;
        public Mock<IGenericReadRepository<GasStationTankSchedule>> _mockGasStationTanksScheduleRepo;
        public Mock<IGenericReadRepository<GasStationScheduleByTime>> _mockGasStationScheduleByTimeRepo;
        public IEnumerable<GasStationTankSchedule> _gasStationScheduledTanks;
        public IEnumerable<GasStationSchedule> _gasStationSchedule;
        public Mock<IDayComparable> _mockDayComparable;
        public Mock<ITimeIntervalComparable> _mockTimeIntervalComparable;

        public ScheduleOrderGeneratorTests(OrderDataFixture orderData)
        {
            _mockGasStationScheduleRepo = new Mock<IGenericReadRepository<GasStationSchedule>>();
            _gasStationSchedule = new[] { orderData.GasStationSchedule };
            _mockGasStationScheduleByDayRepo = new Mock<IGenericReadRepository<GasStationScheduleByDay>>();
            _mockGasStationTanksScheduleRepo = new Mock<IGenericReadRepository<GasStationTankSchedule>>();
            _mockDayComparable = new Mock<IDayComparable>();
            _mockTimeIntervalComparable = new Mock<ITimeIntervalComparable>();
            _gasStationScheduledTanks = new[] { orderData.GasStationTank1Schedule,
            orderData.GasStationTank2Schedule};
            _mockGasStationScheduleByTimeRepo = new Mock<IGenericReadRepository<GasStationScheduleByTime>>();

            _mockGasStationScheduleRepo.Setup(x => x.FindByAsync(It.IsAny<Expression<Func<GasStationSchedule, bool>>>())).ReturnsAsync(
                _gasStationSchedule);
            _mockGasStationScheduleByDayRepo.Setup(x => x.FindByAsync(It.IsAny<Expression<Func<GasStationScheduleByDay, bool>>>())).ReturnsAsync(
                orderData.GasStationSchedulesByDay
             );
            _mockGasStationTanksScheduleRepo.Setup(x => x.FindByAsync(It.IsAny<Expression<Func<GasStationTankSchedule, bool>>>())).ReturnsAsync(
                _gasStationScheduledTanks
             );
            _mockGasStationScheduleByTimeRepo.Setup(x => x.FindByAsync(It.IsAny<Expression<Func<GasStationScheduleByTime, bool>>>())).ReturnsAsync(
                orderData.GasStationScheduleByTime
             );
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
             );

            Assert.ThrowsAsync<ArgumentException>(
                async () =>
                     {
                         await scheduleOrder.CreateOrderAsync(_orderData.GasStationDetailSchedule);
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
            );
            var result = await scheduleOrder.CreateOrderAsync(_orderData.GasStationDetailSchedule);

            var lineItemsTankIds = result.LineItems.Select(x => x.TankId).ToList();

            _mockDayComparable.Verify(x => x.Compare(It.IsAny<DayOfWeek>()), Times.Once);
            Assert.Equal(OrderType.Schedule, result.OrderType);
            Assert.Equal(2, result.LineItems.Count());
            Assert.True(lineItemsTankIds.Exists(x => x == _gasStationScheduledTanks
                .FirstOrDefault().TankId));
            Assert.True(lineItemsTankIds.Exists(x => x == _gasStationScheduledTanks
                .LastOrDefault().TankId));
            Assert.Equal(result.LineItems.FirstOrDefault(
                x => x.TankId == _gasStationScheduledTanks.FirstOrDefault().TankId)
                .Quantity, _gasStationScheduledTanks.FirstOrDefault().Quantity);
            Assert.Equal(result.LineItems.FirstOrDefault(
                x => x.TankId == _gasStationScheduledTanks.LastOrDefault().TankId)
                .Quantity, _gasStationScheduledTanks.LastOrDefault().Quantity);
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
            );

            await Assert.ThrowsAsync<ScheduleOrder.DayConfugurationException>(async () =>
            {
                await scheduleOrder.CreateOrderAsync(_orderData.GasStationDetailSchedule);
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
            );

            await Assert.ThrowsAsync<ScheduleOrder.TankConfugurationException>(async () =>
            {
                await scheduleOrder.CreateOrderAsync(_orderData.GasStationDetailSchedule);
            });
        }

        [Fact]
        public async Task ShouldReturnScheduleInputOrderObjectWhenScheduleTypeIsTimeInterval()
        {
            _mockTimeIntervalComparable.Setup(x => x.Compare(It.IsAny<TimeSpan>())).Returns(true);
            var scheduleOrder = new ScheduleOrder(
            _mockGasStationScheduleRepo.Object
            , _mockGasStationScheduleByDayRepo.Object
            , _mockGasStationTanksScheduleRepo.Object
            , _mockGasStationScheduleByTimeRepo.Object
            , _mockDayComparable.Object
            , _mockTimeIntervalComparable.Object
            );
            var result = await scheduleOrder.CreateOrderAsync(_orderData.GasStationDetailSchedule);

            var lineItemsTankIds = result.LineItems.Select(x => x.TankId).ToList();

            _mockTimeIntervalComparable.Verify(x => x.Compare(It.IsAny<TimeSpan>()), Times.Once);
            Assert.Equal(OrderType.Schedule, result.OrderType);
            Assert.Equal(2, result.LineItems.Count());
            Assert.True(lineItemsTankIds.Exists(x => x == _gasStationScheduledTanks
                .FirstOrDefault().TankId));
            Assert.True(lineItemsTankIds.Exists(x => x == _gasStationScheduledTanks
                .LastOrDefault().TankId));
            Assert.Equal(result.LineItems.FirstOrDefault(
                x => x.TankId == _gasStationScheduledTanks.FirstOrDefault().TankId)
                .Quantity, _gasStationScheduledTanks.FirstOrDefault().Quantity);
            Assert.Equal(result.LineItems.FirstOrDefault(
                x => x.TankId == _gasStationScheduledTanks.LastOrDefault().TankId)
                .Quantity, _gasStationScheduledTanks.LastOrDefault().Quantity);
        }
    }
}
