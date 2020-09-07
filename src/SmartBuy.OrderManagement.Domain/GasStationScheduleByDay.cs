using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBuy.OrderManagement.Domain
{
    public class GasStationScheduleByDay
    {
        private GasStationScheduleByDay()
        {

        }

        public GasStationScheduleByDay(Guid gasStationId, DayOfWeek dayOfWeek)
        {
            GasStationId = gasStationId;
            DayOfWeek = dayOfWeek;
        }

        public Guid GasStationId { get; private set; }

        public DayOfWeek DayOfWeek { get; private set; }
    }
}
