using SmartBuy.OrderManagement.Domain;
using SmartBuy.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartBuy.OrderManagement.Infrastructure.Abstractions
{
    public interface IManageOrderRepository
    {
        Task<ManageOrder> GetOrdersByGasStationIdAsync(Guid gasStationId,
            OrderType? orderType = null);

        Task<ManageOrder> GetLatestOrdersByGasStationAndOrderTypeAsync((Guid gasStationId,
            OrderType orderType) gasStation);

        Task<ManageOrder> GetOrdersDetailByGasStationIdAsync(Guid gasStationId,
            OrderType? orderType = null);

        Task<ManageOrder> GetOrdersByGasStationIdsAsync(IEnumerable<Guid> gasStationIds);
    }
}
