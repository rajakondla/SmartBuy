using SmartBuy.SharedKernel;
using SmartBuy.SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBuy.Administration.Domain
{
    public class GasStationTankSchedule : Entity<int>
    {
        public int TankId { get; set; }

        public int Quantity { get; set; }

        public TankMeasurement Unit { get; set; }
    }
}
