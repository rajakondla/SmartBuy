﻿using SmartBuy.SharedKernel;
using SmartBuy.SharedKernel.Enums;
using SmartBuy.SharedKernel.ValueObjects;
using System;

namespace SmartBuy.OrderManagement.Domain
{
    public class Tank
    {
        private Tank()
        {

        }

        public Tank(int id, Guid gasStationId,
            int productId, Measurement measurement, int estimatedDaySale)
        {
            Id = id;
            GasStationId = gasStationId;
            ProductId = productId;
            Measurement = measurement;
            ProductId = productId;
            EstimatedDaySale = estimatedDaySale;
        }

        public int Id { get; private set; }

        public Guid GasStationId { get; private set; }

        public int ProductId { get; private set; }

        public Measurement Measurement { get; private set; }

        public int EstimatedDaySale { get; private set; }

        public int NetQuantity =>
                (Measurement.Unit == TankMeasurement.Gallons) ?
                    Measurement.NetQuantity :
                    Measurement.NetQuantity * 42;
    }
}
