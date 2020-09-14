using SmartBuy.SharedKernel;
using SmartBuy.SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBuy.Administration.Domain
{
    public class Carrier : Entity<Guid>
    {
        public string Name { get; set; }

        public int MaxGallons { get; set; }

        public TimeRange DeliveryTime { get; set; }
    }
}
