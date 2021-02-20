using SmartBuy.OrderManagement.Domain.Services.EstimateOrderGenerator;
using SmartBuy.OrderManagement.Domain.Services.ScheduleOrderGenerator;
using SmartBuy.OrderManagement.Infrastructure.Abstractions;
using SmartBuy.SharedKernel.Enums;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using SmartBuy.SharedKernel;

namespace SmartBuy.OrderManagement.Domain.Services
{
    public class AutoOrderGenerator
    {
        private readonly IGasStationRepository _gasStationRepository;
        private readonly ScheduleOrder _scheduleOrder;
        private readonly EstimateOrder _estimateOrder;
        private readonly IManageOrderRepository _manageOrderRepository;

        public AutoOrderGenerator(IGasStationRepository gasStationRepository
            , ScheduleOrder scheduleOrder
            , EstimateOrder estimateOrder
            , IManageOrderRepository manageOrderRepository)
        {
            _gasStationRepository = gasStationRepository;
            _scheduleOrder = scheduleOrder;
            _estimateOrder = estimateOrder;
            _manageOrderRepository = manageOrderRepository;
        }

        public async Task<IEnumerable<OutputDomainResult<Order>>> RunAsync(
            Guid gasStationId)
        {
            if (gasStationId == default(Guid))
                throw new ArgumentException("Invalid gas station id");

            var outputs = new List<OutputDomainResult<Order>>();
            var gasstationDetail = await _gasStationRepository
                .GetGasStationIncludeTankOrderStrategyAsync(gasStationId);

            var runtime = DateTime.UtcNow;
            if (gasstationDetail.OrderType == OrderType.Schedule)
            {
                var result = await _scheduleOrder.CreateAsync(gasstationDetail);
                outputs.Add(result);
            }
            else if (gasstationDetail.OrderType == OrderType.Estimate)
            {
                var result = _estimateOrder.Create(gasstationDetail, runtime);
                outputs.Add(result);
            }

            return outputs;
        }

        public async Task<Order> CreateOrder(Order order)
        {
            var manageOrder = await _manageOrderRepository.
                GetOrdersByGasStationIdAsync(order.GasStationId);

            order = manageOrder.Add(order);

            if (!order.IsConflicting)
            {
                await _manageOrderRepository.UpsertAsync(manageOrder);
            }

            return order;
        }
    }
}
