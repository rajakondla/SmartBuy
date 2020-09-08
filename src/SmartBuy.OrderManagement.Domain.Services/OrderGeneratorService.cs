using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using SmartBuy.OrderManagement.Domain.Services.ScheduleOrderGenerator;
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
        private readonly ScheduleOrder _scheduleOrder;

        public OrderGeneratorService(IGasStationRepository gasStationRepository,
            ScheduleOrder scheduleOrder)
        {
            _gasStationRepository = gasStationRepository;
            _scheduleOrder = scheduleOrder;
        }

        public async Task<InputOrder> RunOrderGenAsync(Guid gasStationId)
        {
            if (gasStationId == default(Guid))
                throw new ArgumentException("Invalid gas station id");

            var result = await _gasStationRepository.GetGasStationDetailsAsync(gasStationId);

            if (result.OrderType == OrderType.Schedule)
            {
                return await _scheduleOrder.CreateOrderAsync(result);
            }

            return DefaultOrder.GetInstance.InputOrder;
        }
    }
}
