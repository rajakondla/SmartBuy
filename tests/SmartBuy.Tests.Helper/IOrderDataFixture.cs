using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using System;
using System.Collections.Generic;
using SmartBuy.OrderManagement.Domain;

namespace SmartBuy.Tests.Helper
{
    public interface IOrderDataFixture
    {
        InputOrder InputOrder { get; }

        IEnumerable<GasStation> GasStations { get; }

        IEnumerable<GasStationSchedule> GasStationSchedules { get; }

        IEnumerable<GasStationTankSchedule> GasStationTankSchedules { get; }

        IEnumerable<GasStationScheduleByDay> GasStationSchedulesByDay { get; }

        IEnumerable<GasStationScheduleByTime> GasStationSchedulesByTime { get; }

        IEnumerable<OrderStrategy> OrderStrategies { get; }

        IEnumerable<Order> Orders { get; }

        IEnumerable<Carrier> Carriers { get; }

        void AddOrder(Order order);
    }
}
