using System;
using System.Collections.Generic;
using SmartBuy.SharedKernel;
using SmartBuy.SharedKernel.ValueObjects;

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

        public TimeRange DeliveryTime { get; set; }

        public IList<Tank> Tanks { get; private set; }

        public Guid DispatcherGroupId { get; set; }
    }
}
