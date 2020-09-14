using SmartBuy.Administration.Domain;
using SmartBuy.OrderManagement.Domain;
using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using SmartBuy.SharedKernel.Enums;
using SmartBuy.SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartBuy.OrderManagement.Rules.Tests.Helper
{
    public class OrderDataFixture
    {
        public readonly IEnumerable<InputOrder> InputOrders;

        public readonly IEnumerable<GasStation> GasStations;

        public readonly IEnumerable<Carrier> Carriers;

        public readonly IEnumerable<GasStationTerminalPreference> GasStationsTerminalPreferences;

        public readonly IEnumerable<GasStationCarrierPreference> GasStationsCarrierPreferences;

        public readonly IEnumerable<CarrierFreight> CarriersFreights;

        public OrderDataFixture()
        {
            var dispatcherGroupId1 = Guid.NewGuid();
            var dispatcherGroupId2 = Guid.NewGuid();

            var terminal1 = Guid.NewGuid();
            var terminal2 = Guid.NewGuid();
            var terminal3 = Guid.NewGuid();

            var gasStation1Data = Guid.NewGuid();
            var gasStation2Data = Guid.NewGuid();
            var gasStation3Data = Guid.NewGuid();

            var carrier1 = Guid.NewGuid();
            var carrier2 = Guid.NewGuid();
            var carrier3 = Guid.NewGuid();

            GasStations = new List<GasStation>{ new GasStation(gasStation1Data, Enumerable.Empty<Tank>(),
                new TimeRange(new TimeSpan(20, 0, 0), new TimeSpan(4, 0, 0)),
                dispatcherGroupId1),
            new GasStation(gasStation2Data, Enumerable.Empty<Tank>(),
                new TimeRange(new TimeSpan(20, 0, 0), new TimeSpan(4, 0, 0)),
                dispatcherGroupId2),
            new GasStation(gasStation3Data, Enumerable.Empty<Tank>(),
                new TimeRange(new TimeSpan(20, 0, 0), new TimeSpan(4, 0, 0)),
                dispatcherGroupId2)};

            Carriers = new List<Carrier> {
                new Carrier(carrier1, 5000, new TimeRange(new TimeSpan(20, 0, 0),
                  new TimeSpan(4, 0, 0))),
                new Carrier(carrier2, 3000, new TimeRange(new TimeSpan(20, 0, 0),
                  new TimeSpan(4, 0, 0))),
                new Carrier(carrier3, 6000, new TimeRange(new TimeSpan(20, 0, 0),
                  new TimeSpan(4, 0, 0)))};

            GasStationsTerminalPreferences = new List<GasStationTerminalPreference>{
                new GasStationTerminalPreference(gasStation1Data,
                terminal1, true),
                new GasStationTerminalPreference(gasStation1Data,
                terminal2, false),
                new GasStationTerminalPreference(gasStation2Data,
               terminal1, true),
                new GasStationTerminalPreference(gasStation2Data,
                terminal3, false),
                new GasStationTerminalPreference(gasStation3Data,
               terminal2, true),
                new GasStationTerminalPreference(gasStation3Data,
                terminal3, false)
            };

            GasStationsCarrierPreferences = new List<GasStationCarrierPreference> {
               new GasStationCarrierPreference(gasStation1Data,
               carrier1, true),
               new GasStationCarrierPreference(gasStation1Data,
               carrier2, false),
               new GasStationCarrierPreference(gasStation2Data,
               carrier1, true),
               new GasStationCarrierPreference(gasStation2Data,
               carrier3, false),
               new GasStationCarrierPreference(gasStation3Data,
               carrier2, true),
               new GasStationCarrierPreference(gasStation3Data,
               carrier3, false)
            };

            CarriersFreights = new List<CarrierFreight>
            {
                new CarrierFreight(gasStation1Data, terminal1, carrier1
                , new Money(10, CurrencyUnit.Cents)),
                 new CarrierFreight(gasStation1Data, terminal2, carrier1
                , new Money(9, CurrencyUnit.Cents)),
                 new CarrierFreight(gasStation1Data, terminal3, carrier1
                , new Money(8, CurrencyUnit.Cents)),
                  new CarrierFreight(gasStation1Data, terminal1, carrier2
                , new Money(10, CurrencyUnit.Cents)),
                 new CarrierFreight(gasStation1Data, terminal2, carrier3
                , new Money(9, CurrencyUnit.Cents)),

                 new CarrierFreight(gasStation2Data, terminal1, carrier1
                , new Money(10, CurrencyUnit.Cents)),
                 new CarrierFreight(gasStation2Data, terminal2, carrier1
                , new Money(9, CurrencyUnit.Cents)),
                 new CarrierFreight(gasStation2Data, terminal3, carrier1
                , new Money(8, CurrencyUnit.Cents)),
                  new CarrierFreight(gasStation2Data, terminal1, carrier2
                , new Money(10, CurrencyUnit.Cents)),
                 new CarrierFreight(gasStation2Data, terminal2, carrier3
                , new Money(9, CurrencyUnit.Cents)),

                new CarrierFreight(gasStation3Data, terminal1, carrier1
                , new Money(10, CurrencyUnit.Cents)),
                 new CarrierFreight(gasStation3Data, terminal2, carrier1
                , new Money(9, CurrencyUnit.Cents)),
                 new CarrierFreight(gasStation3Data, terminal3, carrier1
                , new Money(8, CurrencyUnit.Cents)),
                  new CarrierFreight(gasStation3Data, terminal1, carrier2
                , new Money(10, CurrencyUnit.Cents)),
                 new CarrierFreight(gasStation3Data, terminal2, carrier3
                , new Money(9, CurrencyUnit.Cents))
            };

            InputOrders = new List<InputOrder> {
                new InputOrder
            {
                GasStationId = gasStation1Data,
                FromTime = new DateTime(2020, 9, 14, 20, 0, 0),
                ToTime = new DateTime(2020, 9, 15, 4, 0, 0),
                OrderType = OrderType.Schedule,
                LineItems = new List<InputOrderProduct> {
                    new InputOrderProduct{
                        Quantity = 1000,
                        TankId = 1
                    },
                    new InputOrderProduct{ Quantity = 1000,
                        TankId = 2},
                    new InputOrderProduct
                    {
                        Quantity = 1000,
                        TankId = 3
                    }}
            },
                new InputOrder
                {
                    GasStationId = gasStation2Data,
                    FromTime = new DateTime(2020, 9, 14, 20, 0, 0),
                    ToTime = new DateTime(2020, 9, 15, 4, 0, 0),
                    OrderType = OrderType.Schedule,
                    LineItems = new List<InputOrderProduct> {
                    new InputOrderProduct{
                        Quantity = 1000,
                        TankId = 4
                    },
                    new InputOrderProduct{ Quantity = 1000,
                        TankId = 5},
                    new InputOrderProduct{ Quantity = 1000,
                        TankId = 6}}
                }, new InputOrder
                {
                    GasStationId = gasStation3Data,
                    FromTime = new DateTime(2020, 9, 14, 20, 0, 0),
                    ToTime = new DateTime(2020, 9, 15, 4, 0, 0),
                    OrderType = OrderType.Schedule,
                    LineItems = new List<InputOrderProduct> {
                    new InputOrderProduct{
                        Quantity = 1000,
                        TankId = 7
                    },
                    new InputOrderProduct{ Quantity = 1000,
                        TankId = 8},
                    new InputOrderProduct{ Quantity = 1000,
                        TankId = 9}}
                } };
        }
    }
}
