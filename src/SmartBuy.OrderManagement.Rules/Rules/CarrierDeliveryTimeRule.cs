using SmartBuy.Common.Utilities.Repository;
using SmartBuy.OrderManagement.Domain;
using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using System.Collections.Generic;

namespace SmartBuy.OrderManagement.Rules
{
    internal class CarrierDeliveryTimeRule
    {
        private readonly IGenericReadRepository<Carrier> _carrierRepo;

        internal CarrierDeliveryTimeRule(IGenericReadRepository<Carrier> carrierRepo)
        {
            _carrierRepo = carrierRepo;
        }

        internal bool IsInBetweenDeliveryTime(IEnumerable<InputOrder> inputOrders)
        {
            //if(inputOrders == null)
            //    throw new ArgumentExcept

            return true;
        }
    }
}
