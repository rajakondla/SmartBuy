using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartBuy.OrderManagement.Domain;
using SmartBuy.OrderManagement.Infrastructure.Abstractions;
using SmartBuy.SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBuy.OrderManagement.Infrastructure
{
    public class GasStationRepository : BaseReferenceContext, IGasStationRepository
    {
        private readonly IMapper _mapper;

        public GasStationRepository(ReferenceContext context,
            IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<(GasStation GasStation, OrderType OrderType)> GetGasStationIncludeTankOrderStrategyAsync(Guid gasStationId)
        {
            if (gasStationId == default(Guid))
                throw new ArgumentException("Please pass valid gas station id");

            var gasStationDetail = await (from getGasStation in base.ReferenceContext.GasStations.Include(x=>x.Tanks)
                               join getOrderStrategy in base.ReferenceContext.OrderStrategies
                               on getGasStation.Id equals getOrderStrategy.GasStationId
                               select new
                               {
                                   getGasStation,
                                   getOrderStrategy.OrderType
                               }).FirstOrDefaultAsync()
                               .ConfigureAwait(false);

            return (gasStationDetail.getGasStation, gasStationDetail.OrderType);
        }
    }
}
