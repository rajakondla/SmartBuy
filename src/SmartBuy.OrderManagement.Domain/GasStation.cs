using SmartBuy.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartBuy.OrderManagement.Domain
{
    public class GasStation : Entity<Guid>
    {
        public GasStation(Guid id, List<Tank> tanks,
            TimeSpan fromTime, TimeSpan toTime) : base(id)
        {
            Name = "";
            Tanks = tanks;
            FromTime = fromTime;
            ToTime = toTime;
        }

        private GasStation()
        {
            Name = "";
            Tanks = new List<Tank>();
        }

        public string Name { get; private set; }

        public TimeSpan FromTime { get; private set; }

        public TimeSpan ToTime { get; private set; }

        public ICollection<Tank> Tanks { get; private set; }
    }
}
