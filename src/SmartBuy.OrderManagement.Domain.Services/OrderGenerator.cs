using SmartBuy.OrderManagement.Infrastructure.Abstractions;
using System.Threading.Tasks;

namespace SmartBuy.OrderManagement.Domain.Services
{
    public class OrderGenerator
    {
        private readonly IManageOrderRepository _manageOrderRepository;

        public OrderGenerator(IManageOrderRepository manageOrderRepository)
        {
            _manageOrderRepository = manageOrderRepository;
        }
        public async Task<Order> SaveAsync(Order order)
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
