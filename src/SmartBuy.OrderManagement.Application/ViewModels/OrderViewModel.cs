using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBuy.OrderManagement.Application.ViewModels
{
    public struct OrderViewModel
    {
        public int OrderId { get; set; }

        public IEnumerable<string> Message { get; set; }

        public bool IsSuccess { get; set; }
    }
}
