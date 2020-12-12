using Microsoft.EntityFrameworkCore;
using SmartBuy.OrderManagement.Domain;
using SmartBuy.OrderManagement.Infrastructure.Abstractions;
using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;
using SmartBuy.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBuy.OrderManagement.Infrastructure
{
    public class ManageOrderRepository : IManageOrderRepository
    {
        private readonly OrderContext _orderContext;

        public ManageOrderRepository(OrderContext orderContext)
        {
            _orderContext = orderContext;
        }

        public Task<ManageOrder> GetLatestOrdersByGasStationAndOrderTypeAsync((Guid gasStationId, OrderType orderType) gasStation)
        {
            throw new NotImplementedException();
        }

        public async Task<ManageOrder> GetOrdersByGasStationIdAsync(Guid gasStationId, OrderType? orderType = null)
        {
            var orders = await _orderContext.Orders
                .Where(o => o.GasStationId == gasStationId &&
                        (orderType == null || orderType == o.OrderType))
                .Select(o => o).ToListAsync();

            return new ManageOrder(orders);
        }

        public Task<ManageOrder> GetOrdersByGasStationIdsAsync(IEnumerable<Guid> gasStationIds)
        {
            throw new NotImplementedException();
        }

        public Task<ManageOrder> GetOrdersDetailByGasStationIdAsync(Guid gasStationId, OrderType? orderType = null)
        {
            throw new NotImplementedException();
        }
    }
}
