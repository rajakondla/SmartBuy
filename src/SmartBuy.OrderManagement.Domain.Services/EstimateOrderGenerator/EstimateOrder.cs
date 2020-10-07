using SmartBuy.Common.Utilities.Repository;
using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;
using SmartBuy.SharedKernel.Enums;
using System;
using System.Threading.Tasks;

namespace SmartBuy.OrderManagement.Domain.Services.EstimateOrderGenerator
{
    public class EstimateOrder
    {
        public readonly IGenericReadRepository<GasStation> _gasStations;

        public EstimateOrder(IGenericReadRepository<GasStation> gasStations)
        {
            _gasStations = gasStations;
        }

        public async Task<InputOrder> CreateOrderAsync(GasStationDetailDTO gasStationDetailDTO)
        {
            if (gasStationDetailDTO is null)
                throw new ArgumentNullException(nameof(gasStationDetailDTO));

            if (gasStationDetailDTO.OrderType == OrderType.Estimate)
            {

            }

            return DefaultOrder.GetInstance.InputOrder;
        }
    }
}
