using SmartBuy.SharedKernel;
using System;

namespace SmartBuy.Administration.Domain
{
    public class GasStationCarrierPreference
    {
        public Guid GasStationId { get; set; }

        public Guid CarrierId { get; set; }

        public bool IsPrimary { get; set; }
    }
}
