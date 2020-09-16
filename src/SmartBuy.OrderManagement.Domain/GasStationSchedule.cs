using SmartBuy.SharedKernel.Enums;
using System;

namespace SmartBuy.OrderManagement.Domain
{
    public class GasStationSchedule
    {
        private GasStationSchedule()
        {

        }

        public GasStationSchedule(Guid gasStationId, ScheduleType scheduleType)
        {
            GasStationId = gasStationId;
            ScheduleType = scheduleType;
        }

        public Guid GasStationId { get; set; }

        public ScheduleType ScheduleType { get; set; }
    }
}
