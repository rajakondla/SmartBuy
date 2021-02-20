using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SmartBuy.Common.Utilities.Repository;
using SmartBuy.Core.Modules;
using SmartBuy.OrderManagement.Infrastructure.Abstractions;
using SmartBuy.OrderManagement.Infrastructure.Mappers;

namespace SmartBuy.OrderManagement.Infrastructure
{
    public class Startup : IStartupService
    {
        public void ConfigureService(IServiceCollection services,
            IConfiguration configuration)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapping());
            });

            IMapper mapper = mapperConfig.CreateMapper();

            services.AddSingleton(mapper);

            var connectionString = configuration["ConnectionStrings:SmartButDataContext"];
            //, o => o.MigrationsAssembly(this.GetType().Assembly.FullName)
            services.AddDbContext<OrderContext>(options => options.UseSqlServer(connectionString));
            services.AddDbContext<ReferenceContext>(options => options.UseSqlServer(connectionString));

            services.TryAddScoped(typeof(IReferenceRepository<>), typeof(ReferenceRepository<>));
            services.TryAddScoped<IManageOrderRepository, ManageOrderRepository>();
            services.TryAddScoped<IGasStationRepository, GasStationRepository>();
            services.TryAddScoped<ITankSaleRepository, TankSaleRepository>();
        }
    }
}
