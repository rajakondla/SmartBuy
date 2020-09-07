using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBuy.SharedDatabase
{
    public class GasStation
    {
        public GasStation()
        {
            Tanks = new List<Tank>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public IList<Tank> Tanks { get; private set; }
    }
}
