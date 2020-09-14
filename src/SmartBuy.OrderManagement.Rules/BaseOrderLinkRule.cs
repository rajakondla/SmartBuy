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

        protected virtual async Task<bool> IsSameDispatcherGroup(IEnumerable<InputOrder> inputOrders)
        {
            var distinctGasStationIds = inputOrders.Select(x => x.GasStationId).Distinct();

            var gasStations = await _gasStationRepo.FindByAsync(x => distinctGasStationIds.Contains(x.Id));

            return gasStations.Select(x => x.DispatcherGroupId).Distinct().Count() == 1;
        }

        public async Task<bool> Validate(IEnumerable<InputOrder> inputOrders)
        {
            return await IsSameDispatcherGroup(inputOrders);
        }
    }
}
