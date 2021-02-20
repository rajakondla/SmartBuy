using Xunit;
using SmartBuy.Tests.Helper;
using SmartBuy.OrderManagement.Application.InputDTOs;
using System.Linq;
using System;
using SmartBuy.SharedKernel.Enums;
using SmartBuy.OrderManagement.Domain.Services;
using System.Threading.Tasks;
using Moq;
using SmartBuy.SharedKernel.ValueObjects;
using SmartBuy.OrderManagement.Domain;
using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SmartBuy.OrderManagement.Application.Tests
{
    public class OrderAppTests : BaseTest, IClassFixture<OrderDataFixture>
    {
        private readonly MockRepoHelper _mockHelper;
        private readonly OrderDataFixture _orderDateFixture;
        private readonly OrderApp _orderApp;
        private readonly OrderGenerator _orderGen;

        public OrderAppTests(OrderDataFixture orderDataFixture)
        {
            //var logger = new LoggerConfiguration().WriteTo
            //     .Sink(new TestCorrelatorSink()).CreateLogger();

            _mockHelper = new MockRepoHelper(orderDataFixture);
            _orderDateFixture = orderDataFixture;
            _orderGen = new OrderGenerator(_mockHelper.MockManagerOrderRepository.Object);
            _orderApp = new OrderApp(_mockHelper.MockManagerOrderRepository.Object
                , _mockHelper.MockGasStationsRepo.Object
                , _orderGen
                , base.LoggerFactory.CreateLogger<OrderApp>());
        }

        [Fact]
        public async Task ShoudCreateOrderWhenThereIsNoOverLappingDates()
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

            var result = await _orderApp.AddAsync(orderInputDTO);

            Assert.True(result.IsSuccess);
            Assert.True(result.OrderId > 0);
            Assert.Single(result.Message);
            Assert.True(result.Message.First() == OrderConstant.successMessage);
            _mockHelper.MockManagerOrderRepository.Verify(repo =>
            repo.GetOrderByGasStationIdDeliveryDateAsync(It.IsAny<Guid>(), It.IsAny<DateTimeRange>()),
              Times.Once);
            _mockHelper.MockManagerOrderRepository.Verify(repo =>
            repo.UpsertAsync(It.IsAny<ManageOrder>()), Times.Once);
        }


        [Fact]
        public async Task ShoudNotCreateOrderWhenThereIsOverLappingDates()
        {
            var gasStation = _orderDateFixture.GasStations.First();

            var orderInputDTO = new OrderInputDTO
            {
                CarrierId = _orderDateFixture.Carriers.First().Id,
                Comments = "Manual Order",
                OrderType = _orderDateFixture.OrderStrategies
                .First(x => x.GasStationId == gasStation.Id)
                .OrderType,
                FromDateTime = new DateTime(2020, 9, 9, 4, 0, 0),
                ToDateTime = new DateTime(2020, 9, 9, 8, 0, 0),
                GasStationId = gasStation.Id,
                LineItems = gasStation.Tanks.Select(
                    x => new OrderInputDTO.OrderProductInputDTO
                    {
                        TankId = x.Id,
                        Quantity = 500,
                        Unit = TankMeasurement.Gallons
                    })
            };

            var result = await _orderApp.AddAsync(orderInputDTO);

            Assert.False(result.IsSuccess);
            Assert.True(result.OrderId == default(int));
            Assert.Single(result.Message);
            Assert.True(result.Message.First() == OrderConstant.duplicateOrderMessage);
            _mockHelper.MockManagerOrderRepository.Verify(repo =>
            repo.GetOrderByGasStationIdDeliveryDateAsync(It.IsAny<Guid>(), It.IsAny<DateTimeRange>()),
              Times.Never);
            _mockHelper.MockManagerOrderRepository.Verify(repo =>
            repo.UpsertAsync(It.IsAny<ManageOrder>()), Times.Never);
        }
    }
}
