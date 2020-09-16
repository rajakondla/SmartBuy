using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace SmartBuy.OrderManagement.Rules
{
    internal class CarrierRule
    {
        internal bool IsCarrierSame(IEnumerable<InputOrder> inputOrders)
        {
            return inputOrders.Select(x => x.CarrierId).Distinct().Count() == 1;
        }
    }
}
