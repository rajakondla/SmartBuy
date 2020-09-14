﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBuy.Administration.Domain
{
    public class GasStationTerminalPreference
    {
        private GasStationTerminalPreference()
        {

        }

        public GasStationTerminalPreference(Guid gasStationId, Guid terminalId,
            bool isPrimary)
        {
            GasStationId = gasStationId;
            TerminalId = terminalId;
            IsPrimary = isPrimary;
        }

        public Guid GasStationId { get; set; }

        public Guid TerminalId { get; set; }

        public bool IsPrimary { get; set; }
    }
}
