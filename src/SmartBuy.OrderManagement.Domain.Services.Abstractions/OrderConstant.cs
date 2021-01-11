using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBuy.OrderManagement.Domain.Services.Abstractions
{
    public class OrderConstant
    {
        public const string successMessage = "Order created successfully!";

        public const string duplicateOrderMessage = "Order already exists with same dispatch date";
    }
}
