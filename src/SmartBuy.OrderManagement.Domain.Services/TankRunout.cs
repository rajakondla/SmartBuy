using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartBuy.OrderManagement.Domain.Services
{
    public class TankRunout
    {
        public static IEnumerable<TankReading> GetRunoutReadingsByHour(Tank tank, DateTime runTime)
        {
            var perHourSale = tank.EstimatedDaySale / 24.0;
            var reading = new TankReading(runTime, tank.Measurement.Quantity);
            var readingsByHour = new List<TankReading> { reading };
            (double currentQty, DateTime currentReadingTime) currentReading = (reading.Quantity, reading.ReadingTime);

            while (currentReading.currentQty > tank.Measurement.Bottom)
            {
                currentReading.currentQty -= perHourSale;
                currentReading.currentReadingTime = currentReading.currentReadingTime.AddHours(1);
                readingsByHour.Add(reading.AddReading(currentReading.currentReadingTime,
                    Math.Round(currentReading.currentQty, 2)));
            }

            return readingsByHour;
        }

        public static (int TankId, TankReading TankReading) GetFastestRunoutTankReading(IEnumerable<RunoutReading> readings)
        {
            var runoutReadings = new Dictionary<int, TankReading>();
            foreach (var reading in readings)
            {
                var item = reading.TankReadings
                     .Last(x => x.Quantity > reading.Bottom);
                runoutReadings.Add(reading.TankId, item);
            }

            var result = runoutReadings.First(x => x.Value.ReadingTime == runoutReadings.Min(x => x.Value.ReadingTime));

            return (result.Key, result.Value);
        }

        public static IEnumerable<(int TankId, TankReading TankReading)> GetTanksQuantityByReadingTime(IEnumerable<RunoutReading> readings
            , DateTime readingTime)
        {
            var runoutReadings = new List<(int TankId, TankReading TankReading)>();
            foreach (var reading in readings)
            {
                var item = reading.TankReadings
                     .First(x => x.ReadingTime == readingTime);
                runoutReadings.Add((reading.TankId, item));
            }
            return runoutReadings;
        }
    }
}
