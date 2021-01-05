using System;
using System.Collections.Generic;
using System.Linq;
using SmartBuy.SharedKernel.Enums;

namespace SmartBuy.OrderManagement.Application.InputDTOs
{
    public class OrderInputDTO
    {
        public OrderInputDTO()
        {
            LineItems = Enumerable.Empty<OrderProductInputDTO>();
        }

        public int? OrderId { get; set; }

        public Guid GasStationId { get; set; }

        public DateTime FromDateTime { get; set; }

        public DateTime ToDateTime { get; set; }

        public Guid CarrierId { get; set; }

        public string? Comments { get; set; }

        public OrderType OrderType { get; set; }

        public IEnumerable<OrderProductInputDTO> LineItems { get; set; }

        public struct OrderProductInputDTO
        {
            public int TankId { get; set; }

            public int Quantity { get; set; }

            public TankMeasurement Unit { get; set; }
        }
    }
}
