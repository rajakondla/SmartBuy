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
    public enum TankMeasurement
    {
        Barrels,
        Gallons
    }

    public enum CurrencyUnit
    {
        Dollor,
        Cents
    }
}
