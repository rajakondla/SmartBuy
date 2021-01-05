using SmartBuy.OrderManagement.Domain;
using SmartBuy.SharedKernel.Enums;
using System;
using System.Threading.Tasks;

namespace SmartBuy.OrderManagement.Infrastructure.Abstractions
{
    public interface IGasStationRepository
    {
        Task<(GasStation GasStation, OrderType OrderType)> GetGasStationIncludeTankOrderStrategyAsync(Guid gasStationId);
    }
}
