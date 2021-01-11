using SmartBuy.OrderManagement.Infrastructure.Mappers;
using Xunit;
using Moq;
using AutoMapper;
using SmartBuy.OrderManagement.Infrastructure.Tests.Helper;
using SmartBuy.OrderManagement.Domain;
using System.Linq;
using SmartBuy.SharedKernel.Enums;
using System.Threading.Tasks;

namespace SmartBuy.OrderManagement.Infrastructure.Tests
{
    public class ManageOrderRepositoryTests : IClassFixture<AdministrationDataFixture>,
        IClassFixture<OrderDataFixture>
    {
        private AdministrationDataFixture _adminstrationData;
        private GasStationRepository _gasStationRepository;
        private OrderContext _orderContext;
        private OrderDataFixture _orderData;

        public ManageOrderRepositoryTests(AdministrationDataFixture adminstrationData
            , OrderDataFixture orderData)
        {
            _adminstrationData = adminstrationData;
            _orderData = orderData;
            var mockMapper = new Mock<IMapper>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapping());
            });
            var mapper = config.CreateMapper();
            _gasStationRepository = new GasStationRepository(new ReferenceContext(), mapper);
            _orderContext = new OrderContext();
        }

        [Fact]
        public async Task ShouldCreateOrderWhenThereIsNoOverlappingDatesWithPreviousOrders()
        {
            var manageOrderRepo = new ManageOrderRepository(_orderContext);
            var order = _orderData.GetOrders().First();
            order.State = TrackingState.Added;
            var manageOrder = new ManageOrder();
            manageOrder.Add(order);

            await manageOrderRepo.UpsertAsync(manageOrder);

            var result = await manageOrderRepo.GetOrdersByGasStationIdAsync(order.GasStationId);

            Assert.NotEmpty(result.Orders);
            Assert.Equal(order.GasStationId, result.Orders.First().GasStationId);
        }

        [Fact]
        public async Task ShouldReturnOrderByGasStationIdAndDeliveryDate()
        {
            var manageOrderRepo = new ManageOrderRepository(_orderContext);
            var order = _orderData.GetOrders().First();
            var manageOrder = new ManageOrder();
            manageOrder.Add(order);

            await manageOrderRepo.UpsertAsync(manageOrder);
            var res = await manageOrderRepo.GetOrderByGasStationIdDeliveryDateAsync(order.GasStationId,
               order.DispatchDate);

            Assert.NotNull(res);
            Assert.True(res.Id > 0);
        }
    }
}
