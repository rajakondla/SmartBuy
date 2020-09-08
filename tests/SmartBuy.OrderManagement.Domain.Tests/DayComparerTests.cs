using Moq;
using SmartBuy.OrderManagement.Domain.Services.ScheduleOrderGenerator;
using Xunit;
using System;

namespace SmartBuy.OrderManagement.Domain.Tests
{
    public class DayComparerTests
    {
        [Fact]
        public void ShouldReturnTrueWhenDayIsToday()
        {
            var dayCompare = new DayComparer();

            Assert.True(dayCompare.Compare(DateTime.UtcNow.DayOfWeek));
        }

        [Fact]
        public void ShouldReturnFalseWhenDayIsNotToday()
        {
            var dayCompare = new DayComparer();

            Assert.False(dayCompare.Compare(DateTime.UtcNow.AddDays(-1).DayOfWeek));
        }
    }
}
