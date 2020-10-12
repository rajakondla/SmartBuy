using System;
using System.Collections.Generic;

namespace SmartBuy.OrderManagement.Domain.Services
{
    public struct TankReading
    {
        private readonly DateTime _readingTime;
        public double _quantity;

        public TankReading(DateTime readingTime, double quantity)
        {
            _readingTime = readingTime;
            _quantity = quantity;
        }

        public DateTime ReadingTime => _readingTime;
        public double Quantity => _quantity;

        public TankReading AddReading(DateTime readingTime, double quantity)
        {
            this._quantity -= quantity;
            return new TankReading(readingTime, this._quantity);
        }
    }
    
    public class FastRunoutReading
    {
        public FastRunoutReading()
        {
            TankReadings = new List<TankReading>();
        }

        public int TankId { get; set; }
        public int Bottom { get; set; }

        public IEnumerable<TankReading> TankReadings { get; set; }
    }
}
