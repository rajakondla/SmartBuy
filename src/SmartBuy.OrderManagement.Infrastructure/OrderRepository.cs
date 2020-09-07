using Microsoft.EntityFrameworkCore;
using SmartBuy.OrderManagement.Infrastructure.Abstractions;
using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;
using SmartBuy.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBuy.OrderManagement.Infrastructure
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderContext _orderContext;

        public OrderRepository(OrderContext orderContext)
        {
            _orderContext = orderContext;
        }

        public async Task<IEnumerable<OrderDetailDTO>> GetOrdersByGasStationIdAsync(Guid gasStationId,
            OrderType? orderType = null)
        {
            return await _orderContext.Orders.Where(o => o.GasStationId == gasStationId &&
            (orderType == null || orderType == o.OrderType))
                .Select(x => new OrderDetailDTO
                {
                    GasStationId = x.GasStationId,
                    FromDateTime = x.DispatchDate.Start,
                    ToDateTime = x.DispatchDate.End
                }).ToListAsync();
        }

        public Task<IEnumerable<OrderDetailDTO>> GetOrdersDetailByGasStationIdAsync(Guid gasStationId,
            OrderType? orderType = null)
        {
            throw new NotImplementedException();
        }
    }
}
