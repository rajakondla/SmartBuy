using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;
using System;

namespace SmartBuy.OrderManagement.Domain.Services
{
    public class TankRunout
    {
        public static DateTime GetRunoutTime(TankDetail tankDetail, DateTime runTime)
        {
            int perHourSale = tankDetail.EstimatedDaySale / 24;
            int hour = runTime.Hour;
            int quantity = tankDetail.Quantity;
            do
            {
                if (quantity <= tankDetail.Bottom)
                {
                    // runTime
                }
            } while (true);

            return DateTime.Now;
        }
    }
}
