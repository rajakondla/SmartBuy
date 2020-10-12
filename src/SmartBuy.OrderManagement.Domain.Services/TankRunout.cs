using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartBuy.OrderManagement.Domain.Services
{
    public class TankRunout
    {
        public static IEnumerable<TankReading> GetRunoutReadingsByHour(TankDetail tankDetail, DateTime runTime)
        {
            var perHourSale = tankDetail.EstimatedDaySale / 24.0;
            var reading = new TankReading(runTime, tankDetail.Quantity);
            var readingsByHour = new List<TankReading> { reading };
            (double currentQty, DateTime currentReadingTime)  currentReading = (reading.Quantity, reading.ReadingTime);

            while (currentReading.currentQty > tankDetail.Bottom)
            {
                currentReading.currentQty -= perHourSale;
                currentReading.currentReadingTime = currentReading.currentReadingTime.AddHours(1);
                readingsByHour.Add(reading.AddReading(currentReading.currentReadingTime,
                    currentReading.currentQty));
            }

            return readingsByHour;
        }

        public static (int TankId, TankReading TankReading) GetFastestRunoutTankReading(IEnumerable<FastRunoutReading> readings)
        {
            var runoutReadings = new Dictionary<int, TankReading>();
            foreach (var reading in readings)
            {
                var item = reading.TankReadings
                     .Where(x => x.Quantity > reading.Bottom)
                     .Last();
                runoutReadings.Add(reading.TankId, item);
            }

            var result = runoutReadings.First(x => x.Value.ReadingTime == runoutReadings.Min(x => x.Value.ReadingTime));

            return (result.Key, result.Value);
        }
    }
}
