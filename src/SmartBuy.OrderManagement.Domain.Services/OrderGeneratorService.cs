using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using SmartBuy.OrderManagement.Infrastructure.Abstractions;
using SmartBuy.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartBuy.OrderManagement.Domain.Services
{
    public class OrderGeneratorService
    {
        private readonly IGasStationRepository _gasStationRepository;

        public OrderGeneratorService(IGasStationRepository gasStationRepository)
        {
            _gasStationRepository = gasStationRepository;
        }

        public async Task<InputOrder> RunOrderGenAsync(Guid gasStationId)
        {
            if (gasStationId == default(Guid))
                throw new ArgumentException("Invalid gas station id");

            var result = await _gasStationRepository.GetGasStationDetailsAsync(gasStationId);

            if (result.OrderType == OrderType.Schedule)
            {
                return new InputOrder
                {
                    OrderType = OrderType.Schedule,
                    Comments = "Schedule Order",
                    GasStationId = gasStationId
                };
            }

            return DefaultOrder.GetInstance.InputOrder;
        }

        private class DayComparer : IDayComparable
        {
            public bool Compare(DayOfWeek targetDay)
            {
                return DateTime.UtcNow.DayOfWeek == targetDay;
            }
        }
    }
}
