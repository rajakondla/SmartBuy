using SmartBuy.OrderManagement.Infrastructure.Mappers;
using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;
using Xunit;
using System;
using System.Linq;
using Moq;
using AutoMapper;
using System.Threading.Tasks;
using SmartBuy.OrderManagement.Infrastructure.Tests.Helper;

namespace SmartBuy.OrderManagement.Infrastructure.Tests
{
    public class GasStationRepositoryTests : IClassFixture<AdministrationDataFixture>
    {
        private AdministrationDataFixture _adminstrationData;
        private GasStationRepository _gasStationRepository;

        public GasStationRepositoryTests(AdministrationDataFixture adminstrationData)
        {
            _adminstrationData = adminstrationData;
            Setup();
        }

        private void Setup()
        {
            var mockMapper = new Mock<IMapper>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapping());
            });
            var mapper = config.CreateMapper();
            var refContext = new ReferenceContext();
            _gasStationRepository = new GasStationRepository(refContext, mapper);
        }

        [Fact]
        public void GasStationDetailsTest()
        {
            var gasStationDetailModel = new GasStationDetailDTO();

            Assert.NotNull(gasStationDetailModel.TankDetails);
        }

        [Fact]
        public void ShouldThrowArugumentExceptionError()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => { 
                await _gasStationRepository.GetGasStationIncludeTankOrderStrategyAsync(default(Guid)); 
            });
        }

        [Fact]
        public async Task GetGasStationDetailsAsync()
        {
            var result = await _gasStationRepository
                  .GetGasStationIncludeTankOrderStrategyAsync(_adminstrationData.GasStation.Id)
                  .ConfigureAwait(false);

            var tank1 = _adminstrationData.Tanks.FirstOrDefault();
            var tank2 = _adminstrationData.Tanks.LastOrDefault();

            Assert.Equal(_adminstrationData.GasStation.Id, result.GasStation.Id);
            Assert.Contains(tank1.Id, result.GasStation.Tanks.Select(t => t.Id));
            Assert.Contains(tank2.Id, result.GasStation.Tanks.Select(t => t.Id));
            Assert.Single(result.GasStation.Tanks.Where(t =>
            t.Id == tank1.Id));
            Assert.Equal(tank1.Measurement.Quantity, result.GasStation.Tanks.Where(t =>
            t.Id == tank1.Id).FirstOrDefault().Measurement.Quantity);
            Assert.Equal(tank1.Measurement.Top, result.GasStation.Tanks.Where(t =>
            t.Id == tank1.Id).FirstOrDefault().Measurement.Top);
            Assert.Equal(tank1.Measurement.Bottom, result.GasStation.Tanks.Where(t =>
            t.Id == tank1.Id).FirstOrDefault().Measurement.Bottom);
        }
    }
}
