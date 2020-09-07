using Microsoft.EntityFrameworkCore;
using SmartBuy.Administration.Infrastructure;
using SmartBuy.OrderManagement.Infrastructure.Mappers;
using SmartBuy.OrderManagement.Domain;
using SmartBuy.OrderManagement.Infrastructure.Abstractions.DTOs;
using Xunit;
using System;
using SmartBuy.SharedKernel.ValueObjects;
using System.Linq;
using Castle.Core.Internal;
using SmartBuy.OrderManagement.Infrastructure.Abstractions;
using Moq;
using AutoMapper;
using System.Threading.Tasks;
using SmartBuy.SharedKernel.Enums;
using SmartBuy.OrderManagement.Infrastructure.Tests.Helper;

namespace SmartBuy.OrderManagement.Infrastructure.Tests
{
    public class GasStationRepositoryTests : IClassFixture<AdministrationDataFixture>
    {
        private DbContextOptionsBuilder _builder;
        private AdministrationDataFixture _adminstrationData;
        private GasStationRepository _gasStationRepository;

        public GasStationRepositoryTests(AdministrationDataFixture adminstrationData)
        {
            _builder = new DbContextOptionsBuilder();
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
            Assert.ThrowsAsync<ArgumentException>(async () => { await _gasStationRepository.GetGasStationDetailsAsync(default(Guid)); });
        }

        [Fact]
        public async Task GetGasStationDetailsAsync()
        {
            var gasStationDetail = await _gasStationRepository.GetGasStationDetailsAsync(_adminstrationData.GasStation.Id)
                                  .ConfigureAwait(false);

            Assert.NotNull(gasStationDetail);
            Assert.Equal(_adminstrationData.GasStation.Id, gasStationDetail.GasStationId);
            Assert.Contains(_adminstrationData.Tanks.FirstOrDefault().Id, gasStationDetail.TankDetails.Select(t => t.Id));
            Assert.Contains(_adminstrationData.Tanks.LastOrDefault().Id, gasStationDetail.TankDetails.Select(t => t.Id));
            Assert.Single(gasStationDetail.TankDetails.Where(t =>
            t.Id == _adminstrationData.Tanks.FirstOrDefault().Id));
            Assert.Equal(_adminstrationData.Tanks.FirstOrDefault().Measurement.Quantity, gasStationDetail.TankDetails.Where(t =>
            t.Id == _adminstrationData.Tanks.FirstOrDefault().Id).FirstOrDefault().Quantity);
            Assert.Equal(_adminstrationData.Tanks.FirstOrDefault().Measurement.Top, gasStationDetail.TankDetails.Where(t =>
            t.Id == _adminstrationData.Tanks.FirstOrDefault().Id).FirstOrDefault().Top);
            Assert.Equal(_adminstrationData.Tanks.FirstOrDefault().Measurement.Bottom, gasStationDetail.TankDetails.Where(t =>
            t.Id == _adminstrationData.Tanks.FirstOrDefault().Id).FirstOrDefault().Bottom);
        }
    }
}
