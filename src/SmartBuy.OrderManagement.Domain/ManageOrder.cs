using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using SmartBuy.SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;

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

        public void CreateNewOrder(InputOrder order)
        {

        }
    }
}

