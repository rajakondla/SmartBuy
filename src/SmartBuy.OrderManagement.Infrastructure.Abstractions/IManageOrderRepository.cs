using SmartBuy.OrderManagement.Domain;
using SmartBuy.SharedKernel;
using SmartBuy.SharedKernel.Enums;
using SmartBuy.SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartBuy.OrderManagement.Infrastructure.Abstractions
{
    public interface IManageOrderRepository
    {
        Task<ManageOrder> GetOrdersByGasStationIdAsync(Guid gasStationId);

        Task<Order> GetOrderByGasStationIdDeliveryDateAsync(Guid gasStationId, DateTimeRange dispatchDate);

        Task UpsertAsync(ManageOrder manageOrder);
    }
}
