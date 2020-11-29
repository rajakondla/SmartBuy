using SmartBuy.Common.Utilities.Repository;
using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;
using SmartBuy.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace SmartBuy.OrderManagement.Domain.Services.EstimateOrderGenerator
{
    public class EstimateOrder
    {
        public InputOrder CreateOrder(GasStationDetailDTO gasStationDetailDTO
            , DateTime runTime)
        {
            if (gasStationDetailDTO is null)
                throw new ArgumentNullException(nameof(gasStationDetailDTO));

            if (gasStationDetailDTO.OrderType == OrderType.Estimate)
            {
                var allTankReadings = new List<(TankDetail tank, IEnumerable<TankReading> TankReadings)>();

                foreach (var tank in gasStationDetailDTO.TankDetails)
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

                return new InputOrder
                {
                    GasStationId = gasStationDetailDTO.GasStationId,
                    Comments = "Estimate order created by system",
                    FromTime = fastestRunoutTank.TankReading.ReadingTime,
                    ToTime = fastestRunoutTank.TankReading.ReadingTime.AddHours(4),
                    OrderType = OrderType.Estimate,
                    LineItems = allTankQuantities.Select(x => new InputOrderProduct
                    {
                        TankId = x.TankId,
                        Quantity = (gasStationDetailDTO.TankDetails
                                   .First(t => t.Id == x.TankId)
                                   .Measurement.NetQuantity) - (int)Math.Round(x.TankReading.Quantity),
                    }).ToList()
                };
            }

            return DefaultOrder.GetInstance.InputOrder;
        }
    }
}
