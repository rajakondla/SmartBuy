using SmartBuy.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBuy.SharedDatabase
{
    public class OrderProduct
    {
        public OrderProduct(Order order, Tank tank)
        {
            Order = order;
            OrderId = order.Id;
            Tank = tank;
            TankId = tank.Id;
        }

        public OrderProduct()
        {

        }

        public int Id { get; set; }

        public int OrderId { get; set; }

        public int TankId { get; set; }

        public Order Order { get; private set; }

        public Tank Tank { get; private set; }
    }
}
