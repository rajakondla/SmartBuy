using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBuy.OrderManagement.Domain.Services
{
    public class DefaultOrder
    {
        private static InputOrder _inputOrder = new InputOrder();
        private static DefaultOrder _instance = new DefaultOrder();

        private DefaultOrder()
        {

        }

        public static DefaultOrder GetInstance => _instance;

        public InputOrder InputOrder => _inputOrder;
    }
}
