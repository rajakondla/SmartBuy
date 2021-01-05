using SmartBuy.SharedKernel.Enums;
using System;

namespace SmartBuy.OrderManagement.Domain
{
    public class OrderStrategy
    {
        private OrderStrategy()
        {

        }

        public OrderStrategy(Guid gasStationId, OrderType orderType)
        {
            GasStationId = gasStationId;
            OrderType = orderType;
        }

        public Guid GasStationId { get; private set; }

        public OrderType OrderType { get; private set; }
    }
}
