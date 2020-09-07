using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBuy.Administration.Domain
{
    public class GasStationScheduleByTime
    {
        public Guid GasStationId { get; set; }

        public TimeSpan TimeInteral { get; set; }
    }
}
