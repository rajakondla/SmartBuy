using System;

namespace SmartBuy.OrderManagement.Domain
{
    public class GasStationCarrierPreference
    {
        private GasStationCarrierPreference()
        {

        }

        public GasStationCarrierPreference(Guid gasStationId, Guid carrierId, bool isPrimary)
        {
            GasStationId = gasStationId;
            CarrierId = carrierId;
            IsPrimary = isPrimary;
        }

        public Guid GasStationId { get; set; }

        public Guid CarrierId { get; set; }

        public bool IsPrimary { get; set; }
    }
}
