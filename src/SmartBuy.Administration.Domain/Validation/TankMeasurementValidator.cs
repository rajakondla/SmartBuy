using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;
using SmartBuy.SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartBuy.Administration.Domain.Validation
{
    public class TankMeasurementValidator : AbstractValidator<Measurement>
    {
        public TankMeasurementValidator()
        {
            RuleFor(x => new
            {
                x.NetQuantity,
                x.Bottom,
                x.Quantity,
                x.Top
            })
                .Must(x => x.NetQuantity >= (x.Top + x.Bottom + x.Quantity))
                .WithMessage("Tank capacity should not be less than (Top + Bottom + Quantity)");
        }
    }
}
