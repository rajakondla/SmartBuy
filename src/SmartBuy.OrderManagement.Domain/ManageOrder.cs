using SmartBuy.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartBuy.OrderManagement.Domain
{
    public class ManageOrder
    {
        private List<Order> _orders;

        public ManageOrder()
        {
            _orders = new List<Order>();
        }

        public ManageOrder(IEnumerable<Order> orders)
        {
            _orders = new List<Order>(orders);
        }

        public IEnumerable<Order> Orders => _orders;

        public Order Add(Order order)
        {
            if (ValidateOrders(order))
                order.State = SharedKernel.Enums.TrackingState.Added;
            else
                order.IsConflicting = true;

            _orders.Add(order);
            return order;
        }

        private bool ValidateOrders(Order order)
        {
            return _orders
              .FirstOrDefault(x => x.GasStationId == order.GasStationId
              && x.DispatchDate.Overlaps(order.DispatchDate)) == null;
        }
    }
}

