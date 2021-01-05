using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using SmartBuy.OrderManagement.Domain.Services.EstimateOrderGenerator;
using SmartBuy.OrderManagement.Domain.Services.ScheduleOrderGenerator;
using SmartBuy.OrderManagement.Infrastructure.Abstractions;
using SmartBuy.SharedKernel.Enums;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using SmartBuy.SharedKernel;
using System.Linq;

namespace SmartBuy.OrderManagement.Domain.Services
{
    public class OrderGenerator
    {
        private readonly IGasStationRepository _gasStationRepository;
        private readonly ScheduleOrder _scheduleOrder;
        private readonly EstimateOrder _estimateOrder;
        private readonly IManageOrderRepository _manageOrderRepository;

        public OrderGenerator(IGasStationRepository gasStationRepository
            , ScheduleOrder scheduleOrder
            , EstimateOrder estimateOrder
            , IManageOrderRepository manageOrderRepository)
        {
            _gasStationRepository = gasStationRepository;
            _scheduleOrder = scheduleOrder;
            _estimateOrder = estimateOrder;
            _manageOrderRepository = manageOrderRepository;
        }

        public async Task<IEnumerable<OutputDomainResult<Order>>> AutoOrderGenAsync(Guid gasStationId)
        {
            if (gasStationId == default(Guid))
                throw new ArgumentException("Invalid gas station id");

            var outputs = new List<OutputDomainResult<Order>>();
            var gasstationDetail = await _gasStationRepository
                .GetGasStationIncludeTankOrderStrategyAsync(gasStationId);

            var runtime = DateTime.UtcNow;
            if (gasstationDetail.OrderType == OrderType.Schedule)
            {
                var result = await _scheduleOrder.GetAsync(gasstationDetail);
                outputs.Add(result);
            }
            else if (gasstationDetail.OrderType == OrderType.Estimate)
            {
                var result = _estimateOrder.Get(gasstationDetail, runtime);
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

                return (await _manageOrderRepository.GetOrdersByGasStationIdAsync(order.GasStationId,
                    order.OrderType)).Orders.First();
            }
            else
                return order;
            //try
            //{
            //    await _manageOrderRepository.UpsertAsync(manageOrder);
            //}
            //catch (Exception e) when (e.InnerException.GetType() == typeof(SqlException))
            //{
            //    return new OrderViewModel
            //    {
            //        IsSuccess = false,
            //        Message = new[] { e.Message }
            //    };
            //}
            //catch (Exception)
            //{
            //    throw;
            //}

            //var orderId = (await _manageOrderRepository.GetOrdersByGasStationIdAsync(orderInput.GasStationId,
            //    orderInput.OrderType)).Orders.First().Id;
        }
    }
}
