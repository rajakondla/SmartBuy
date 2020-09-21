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
        public readonly IGenericReadRepository<TankReading> _tankReadings;

        public EstimateOrder(IGenericReadRepository<GasStation> gasStations
            , IGenericReadRepository<TankReading> tankReadings)
        {
            _gasStations = gasStations;
            _tankReadings = tankReadings;
        }

        public async Task<InputOrder> CreateOrderAsync(GasStationDetailDTO gasStationDetailDTO)
        {
            if (gasStationDetailDTO is null)
                throw new ArgumentException("gasStationDetailDTO cannot be null",
                    nameof(gasStationDetailDTO));

            if (gasStationDetailDTO.OrderType == OrderType.Estimate)
            {

            }

            return DefaultOrder.GetInstance.InputOrder;
        }
    }
}
