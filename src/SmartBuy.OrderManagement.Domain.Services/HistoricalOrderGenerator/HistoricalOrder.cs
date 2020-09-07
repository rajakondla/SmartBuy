using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartBuy.OrderManagement.Domain.Services.HistoricalOrderGenerator
{
    public class HistoricalOrder : IOrderCreation
    {
        private readonly GasStationDetailDTO _gasStationDetailDTO;

        public HistoricalOrder(GasStationDetailDTO gasStationDetailDTO)
        {
            _gasStationDetailDTO = gasStationDetailDTO;
        }

        public Task<InputOrder> CreateOrderAsync(GasStationDetailDTO gasStationDetailDTO)
        {
            throw new NotImplementedException();
        }
    }
}
