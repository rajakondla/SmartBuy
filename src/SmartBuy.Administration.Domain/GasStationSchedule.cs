using SmartBuy.SharedKernel;
using SmartBuy.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBuy.Administration.Domain
{
    public class GasStationSchedule
    {
        public Guid GasStationId { get; set; }

        public ScheduleType ScheduleType { get; set; }
    }
}
