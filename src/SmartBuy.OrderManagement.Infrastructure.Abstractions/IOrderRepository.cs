using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;
using SmartBuy.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartBuy.OrderManagement.Infrastructure.Abstractions
{
    public interface IOrderRepository
    {
        Task<IEnumerable<OrderDetailDTO>> GetOrdersByGasStationIdAsync(Guid gasStationId,
            OrderType? orderType = null);

        Task<IEnumerable<OrderDetailDTO>> GetOrdersDetailByGasStationIdAsync(Guid gasStationId,
            OrderType? orderType = null);
    }
}
