using FluentValidation;
using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SmartBuy.OrderManagement.Domain.Validattion
{
    public class InputOrderProductValidator : AbstractValidator<InputOrderProduct>
    {
        public InputOrderProductValidator()
        {
            RuleFor(x => x.Quantity).GreaterThan(0)
                .WithMessage("Quantity should be greater than zero");
            RuleFor(x => x.TankId).GreaterThan(0)
                .WithMessage("TankId should be greater than zero");
        }
    }
}
