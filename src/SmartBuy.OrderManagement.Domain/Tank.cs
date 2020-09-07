using SmartBuy.SharedKernel;
using SmartBuy.SharedKernel.ValueObjects;
using System;

namespace SmartBuy.OrderManagement.Domain
{
    public class Tank : Entity<int>
    {
        private Tank()
        {

        }

        public Tank(int id, Guid gasStationId,
            int productId, Measurement measurement) : base(id)
        {
            GasStationId = gasStationId;
            ProductId = productId;
            Measurement = measurement;
            ProductId = productId;
        }

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
