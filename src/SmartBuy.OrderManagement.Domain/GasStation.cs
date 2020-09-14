using SmartBuy.SharedKernel;
using SmartBuy.SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;

namespace SmartBuy.OrderManagement.Domain
{
    public class GasStation : Entity<Guid>
    {
        public GasStation(Guid id, IEnumerable<Tank> tanks,
            TimeRange deliveryTime, Guid dispatcherGroupId) : base(id)
        {
            Name = "";
            Tanks = new List<Tank>();
            foreach (var tank in tanks)
                Tanks.Add(tank);
            DeliveryTime = deliveryTime;
            DispatcherGroupId = dispatcherGroupId;
        }

        private GasStation()
        {
            Name = "";
            Tanks = new List<Tank>();
            DeliveryTime = new TimeRange(default(TimeSpan), default(TimeSpan));
        }

        public string Name { get; private set; }

        public TimeRange DeliveryTime { get; private set; }

        public ICollection<Tank> Tanks { get; private set; }

        public Guid DispatcherGroupId { get; private set; }
    }
}
