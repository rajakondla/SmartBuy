using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBuy.OrderManagement.Domain.Services.Abstractions
{
    public interface IDayComparable
    {
        bool Compare(DayOfWeek targetDay);
    }
}
