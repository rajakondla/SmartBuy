using SmartBuy.SharedKernel;
using SmartBuy.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBuy.OrderManagement.Domain
{
    public class OrderStrategy
    {
        private OrderStrategy()
        {

        }

        public Guid GasStationId { get; private set; }

        public OrderType OrderType { get; private set; }
    }
}
