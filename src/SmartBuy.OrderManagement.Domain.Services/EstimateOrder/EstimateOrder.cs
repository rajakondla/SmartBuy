using SmartBuy.Common.Utilities.Repository;
using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;
using SmartBuy.SharedKernel;
using SmartBuy.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartBuy.OrderManagement.Domain.Services.EstimateOrderGenerator
{
    public class EstimateOrder
    {
        public OutputDomainResult<Order> Get((GasStation GasStation, OrderType OrderType) gasStationDetail
            , DateTime runTime)
        {
            if (gasStationDetail.OrderType == OrderType.Estimate)
            {
                var allTankReadings = new List<(Tank tank, IEnumerable<TankReading> TankReadings)>();

                foreach (var tank in gasStationDetail.GasStation.Tanks)
                    allTankReadings.Add((tank, TankRunout.GetRunoutReadingsByHour(tank, runTime)));

                var readings = allTankReadings.Select(x => new RunoutReading
                {
                    Bottom = x.tank.Measurement.Bottom,
                    TankId = x.tank.Id,
                    TankReadings = x.TankReadings
                }).ToList();

                var fastestRunoutTank = TankRunout.GetFastestRunoutTankReading(
                    readings
                    );

                var allTankQuantities = TankRunout.GetTanksQuantityByReadingTime(readings,
                    fastestRunoutTank.TankReading.ReadingTime);

                var inputOrder = new InputOrder
                {
                    GasStationId = gasStationDetail.GasStation.Id,
                    Comments = "Estimate order created by system",
                    FromTime = fastestRunoutTank.TankReading.ReadingTime,
                    ToTime = fastestRunoutTank.TankReading.ReadingTime.AddHours(4),
                    OrderType = OrderType.Estimate,
                    LineItems = allTankQuantities.Select(x => new InputOrderProduct
                    {
                        TankId = x.TankId,
                        Quantity = (gasStationDetail.GasStation.Tanks
                                   .First(t => t.Id == x.TankId)
                                   .Measurement.NetQuantity) - (int)Math.Round(x.TankReading.Quantity),
                    }).ToList()
                };

                return Order.Create(inputOrder, gasStationDetail.GasStation);
            }

            return DefaultOutputDomainResult.GetInstance.Order;
        }
    }
}
