using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBuy.OrderManagement.Domain.Services.Abstractions
{
    public interface ITimeIntervalComparable
    {
        bool Compare(TimeSpan timeSpan);
    }
}
