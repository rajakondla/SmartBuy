using SmartBuy.SharedKernel.Enums;
using SmartBuy.SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;

namespace SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs
{
    public class GasStationDetailDTO
    {
        public GasStationDetailDTO()
        {
            TankDetails = new List<TankDetail>();
        }

        public Guid GasStationId { get; set; }

        public TimeSpan FromTime { get; set; }

        public TimeSpan ToTime { get; set; }

        public OrderType OrderType { get; set; }

        public IEnumerable<TankDetail> TankDetails { get; set; }
    }

    public class TankDetail
    {
        public int Id { get; set; }

        public Measurement Measurement { get; set; }

        public int EstimatedDaySale { get; set; }
    }
}
