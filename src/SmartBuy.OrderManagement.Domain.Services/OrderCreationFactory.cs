using SmartBuy.OrderManagement.Domain.Services.EstimateOrderGenerator;
using SmartBuy.OrderManagement.Domain.Services.HistoricalOrderGenerator;
using SmartBuy.OrderManagement.Domain.Services.ScheduleOrderGenerator;
using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;
using SmartBuy.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBuy.OrderManagement.Domain.Services
{
    internal class OrderCreationFactory
    {
        public IOrderCreation Generate(GasStationDetailDTO gasStationDetail)
        {
            //switch (gasStationDetail.OrderType)
            //{
            //    case OrderType.Schedule:
            //        return new ScheduleOrder(gasStationDetail);
            //    case OrderType.Estimate:
            //        return new EstimateOrder(gasStationDetail);
            //    case OrderType.Historical:
            //        return new HistoricalOrder(gasStationDetail);
            //}
            return null;
        }
    }
}
