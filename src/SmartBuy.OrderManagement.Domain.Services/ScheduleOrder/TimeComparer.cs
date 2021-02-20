using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using System;

namespace SmartBuy.OrderManagement.Domain.Services.ScheduleOrderGenerator
{
    public class TimeComparer : ITimeIntervalComparable
    {
        public bool Compare(TimeSpan interval, DateTime recentDeliveryDate, DateTime targetDate)
        {
            return (targetDate - recentDeliveryDate).TotalHours >= interval.Hours;
        }
    }
}
