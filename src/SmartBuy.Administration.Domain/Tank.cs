using SmartBuy.Administration.Domain.Validation;
using SmartBuy.SharedKernel;
using SmartBuy.SharedKernel.ValueObjects;
using System;
using System.Linq;
using System.Net.Http.Headers;

namespace SmartBuy.Administration.Domain
{
    public class Tank : Entity<int>
    {
        public Tank()
        {
            GasStationId = default(Guid);
            ProductId = 0;
            Name = "";
            Number = 0;
            Measurement = new Measurement();
        }

        public Tank(int id) :base(id)
        {
            GasStationId = default(Guid);
            ProductId = 0;
            Name = "";
            Number = 0;
            Measurement = new Measurement();
        }

        public string Name { get; set; }

        public int Number { get; set; }

        public Guid GasStationId { get; set; }

        public int ProductId { get; set; }

        public Measurement Measurement { get; private set; }

        public int EstimatedDaySale { get; set; }

        public Tank AddMeasurement(Measurement measurement)
        {
            var tankValidator = new TankMeasurementValidator();
            var result = tankValidator.Validate(measurement);
            if (result.IsValid)
            {
                Measurement = measurement;
            }
            else
            {
                throw new ArgumentException(result.Errors.Select(x => x.ErrorMessage).FirstOrDefault());
            }
            return this;
        }
    }
}
