using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBuy.SharedKernel.Enums
{
    public enum OrderType
    {
        Manual = 1,
        Schedule = 2,
        Estimate = 3,
        Historical = 4
    }

    public enum ScheduleType
    {
        ByTime = 1,
        ByDay = 2
    }
    public enum TankMeasurement
    {
        Gallons = 1,
        Barrels = 2
    } 

    public enum CurrencyUnit
    {
        Cents = 1,
        Dollor = 2
    }

    public enum TrackingState
    {
        Unchanged = 0,
        Added = 1,
        Modified = 2,
        Deleted = 3
    }
}
