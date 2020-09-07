using SmartBuy.SharedDatabase;
using SmartBuy.SharedDatabase.Enums;
using SmartBuy.SharedDatabase.Model.ValueObjects;
using System;

namespace SmartBuy.Database
{
    public class Order
    {
        public Order(GasStation gasStation)
        {
            GasStation = gasStation;
            GasStationId = gasStation.Id;
        }

        public int Id { get; set; }

        public Guid GasStationId { get; set; }

        public GasStation GasStation { get; set; }

        public OrderType OrderType { get; set; }

        public OrderStartEnd TimeRange { get; set; }

    }
}
