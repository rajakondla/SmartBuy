using SmartBuy.Common.Utilities.Repository;
using SmartBuy.OrderManagement.Domain;
using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBuy.OrderManagement.Rules
{
    internal class DispatcherGroupRule
    {
        private readonly IGenericReadRepository<GasStation> _gasStationRepo;
        internal DispatcherGroupRule(IGenericReadRepository<GasStation> gasStationRepo)
        {
            _gasStationRepo = gasStationRepo;
        }

        internal async Task<bool> IsDispatcherSame(IEnumerable<InputOrder> inputOrders)
        {
            if (inputOrders == null)
                throw new ArgumentException("input order is null", nameof(inputOrders));

            var distinctGasStationIds = inputOrders.Select(x => x.GasStationId).Distinct();

            var gasStations = await _gasStationRepo.FindByAsync(x => distinctGasStationIds.Contains(x.Id));

            return gasStations.Select(x => x.DispatcherGroupId).Distinct().Count() == 1;
        }
    }
}
