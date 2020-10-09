using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace SmartBuy.OrderManagement.Domain.Services
{
    public class TankRunout
    {
        public static IEnumerable<TankReading> GetRunoutReadingsByHour(TankDetail tankDetail, DateTime runTime)
        {
            var perHourSale = tankDetail.EstimatedDaySale / 24.0;
            var currentReading = new TankReading(runTime, tankDetail.Quantity);
            var readingsByHour = new List<TankReading> { currentReading };

            while (currentReading.Quantity > tankDetail.Bottom)
            {
                readingsByHour.Add(currentReading.AddReading(currentReading.ReadingTime.AddHours(1), perHourSale));
            }

            return readingsByHour;
        }

        public static IEnumerable<(int TankId, TankReading TankReading)> GetFastRunoutReadings(IEnumerable<FastRunoutReading> readings)
        {
            var runoutReadings = new Dictionary<int, TankReading>();
            foreach(var reading in readings)
            {
               var item= reading.TankReadings
                    .Where(x => x.Quantity > reading.Bottom)
                    .Last();
                runoutReadings.Add(reading.TankId, item);
            }
        }
    }
}
