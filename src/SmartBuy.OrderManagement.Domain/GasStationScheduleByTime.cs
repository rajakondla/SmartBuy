using System;

namespace SmartBuy.OrderManagement.Domain
{
    public class GasStationScheduleByTime
    {
        private GasStationScheduleByTime()
        {

        }

        public GasStationScheduleByTime(Guid gasStationId, TimeSpan timeInterval)
        {
            GasStationId = gasStationId;
            TimeInteral = timeInterval;
        }

        public Guid GasStationId { get; private set; }

        public TimeSpan TimeInteral { get; private set; }
    }
}
