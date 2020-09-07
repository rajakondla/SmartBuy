using SmartBuy.SharedKernel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SmartBuy.OrderManagement.Domain
{
    public class TankReading
    {
        public TankReading(int tankId, int quantity,
            DateTime readingTime)
        {
            TankId = tankId;
            Quantity = quantity;
            ReadingTime = readingTime;
        }

        private TankReading()
        {

        }

        public int TankId { get; private set; }

        public int Quantity { get; private set; }

        public DateTime ReadingTime { get; private set; }
    }
}
