using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;
using System;
using System.Threading.Tasks;

namespace SmartBuy.OrderManagement.Infrastructure.Abstractions
{
    public interface IGasStationRepository
    {
        Task<GasStationDetailDTO> GetGasStationDetailsAsync(Guid gasStationId);
    }
}
