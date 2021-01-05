using System;
using SmartBuy.OrderManagement.Domain.Services.ScheduleOrderGenerator;
using Xunit;

namespace SmartBuy.OrderManagement.Domain.Tests
{
    public class TimeComparerTests
    {
        [Fact]
        public void ShouldAllowIfDateTimeDiffGreaterThan12Hours()
        {
            TimeComparer timeCompareObj = new TimeComparer();
            var flag = timeCompareObj.Compare(new TimeSpan(12, 0, 0),
                new DateTime(2020, 9, 9, 5, 0, 0), new DateTime(2020, 9, 10, 8, 0, 0));
            Assert.True(flag);
        }

        [Fact]
        public void ShouldNotAllowIfDateTimeDiffLessThan12Hours()
        {
            TimeComparer timeCompareObj = new TimeComparer();
            var flag = timeCompareObj.Compare(new TimeSpan(12, 0, 0),
               new DateTime(2020, 9, 9, 5, 0, 0), new DateTime(2020, 9, 9, 12, 0, 0));
            Assert.False(flag);
        }
    }
}
