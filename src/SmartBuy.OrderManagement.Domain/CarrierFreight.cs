using SmartBuy.SharedKernel;
using SmartBuy.SharedKernel.ValueObjects;
using System;

namespace SmartBuy.Administration.Domain
{
    public class CarrierFreight
    {
        private CarrierFreight()
        {

        }

        public CarrierFreight(Guid gasStationId, Guid terminalId, Guid carrierId, 
            Money cost)
        {
            GasStationId = gasStationId;
            TerminalId = terminalId;
            CarrierId = carrierId;
            Cost = cost;
        }

        public Guid GasStationId { get; set; }

        public Guid TerminalId { get; set; }

        public Guid CarrierId { get; set; }

        public Money Cost { get; set; }
    }
}
