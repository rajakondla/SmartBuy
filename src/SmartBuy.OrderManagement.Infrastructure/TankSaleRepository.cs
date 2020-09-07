using AutoMapper;
using SmartBuy.OrderManagement.Infrastructure.Abstractions;
using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SmartBuy.OrderManagement.Domain;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SmartBuy.OrderManagement.Infrastructure
{
    public class TankSaleRepository : BaseReferenceContext, ITankSaleRepository
    {
        public TankSaleRepository(ReferenceContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TankSale>> GetSalesByGasStationAsync(Guid gasStationId, DateTime fromTime, DateTime toTime)
        {
            if (gasStationId == default(Guid))
                throw new ArgumentException("Please pass valid gas station id");

            return
                  await (from getTank in base.ReferenceContext.Tanks
                         join getSales in base.ReferenceContext.TankSales
                         on getTank.Id equals getSales.TankId
                         where getTank.GasStationId == gasStationId
                         && (getSales.SaleTime >= fromTime && getSales.SaleTime <= toTime)
                         select getSales
                ).ToListAsync();
        }

        public async Task<IEnumerable<TankSale>> GetSalesByTankAsync(int tankId, DateTime fromTime, DateTime toTime)
        {
            if (tankId == 0)
                throw new ArgumentException("Please pass valid tank id");

            return await base.ReferenceContext.TankSales
                            .Where(t =>
                              fromTime <= t.SaleTime && t.SaleTime <= toTime
                            ).ToListAsync();
        }
    }
}
