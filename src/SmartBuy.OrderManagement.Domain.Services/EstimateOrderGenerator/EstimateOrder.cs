using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartBuy.OrderManagement.Domain.Services.EstimateOrderGenerator
{
    public class EstimateOrder : IOrderCreation
    {
        private readonly GasStationDetailDTO _gasStationDetailDTO;

        public EstimateOrder(GasStationDetailDTO gasStationDetailDTO)
        {
            _gasStationDetailDTO = gasStationDetailDTO;
        }

        public Task<InputOrder> CreateOrderAsync(GasStationDetailDTO gasStationDetailDTO)
        {
            throw new NotImplementedException();
        }
    }
}
