
using SmartBuy.Common.Utilities.Repository;
using SmartBuy.OrderManagement.Domain;
using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SmartBuy.OrderManagement.Rules
{
    public class BasicOrderRule
    {
        private readonly DispatcherGroupRule _dispatcherGroupRule;
        private readonly CarrierRule _carrierRule;
        private readonly CarrierMaxGallonsRule _carrierMaxGallonsRule;
        private readonly CarrierDeliveryTimeRule _carrierDeliveryTimeRule;

        public BasicOrderRule(IGenericReadRepository<GasStation> gasStationRepo
            , IGenericReadRepository<Carrier> carrierRepo)
        {
            _dispatcherGroupRule = new DispatcherGroupRule(gasStationRepo);
            _carrierRule = new CarrierRule();
            _carrierMaxGallonsRule = new CarrierMaxGallonsRule(carrierRepo);
            _carrierDeliveryTimeRule = new CarrierDeliveryTimeRule(carrierRepo);
        }

        public async Task<bool> ValidateOrders(IEnumerable<InputOrder> inputOrders
            , int orderLinkTresholdValue)
        {
            var result = await _dispatcherGroupRule.IsDispatcherSame(inputOrders);

            if (result)
            {
                result = _carrierRule.IsCarrierSame(inputOrders);

                if (result)
                {
                    result = await _carrierMaxGallonsRule.IsOrderGallonsLessThanOrEqualMaxCapacity(
                        inputOrders
                        );

                    if (result)
                    {
                        result = await _carrierDeliveryTimeRule.IsInBetweenDeliveryTime(
                       inputOrders, orderLinkTresholdValue
                       );
                    }
                }
            }

            return result;
        }
    }
}
