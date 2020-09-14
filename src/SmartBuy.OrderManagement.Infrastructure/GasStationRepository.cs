using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartBuy.OrderManagement.Domain;
using SmartBuy.OrderManagement.Infrastructure.Abstractions;
using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;
using SmartBuy.OrderManagement.Infrastructure.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

        public async Task<GasStationDetailDTO> GetGasStationDetailsAsync(Guid gasStationId)
        {
            if (gasStationId == default(Guid))
                throw new ArgumentException("Please pass valid gas station id");

            var tanks = await (from getGasStation in base.ReferenceContext.GasStations
                               join getTank in base.ReferenceContext.Tanks
                               on getGasStation.Id equals getTank.GasStationId
                               select new { getTank, FromTime = getGasStation.DeliveryTime.Start
                               , ToTime = getGasStation.DeliveryTime.End }).ToListAsync()
                               .ConfigureAwait(false);

            (TimeSpan StartTime, TimeSpan EndTime) duration = tanks.Select(
                x => (x.FromTime, x.ToTime)
                ).FirstOrDefault();

            var orderStrategy = await base.ReferenceContext.OrderStrategies
                .FirstOrDefaultAsync(x => x.GasStationId == gasStationId)
                .ConfigureAwait(false);

            return new GasStationDetailDTO
            {
                GasStationId = gasStationId,
                FromTime = duration.StartTime,
                ToTime = duration.EndTime,
                OrderType = orderStrategy.OrderType,
                TankDetails = tanks.Select(t => _mapper.Map<TankDetail>(t.getTank)).ToList()
            };
        }
    }
}
