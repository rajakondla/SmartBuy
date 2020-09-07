using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartBuy.OrderManagement.Domain.Services.Abstractions
{
    public interface IOrderGeneratorService
    {
        Task<IEnumerable<InputOrder>> RunOrderGeneratorAsync(IEnumerable<Guid> gasStationIds);
    }
}
