using System;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SmartBuy.OrderManagement.Domain.Services.ScheduleOrderGenerator;
using SmartBuy.OrderManagement.Domain.Tests.Helper;
using SmartBuy.OrderManagement.Infrastructure.Abstractions;
using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;
using SmartBuy.SharedKernel.Enums;
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
