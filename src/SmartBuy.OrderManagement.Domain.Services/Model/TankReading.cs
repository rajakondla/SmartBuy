using System;
using System.Collections.Generic;

namespace SmartBuy.OrderManagement.Domain.Services
{
    public struct TankReading
    {
        private readonly DateTime _readingTime;
        public readonly double _quantity;

        public TankReading(DateTime readingTime, double quantity)
        {
            _readingTime = readingTime;
            _quantity = quantity;
        }

        public DateTime ReadingTime => _readingTime;
        public double Quantity => _quantity;

        public TankReading AddReading(DateTime readingTime, double quantity)
        {
            return new TankReading(readingTime, quantity);
        }
    }
    
    public class RunoutReading
    {
        public RunoutReading()
        {
            TankReadings = new List<TankReading>();
        }

        public int TankId { get; set; }
        public int Bottom { get; set; }

        public IEnumerable<TankReading> TankReadings { get; set; }
    }
}
