using SmartBuy.Common.Utilities.Repository;
using SmartBuy.OrderManagement.Domain;
using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using SmartBuy.SharedKernel.ValueObjects;

namespace SmartBuy.OrderManagement.Rules
{
    internal class CarrierDeliveryTimeRule
    {
        private readonly IGenericReadRepository<Carrier> _carrierRepo;

        internal CarrierDeliveryTimeRule(IGenericReadRepository<Carrier> carrierRepo)
        {
            _carrierRepo = carrierRepo;
        }

        internal async Task<bool> IsInBetweenDeliveryTime(IEnumerable<InputOrder> inputOrders,
            int thresholdHours)
        {
            if (inputOrders == null)
                throw new ArgumentException("input orders parameter cannot be null",
                    nameof(inputOrders));

            var carrierDeliveryTime = (await _carrierRepo.FindByKeyAsync(inputOrders.FirstOrDefault()
                .CarrierId)).DeliveryTime;

            foreach (var order in inputOrders)
            {
                if (IsInValidateFromTime(order.FromTime.TimeOfDay, carrierDeliveryTime, thresholdHours)
                    &&
                    IsInValidateToTime(order.ToTime.TimeOfDay, carrierDeliveryTime, thresholdHours))

                    return false;
            }

            return true;
        }

        private static bool IsInValidateFromTime(TimeSpan fromTime, TimeRange timeRange,
            int thresholdHours)
        {
            return fromTime
                     < timeRange.Start.Subtract(new TimeSpan(thresholdHours, 0, 0))
                     || fromTime > timeRange.End
                     .Add(new TimeSpan(thresholdHours, 0, 0));
        }

        private static bool IsInValidateToTime(TimeSpan toTime, TimeRange timeRange,
            int thresholdHours)
        {
            return toTime
                     < timeRange.Start.Subtract(new TimeSpan(thresholdHours, 0, 0))
                     || toTime > timeRange.End
                     .Add(new TimeSpan(thresholdHours, 0, 0));
        }
    }
}
