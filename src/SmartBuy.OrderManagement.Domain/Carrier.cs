using SmartBuy.SharedKernel;
using SmartBuy.SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBuy.Administration.Domain
{
    public class Carrier 
    {
        private Carrier()
        {
                
        }

        public Carrier(Guid id, int maxGallons, TimeRange deliveryTime)
        {
            Id = id;
            MaxGallons = maxGallons;
            DeliveryTime = deliveryTime;
        }

        public Guid Id { get; private set; }

        public int MaxGallons { get; private set; }

        public TimeRange DeliveryTime { get; private set; }
    }
}
