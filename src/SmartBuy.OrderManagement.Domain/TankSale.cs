using System;

namespace SmartBuy.OrderManagement.Domain
{
    public class TankSale
    { 
        public TankSale(int tankId, int quantity,
            DateTime readingTime)
        {
            TankId = tankId;
            Quantity = quantity;
            SaleTime = readingTime;
        }

        private TankSale()
        {

        }

        public int TankId { get; private set; }

        public int Quantity { get; private set; }

        public DateTime SaleTime { get; private set; }
    }
}
