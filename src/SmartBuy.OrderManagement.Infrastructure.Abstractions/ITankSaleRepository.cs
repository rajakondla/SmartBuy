﻿using SmartBuy.OrderManagement.Domain;
using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartBuy.OrderManagement.Infrastructure.Abstractions
{
    public interface ITankSaleRepository
    {
        Task<IEnumerable<TankSale>> GetSalesByTankAsync(int tankId, DateTime fromTime, DateTime toTime);

        Task<IEnumerable<TankSale>> GetSalesByGasStationAsync(Guid gasStationId, DateTime fromTime, DateTime toTime);
    }
}
