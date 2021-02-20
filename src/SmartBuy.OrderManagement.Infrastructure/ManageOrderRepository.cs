using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartBuy.OrderManagement.Domain;
using SmartBuy.OrderManagement.Infrastructure.Abstractions;
using SmartBuy.SharedKernel.Enums;
using SmartBuy.SharedKernel.ValueObjects;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBuy.OrderManagement.Infrastructure
{
    public class ManageOrderRepository : IManageOrderRepository
    {
        private readonly OrderContext _orderContext;
        private readonly ILogger _logger;

        public ManageOrderRepository(OrderContext orderContext,
            ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("Database");
            _orderContext = orderContext;
        }

        public async Task<ManageOrder> GetOrdersByGasStationIdAsync(Guid gasStationId)
        {
            var orders = await _orderContext.Orders
                .Where(o => o.GasStationId == gasStationId).ToListAsync();

            var manageOrder = new ManageOrder();

            foreach (var order in orders)
                manageOrder.Add(order);

            return manageOrder;
        }

        public async Task<Order> GetOrderByGasStationIdDeliveryDateAsync(Guid gasStationId,
            DateTimeRange dispatchDate)
        {
            _logger.LogDebug(new EventId(123), "Debug from GetOrderByGasStationIdDeliveryDateAsync");

            return await _orderContext.Orders.FirstOrDefaultAsync(o => o.GasStationId == gasStationId &&
             o.DispatchDate.Start == dispatchDate.Start && o.DispatchDate.End == dispatchDate.End);
        }

        public async Task UpsertAsync(ManageOrder manageOrder)
        {
            void insertOrder(Order order)
            {
                order.CreatedDate = DateTime.UtcNow;
                order.ModifiedDate = DateTime.UtcNow;
                foreach (var item in order.OrderProducts)
                    _orderContext.Entry(item).State = EntityState.Added;
                _orderContext.Entry(order).State = EntityState.Added;
            }

            void updateOrder(Order order)
            {
                order.ModifiedDate = DateTime.UtcNow;

                _orderContext.Entry(order).State = EntityState.Modified;
            }

            foreach (var order in manageOrder.Orders)
            {
                if (order.State == TrackingState.Added)
                    insertOrder(order);
                else if (order.State == TrackingState.Modified)
                    updateOrder(order);
            }

            await _orderContext.SaveChangesAsync();
        }
    }
}
