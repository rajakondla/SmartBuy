using SmartBuy.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBuy.OrderManagement.Domain
{
    public class ManageOrder
    {
        public IEnumerable<Order> Orders;

        public ManageOrder(IEnumerable<Order> orders)
        {
            Orders = orders;
        }

        public IEnumerable<OutputDomainResult<Order>> ValidateOrders(
            IEnumerable<Order> oldOrders)
        {
            if (Orders == null)
                throw new NullReferenceException(nameof(Orders));

            if (oldOrders == null)
                throw new ArgumentNullException(nameof(oldOrders));

            var overlappingOrders = (
                from newOrd in oldOrders
                from oldOrd in Orders
                .Where(x => x.GasStationId == newOrd.GasStationId
                 && ((x.DispatchDate.Start <= newOrd.DispatchDate.Start && newOrd.DispatchDate.Start <= x.DispatchDate.End)
                 || (x.DispatchDate.Start <= newOrd.DispatchDate.End && newOrd.DispatchDate.End <= x.DispatchDate.End)))
                select newOrd
                ).ToList();

            var result = new List<OutputDomainResult<Order>>();
            foreach (var newOrder in Orders)
            {
                result.Add(new OutputDomainResult<Order>(
                    !overlappingOrders.Select(x => x.GasStationId).Contains(
                        newOrder.GasStationId),
                    entity: newOrder));
            }

            return result;
        }
    }
}

