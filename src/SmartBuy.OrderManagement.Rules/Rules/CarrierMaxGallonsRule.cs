using FluentValidation.Results;
using SmartBuy.Administration.Domain;
using SmartBuy.Common.Utilities.Repository;
using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBuy.OrderManagement.Rules
{
    internal class CarrierMaxGallonsRule
    {
        private readonly IGenericReadRepository<Carrier> _carrierRepo;

        internal CarrierMaxGallonsRule(IGenericReadRepository<Carrier> carrierRepo)
        {
            _carrierRepo = carrierRepo;
        }

        internal async Task<bool> IsOrderGallonsLessThanOrEqualMaxCapacity(IEnumerable<InputOrder> inputOrders)
        {
            var carrierId = inputOrders.FirstOrDefault().CarrierId;

            var carrier = (await _carrierRepo.FindByKeyAsync(carrierId));

            return carrier.MaxGallons >= inputOrders.Sum(x => x.LineItems.Sum(l => l.Quantity));
        }
    }
}
