using SmartBuy.SharedKernel.Enums;
using SmartBuy.SharedKernel;

namespace SmartBuy.Administration.Domain
{
    public class GasStationTankSchedule : Entity<int>
    {
        public int TankId { get; set; }

        public int Quantity { get; set; }

        public TankMeasurement Unit { get; set; }
    }
}
