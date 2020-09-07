using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBuy.SharedDatabase
{
    public class Tank
    {
        public Tank(GasStation gasStation)
        {
            GasStation = gasStation;
            GasStationId = gasStation.Id;
        }

        public Tank()
        {

        }

        public int Id { get; set; }

        public string Name { get; set; }

        public Guid GasStationId { get; set; }

        public GasStation GasStation { get; private set; }
    }
}
