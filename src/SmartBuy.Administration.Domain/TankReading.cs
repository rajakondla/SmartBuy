using SmartBuy.SharedKernel;
using System;
namespace SmartBuy.Administration.Domain
{
    public class TankReading : Entity<int>
    {
        public int TankId { get; set; }

        public int Quantity { get; set; }

        public DateTime ReadingTime { get; set; }
    }
}
