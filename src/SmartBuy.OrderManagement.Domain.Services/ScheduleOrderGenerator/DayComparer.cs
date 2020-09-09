using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using System;

namespace SmartBuy.OrderManagement.Domain.Services.ScheduleOrderGenerator
{
    public class DayComparer : IDayComparable
    {
        public bool Compare(DayOfWeek targetDay)
        {
            return DateTime.Now.DayOfWeek == targetDay;
        }
    }
}
