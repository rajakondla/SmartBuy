using System;
using System.Collections.Generic;
using SmartBuy.SharedKernel;

namespace SmartBuy.Administration.Domain
{
    public class GasStation : Entity<Guid>
    {
        public GasStation(Guid id) : base(id)
        {
            Name = "";
            Address = "";
            Tanks = new List<Tank>();
        }

        private GasStation()
        {
            Name = "";
            Address = "";
            Tanks = new List<Tank>();
        }

        public string Name { get; set; }

        public int Number { get; set; }

        public string Address { get; set; }

        public TimeSpan FromTime { get; set; }

        public TimeSpan ToTime { get; set; }

        public IList<Tank> Tanks { get; private set; }
    }
}
