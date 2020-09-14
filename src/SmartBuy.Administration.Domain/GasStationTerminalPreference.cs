using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBuy.Administration.Domain
{
    public class GasStationTerminalPreference
    {
        public Guid GasStationId { get; set; }

        public Guid TerminalId { get; set; }

        public bool IsPrimary { get; set; }
    }
}
