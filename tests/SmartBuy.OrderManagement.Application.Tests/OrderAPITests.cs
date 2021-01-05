using Xunit;
using SmartBuy.Tests.Helper;
using SmartBuy.OrderManagement.Application.InputDTOs;
using System.Linq;
using System;
using SmartBuy.SharedKernel.Enums;

namespace SmartBuy.OrderManagement.Application.Tests
{
    public class OrderAPITests : IClassFixture<OrderDataFixture>
    {
        private MockRepoHelper _mockHelper;
        private OrderDataFixture _orderDateFixture;

        public OrderAPITests(OrderDataFixture orderDataFixture)
        {
            _mockHelper = new MockRepoHelper(orderDataFixture);
            _orderDateFixture = orderDataFixture;
        }

        [Fact]
        public void ShoudCreateOrderWhenThereIsNoOverLappingDates()
        {
            var gasStation = _orderDateFixture.GasStations.First();

            var orderInputDTO = new OrderInputDTO
            {
                CarrierId = _orderDateFixture.Carriers.First().Id,
                Comments = "Manual Order",
                OrderType = _orderDateFixture.OrderStrategies
                .First(x => x.GasStationId == gasStation.Id)
                .OrderType,
                FromDateTime = new DateTime(2020, 12, 1, 8, 0, 0),
                ToDateTime = new DateTime(2020, 12, 1, 12, 0, 0),
                GasStationId = gasStation.Id,
                LineItems = gasStation.Tanks.Select(
                    x => new OrderInputDTO.OrderProductInputDTO
                    {
                        TankId = x.Id,
                        Quantity = 500,
                        Unit = TankMeasurement.Gallons
                    })
            };

            //var createOrder = new OrderAPI(_mockHelper.MockManagerOrderRepository.Object
            //    , _mockHelper.MockGasStationsRepo.Object);

            //var result = createOrder.Add(orderInputDTO);

            //Assert.True(result.OrderId > 0);
        }
    }
}
