using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBuy.Administration.Domain
{
    public class GasStationScheduleByDay
    { 
        public Guid GasStationId { get; set; }

        public DayOfWeek DayOfWeek { get; set; }
    }
}
