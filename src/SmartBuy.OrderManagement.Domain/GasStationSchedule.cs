using SmartBuy.SharedKernel;
using SmartBuy.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

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
