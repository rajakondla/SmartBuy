using FluentValidation;
using FluentValidation.Results;
using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SmartBuy.OrderManagement.Domain.Validattion
{
    public class InputOrderValidator : AbstractValidator<InputOrder>
    {
        private GasStation _gasStation;
        public InputOrderValidator(GasStation gasStation)
        {
            _gasStation = gasStation;
            RuleFor(x => x.Comments).NotEmpty()
                .WithMessage("Comments should not be blank");
            RuleFor(x => x.GasStationId).Equal(_gasStation.Id)
                .WithMessage("Invalid Gas Station");
            RuleFor(x => x.LineItems).Must(x => x.Any())
                .WithMessage("No line items for this order");
            RuleForEach(x => x.LineItems).SetValidator(new InputOrderProductValidator());
            RuleFor(x => x.LineItems).Must(IsLineItemsValid)
                .WithMessage("Gas Station tanks and Order tanks are not same");
        }

        private bool IsLineItemsValid(IEnumerable<InputOrderProduct> lineItems)
        {
            var tanks = _gasStation.Tanks.ToList();
            return (from lineItem in lineItems
                    where tanks.Any(x => x.Id != lineItem.TankId)
                    select lineItem).Any();
        }
    }
}
