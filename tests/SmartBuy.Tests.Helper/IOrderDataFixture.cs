using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using System;
using System.Collections.Generic;
using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;
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

        IEnumerable<Order> GetOrders();

        IEnumerable<Carrier> Carriers { get; }
    }
}
