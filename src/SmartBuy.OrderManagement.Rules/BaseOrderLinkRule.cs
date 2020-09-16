using SmartBuy.Common.Utilities.Repository;
using SmartBuy.OrderManagement.Domain;
using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBuy.OrderManagement.Rules
{
    public class BaseOrderLinkRule
    {
        private readonly IGenericReadRepository<GasStation> _gasStationRepo;

        public BaseOrderLinkRule(IGenericReadRepository<GasStation> gasStationRepo)
        {
            _gasStationRepo = gasStationRepo;
        }

        private async Task<bool> IsSameDispatcherGroup(IEnumerable<InputOrder> inputOrders)
        {
            var distinctGasStationIds = inputOrders.Select(x => x.GasStationId).Distinct();

            var gasStations = await _gasStationRepo.FindByAsync(x => distinctGasStationIds.Contains(x.Id));

            return gasStations.Select(x => x.DispatcherGroupId).Distinct().Count() == 1;
        }

        private bool IsCarrierSame(IEnumerable<InputOrder> inputOrders)
        {
            return inputOrders.Select(x => x.CarrierId).Distinct().Count() == 1;
        }

        public async Task<bool> Validate(IEnumerable<InputOrder> inputOrders)
        {
            var result = await IsSameDispatcherGroup(inputOrders);
            if (result)
                result = IsCarrierSame(inputOrders);
            return result;
        }
    }
}
