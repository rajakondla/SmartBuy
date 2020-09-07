using SmartBuy.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBuy.Administration.Domain
{
    public class TankSale : Entity<int>
    {
        public int Quantity { get; set; }

        public DateTime SaleTime { get; set; }

        public int TankId { get; set; }
    }
}
