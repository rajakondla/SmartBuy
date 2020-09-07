using SmartBuy.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs
{
    public class OrderDetailDTO
    {
        public OrderDetailDTO()
        {
            LineItems = Enumerable.Empty<LineItem>();
        }

        public int OrderId { get; set; }

        public Guid GasStationId { get; set; }

        public DateTime FromDateTime { get; set; }

        public DateTime ToDateTime { get; set; }

        public OrderType OrderType { get; set; }

        public IEnumerable<LineItem> LineItems { get; set; }
    }

    public class LineItem
    {
        public int Quantity { get; set; }

        public int TankId { get; set; }
    }
}
