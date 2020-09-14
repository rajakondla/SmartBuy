using SmartBuy.SharedKernel;
using SmartBuy.SharedKernel.ValueObjects;
using System;

namespace SmartBuy.Administration.Domain
{
    public class CarrierFreight
    {
        public Guid GasStationId { get; set; }

        public Guid TerminalId { get; set; }

        public Guid CarrierId { get; set; }

        public Money Cost { get; set; }
    }
}
