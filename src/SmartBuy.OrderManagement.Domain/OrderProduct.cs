using SmartBuy.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBuy.OrderManagement.Domain
{
    public class OrderProduct : Entity<int>
    {
        private OrderProduct()
        {

        }

        public static OrderProduct Create(int tankId, int quantity)
        {
            return new OrderProduct
            {
                TankId = tankId,
                Quantity = quantity
            };
        }

        public int TankId { get; private set; }

        public int Quantity { get; private set; }

        public int OrderId { get; private set; }
    }
}
