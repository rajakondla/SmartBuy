using SmartBuy.Common.Utilities.Repository;
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
        private readonly IManageOrderRepository _orderRepository;

        public ScheduleOrder(IGenericReadRepository<GasStationSchedule> gasStationSchedule
            , IGenericReadRepository<GasStationScheduleByDay> gasStationScheduleByDay
            , IGenericReadRepository<GasStationTankSchedule> gasStationTankSchedule
            , IGenericReadRepository<GasStationScheduleByTime> gasStationScheduleByTime
            , IDayComparable dayCompare
            , ITimeIntervalComparable timeIntervalCompare
            , IManageOrderRepository orderRepository)
        {
            _gasStationSchedule = gasStationSchedule;
            _gasStationScheduleByDay = gasStationScheduleByDay;
            _gasStationTankSchedule = gasStationTankSchedule;
            _gasStationScheduleByTime = gasStationScheduleByTime;
            _dayCompare = dayCompare;
            _timeIntervalCompare = timeIntervalCompare;
            _orderRepository = orderRepository;
        }

        public async Task<InputOrder> CreateOrderAsync(GasStationDetailDTO gasStationDetail)
        {
            if (gasStationDetail == null)
                throw new ArgumentException("gasStationDetail cannot be null", nameof(gasStationDetail));

            if (gasStationDetail.OrderType == OrderType.Schedule)
            {
                var scheduleDetail = await _gasStationSchedule.FindByAsync(x =>
                 x.GasStationId == gasStationDetail.GasStationId).ConfigureAwait(false);

                var tankIds = gasStationDetail.TankDetails.Select(x => x.Id).ToList();
                var gsScheduleTanks = await _gasStationTankSchedule.FindByAsync(x =>
                 tankIds.Contains(x.TankId)).ConfigureAwait(false);

                if (!gsScheduleTanks.Any())
                    throw new TankConfugurationException(gasStationDetail.GasStationId.ToString());

                if (scheduleDetail.FirstOrDefault().ScheduleType == ScheduleType.ByDay)
                {
                    return await CreateOrderByDay(gasStationDetail, gsScheduleTanks);
                }
                else
                {
                    return await CreateOrderByTimeInterval(gasStationDetail, gsScheduleTanks);
                }
            }

            return DefaultOrder.GetInstance.InputOrder;
        }

        private async Task<InputOrder> CreateOrderByDay(GasStationDetailDTO gasStationDetail, IEnumerable<GasStationTankSchedule> gsScheduleTanks)
        {
            var gsByDay = await _gasStationScheduleByDay.FindByAsync(x =>
            x.GasStationId == gasStationDetail.GasStationId).ConfigureAwait(false);

            if (!gsByDay.Any())
                throw new DayConfugurationException(gasStationDetail.GasStationId.ToString());

            if (gsByDay.Any(x => _dayCompare.Compare(x.DayOfWeek)))
            {
                return CreateInputOrder(gasStationDetail, gsScheduleTanks);
            }

            return DefaultOrder.GetInstance.InputOrder;
        }

        private async Task<InputOrder> CreateOrderByTimeInterval(GasStationDetailDTO gasStationDetail, IEnumerable<GasStationTankSchedule> gsScheduleTanks)
        {
            var gsByTime = await _gasStationScheduleByTime.FindByAsync(x =>
            x.GasStationId == gasStationDetail.GasStationId).ConfigureAwait(false);

            var order = (await _orderRepository.GetOrdersByGasStationIdAsync(gasStationDetail.GasStationId, OrderType.Schedule)).Orders.FirstOrDefault();

            var deliveryDate = order == null ? DateTime.MinValue : order.DispatchDate.Start;

            if (!gsByTime.Any())
                throw new TimIntervalConfigurationException(gasStationDetail.GasStationId.ToString());

            if (gsByTime.Any(x => _timeIntervalCompare.Compare(x.TimeInteral, deliveryDate, DateTime.Now)))
            {
                return CreateInputOrder(gasStationDetail, gsScheduleTanks);
            }

            return DefaultOrder.GetInstance.InputOrder;
        }

        private static InputOrder CreateInputOrder(GasStationDetailDTO gasStationDetail, IEnumerable<GasStationTankSchedule> gsScheduleTanks)
        {
            List<InputOrderProduct> CreateLineItem(IEnumerable<GasStationTankSchedule> tanks)
            {
                return tanks.Select(x => new InputOrderProduct { Quantity = x.Quantity, TankId = x.TankId }).ToList();
            }

            return new InputOrder
            {
                GasStationId = gasStationDetail.GasStationId,
                Comments = "Schedule Order created by system",
                FromTime = DateTime.Now.Date.AddMinutes(gasStationDetail.FromTime.TotalMinutes),
                ToTime = DateTime.Now.Date.AddMinutes(gasStationDetail.ToTime.TotalMinutes),
                OrderType = OrderType.Schedule,
                LineItems = CreateLineItem(gsScheduleTanks)
            };
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

        public class TimIntervalConfigurationException : Exception
        {
            public TimIntervalConfigurationException(string gasStationId) :
             base($"Time interval confuguration is not found for gas station {gasStationId}")
            {

            }
        }
    }
}