using SmartBuy.SharedKernel;
using SmartBuy.SharedKernel.Enums;
using System;

namespace SmartBuy.Administration.Domain
{
    public class OrderStrategy
    {
        public Guid GasStationId { get; set; }

        public OrderType OrderType { get; set; }
    }
}
