using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBuy.SharedKernel.Enums
{
    public enum OrderType
    {
        Manual,
        Schedule,
        Estimate,
        Historical
    }

    public enum ScheduleType
    {
        ByDay,
        ByTime
    }
}
