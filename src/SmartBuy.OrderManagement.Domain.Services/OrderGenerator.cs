using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using SmartBuy.OrderManagement.Domain.Services.EstimateOrderGenerator;
using SmartBuy.OrderManagement.Domain.Services.ScheduleOrderGenerator;
using SmartBuy.OrderManagement.Infrastructure.Abstractions;
using SmartBuy.SharedKernel.Enums;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SmartBuy.OrderManagement.Domain.Services
{
    public class OrderGenerator
    {
        private readonly IGasStationRepository _gasStationRepository;
        private readonly ScheduleOrder _scheduleOrder;
        private readonly EstimateOrder _estimateOrder;

        public OrderGenerator(IGasStationRepository gasStationRepository
            , ScheduleOrder scheduleOrder
            , EstimateOrder estimateOrder)
        {
            _gasStationRepository = gasStationRepository;
            _scheduleOrder = scheduleOrder;
            _estimateOrder = estimateOrder;
        }

        public async Task<IEnumerable<InputOrder>> RunOrderGenAsync(Guid gasStationId)
        {
            if (gasStationId == default(Guid))
                throw new ArgumentException("Invalid gas station id");

            var inputOrders = new List<InputOrder>();
            var result = await _gasStationRepository.GetGasStationDetailsAsync(gasStationId);

            var runtime = DateTime.UtcNow;
            if (result.OrderType == OrderType.Schedule)
            {
                inputOrders.Add(await _scheduleOrder.CreateOrderAsync(result));
            }
            else if (result.OrderType == OrderType.Estimate)
            {
                inputOrders.Add(_estimateOrder.CreateOrder(result, runtime));
            }

            return inputOrders;
        }
    }
}
