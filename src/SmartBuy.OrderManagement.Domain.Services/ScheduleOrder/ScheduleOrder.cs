using SmartBuy.Common.Utilities.Repository;
using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using SmartBuy.OrderManagement.Infrastructure.Abstractions;
using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;
using System;
using System.Threading.Tasks;
using SmartBuy.SharedKernel.Enums;
using System.Linq;
using System.Collections.Generic;
using SmartBuy.SharedKernel;

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

        public async Task<OutputDomainResult<Order>> GetAsync(
            (GasStation GasStation, OrderType OrderType) gasStationDetail)
        {
            if (gasStationDetail.OrderType == OrderType.Schedule)
            {
                var scheduleDetail = await _gasStationSchedule.FindByAsync(x =>
                 x.GasStationId == gasStationDetail.GasStation.Id).ConfigureAwait(false);

                var tankIds = gasStationDetail.GasStation.Tanks.Select(x => x.Id).ToList();
                var gsScheduleTanks = await _gasStationTankSchedule.FindByAsync(x =>
                 tankIds.Contains(x.TankId)).ConfigureAwait(false);

                if (!gsScheduleTanks.Any())
                    throw new TankConfugurationException(gasStationDetail.GasStation.Id.ToString());

                if (scheduleDetail.FirstOrDefault().ScheduleType == ScheduleType.ByDay)
                {
                    return await CreateOrderByDay(gasStationDetail.GasStation, gsScheduleTanks);
                }
                else
                {
                    return await CreateOrderByTimeInterval(gasStationDetail.GasStation, gsScheduleTanks);
                }
            }

            return DefaultOutputDomainResult.GetInstance.Order;
        }

        private async Task<OutputDomainResult<Order>> CreateOrderByDay(GasStation gasStation, IEnumerable<GasStationTankSchedule> gsScheduleTanks)
        {
            var gsByDay = await _gasStationScheduleByDay.FindByAsync(x =>
            x.GasStationId == gasStation.Id).ConfigureAwait(false);

            if (!gsByDay.Any())
                throw new DayConfugurationException(gasStation.Id.ToString());

            if (gsByDay.Any(x => _dayCompare.Compare(x.DayOfWeek)))
            {
                return CreateOrder(gasStation, gsScheduleTanks);
            }

            return DefaultOutputDomainResult.GetInstance.Order;
        }

        private async Task<OutputDomainResult<Order>> CreateOrderByTimeInterval(GasStation gasStation,
            IEnumerable<GasStationTankSchedule> gsScheduleTanks)
        {
            var gsByTime = await _gasStationScheduleByTime.FindByAsync(x =>
            x.GasStationId == gasStation.Id).ConfigureAwait(false);

            var order = (await _orderRepository.GetOrdersByGasStationIdAsync(gasStation.Id
                , OrderType.Schedule)).Orders.FirstOrDefault();

            var deliveryDate = order == null ? DateTime.MinValue : order.DispatchDate.Start;

            if (!gsByTime.Any())
                throw new TimIntervalConfigurationException(gasStation.Id.ToString());

            if (gsByTime.Any(x => _timeIntervalCompare.Compare(x.TimeInteral, deliveryDate, DateTime.Now)))
            {
                return CreateOrder(gasStation, gsScheduleTanks);
            }

            return DefaultOutputDomainResult.GetInstance.Order;
        }

        private static OutputDomainResult<Order> CreateOrder(GasStation gasStation
            , IEnumerable<GasStationTankSchedule> gsScheduleTanks)
        {
            List<InputOrderProduct> CreateLineItem(IEnumerable<GasStationTankSchedule> tanks)
            {
                return tanks.Select(x =>
                new InputOrderProduct
                {
                    Quantity = x.Quantity,
                    TankId = x.TankId
                }).ToList();
            }

            var inputOrder = new InputOrder
            {
                GasStationId = gasStation.Id,
                Comments = "Schedule Order created by system",
                FromTime = DateTime.UtcNow.Date.AddMinutes(gasStation.DeliveryTime.Start.Minutes),
                ToTime = DateTime.Now.Date.AddMinutes(gasStation.DeliveryTime.End.Minutes),
                OrderType = OrderType.Schedule,
                LineItems = CreateLineItem(gsScheduleTanks)
            };

            return Order.Create(inputOrder, gasStation);
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