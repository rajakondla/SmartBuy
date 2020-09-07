using Repository;
using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using SmartBuy.OrderManagement.Infrastructure.Abstractions;
using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;
using System;
using System.Threading.Tasks;
using SmartBuy.SharedKernel.Enums;
using System.Linq;
using System.Collections.Generic;

namespace SmartBuy.OrderManagement.Domain.Services.ScheduleOrderGenerator
{
    public class ScheduleOrder
    {
        private readonly IGenericReadRepository<GasStationSchedule> _gasStationSchedule;
        private readonly IGenericReadRepository<GasStationScheduleByDay> _gasStationScheduleByDay;
        private readonly IGenericReadRepository<GasStationTankSchedule> _gasStationTankSchedule;
        private readonly IGenericReadRepository<GasStationScheduleByTime> _gasStationScheduleByTime;
        private readonly IDayComparable _dayCompare;
        private readonly ITimeIntervalComparable _timeIntervalCompare;

        public ScheduleOrder(IGenericReadRepository<GasStationSchedule> gasStationSchedule
            , IGenericReadRepository<GasStationScheduleByDay> gasStationScheduleByDay
            , IGenericReadRepository<GasStationTankSchedule> gasStationTankSchedule
            , IGenericReadRepository<GasStationScheduleByTime> gasStationScheduleByTime
            , IDayComparable dayCompare
            , ITimeIntervalComparable timeIntervalCompare)
        {
            _gasStationSchedule = gasStationSchedule;
            _gasStationScheduleByDay = gasStationScheduleByDay;
            _gasStationTankSchedule = gasStationTankSchedule;
            _gasStationScheduleByTime = gasStationScheduleByTime;
            _dayCompare = dayCompare;
            _timeIntervalCompare = timeIntervalCompare;
        }

        public async Task<InputOrder> CreateOrderAsync(GasStationDetailDTO gasStationDetail)
        {
            if (gasStationDetail == null)
                throw new ArgumentException("Invalid gas station details", nameof(gasStationDetail));

            if (gasStationDetail.OrderType == OrderType.Schedule)
            {
                var scheduleDetail = await _gasStationSchedule.FindByAsync(x =>
                 x.GasStationId == gasStationDetail.GasStationId).ConfigureAwait(false);

                if (scheduleDetail.FirstOrDefault().ScheduleType == ScheduleType.ByDay)
                {
                    return await CreateOrderByDay(gasStationDetail);
                }
                else
                {
                    return await CreateOrderByTimeInterval(gasStationDetail);
                }
            }

            return DefaultOrder.GetInstance.InputOrder;
        }

        private async Task<InputOrder> CreateOrderByDay(GasStationDetailDTO gasStationDetail)
        {
            var gsByDay = await _gasStationScheduleByDay.FindByAsync(x =>
            x.GasStationId == gasStationDetail.GasStationId).ConfigureAwait(false);

            var tankIds = gasStationDetail.TankDetails.Select(x => x.Id).ToList();
            var gsScheduleTanks = await _gasStationTankSchedule.FindByAsync(x =>
            tankIds.Contains(x.TankId)).ConfigureAwait(false);

            if (!gsByDay.Any())
                throw new DayConfugurationException(gasStationDetail.GasStationId.ToString());

            if (!gsScheduleTanks.Any())
                throw new TankConfugurationException(gasStationDetail.GasStationId.ToString());

            if (gsByDay.Any(x => _dayCompare.Compare(x.DayOfWeek)))
            {
                return new InputOrder
                {
                    GasStationId = gasStationDetail.GasStationId,
                    Comments = "Schedule Order created by system",
                    FromTime = DateTime.UtcNow.Date.AddMinutes(gasStationDetail.FromTime.TotalMinutes),
                    ToTime = DateTime.UtcNow.Date.AddMinutes(gasStationDetail.ToTime.TotalMinutes),
                    OrderType = OrderType.Schedule,
                    LineItems = CreateLineItem(gsScheduleTanks)
                };
            }

            return DefaultOrder.GetInstance.InputOrder;
        }

        private async Task<InputOrder> CreateOrderByTimeInterval(GasStationDetailDTO gasStationDetail)
        {
            var gsByTime = await _gasStationScheduleByTime.FindByAsync(x =>
            x.GasStationId == gasStationDetail.GasStationId).ConfigureAwait(false);

            var tankIds = gasStationDetail.TankDetails.Select(x => x.Id).ToList();
            var gsScheduleTanks = await _gasStationTankSchedule.FindByAsync(x =>
            tankIds.Contains(x.TankId)).ConfigureAwait(false);

            if (!gsScheduleTanks.Any())
                throw new TankConfugurationException(gasStationDetail.GasStationId.ToString());

            if (gsByTime.Any(x => _timeIntervalCompare.Compare(x.TimeInteral)))
            {
                return new InputOrder
                {
                    GasStationId = gasStationDetail.GasStationId,
                    Comments = "Schedule Order created by system",
                    FromTime = DateTime.UtcNow.Date.AddMinutes(gasStationDetail.FromTime.TotalMinutes),
                    ToTime = DateTime.UtcNow.Date.AddMinutes(gasStationDetail.ToTime.TotalMinutes),
                    OrderType = OrderType.Schedule,
                    LineItems = CreateLineItem(gsScheduleTanks)
                };
            }

            return DefaultOrder.GetInstance.InputOrder;
        }

        private List<InputOrderProduct> CreateLineItem(IEnumerable<GasStationTankSchedule> tanks)
        {
            return tanks.Select(x => new InputOrderProduct { Quantity = x.Quantity, TankId = x.TankId }).ToList();
        }

        public class DayConfugurationException : Exception
        {
            public DayConfugurationException(string gasStationId) :
                base($"Day confuguration is not found for gas station {gasStationId}")
            {

            }
        }

        public class TankConfugurationException : Exception
        {
            public TankConfugurationException(string gasStationId) :
                base($"Tank confuguration is not found for gas station {gasStationId}")
            {

            }
        }
    }
}