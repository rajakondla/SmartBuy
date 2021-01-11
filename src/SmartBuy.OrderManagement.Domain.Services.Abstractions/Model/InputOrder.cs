using SmartBuy.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace SmartBuy.OrderManagement.Domain.Services.Abstractions
{
    public class InputOrder
    {
        public InputOrder()
        {
            LineItems = Enumerable.Empty<InputOrderProduct>();
        }

        public Guid GasStationId { get; set; }
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
        public string? Comments { get; set; }
        public OrderType OrderType { get; set; }
        public Guid? CarrierId { get; set; }

        public IEnumerable<InputOrderProduct> LineItems { get; set; }
    }

    public class InputOrderProduct
    {
        public int Quantity { get; set; }

        public int TankId { get; set; }

        public IEnumerable<(DateTime, int)>? ForecastStatistics { get; set; }
    }
}
