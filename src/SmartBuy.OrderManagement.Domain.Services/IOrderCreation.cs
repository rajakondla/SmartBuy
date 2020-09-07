using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;
using System.Threading.Tasks;

namespace SmartBuy.OrderManagement.Domain.Services
{
    public interface IOrderCreation
    {
        Task<InputOrder> CreateOrderAsync(GasStationDetailDTO gasStationDetailDTO);
    }
}
