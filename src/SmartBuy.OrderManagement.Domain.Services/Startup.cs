using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SmartBuy.Core.Modules;
using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using SmartBuy.OrderManagement.Domain.Services.EstimateOrderGenerator;
using SmartBuy.OrderManagement.Domain.Services.ScheduleOrderGenerator;
using SmartBuy.OrderManagement.Infrastructure;
using SmartBuy.OrderManagement.Infrastructure.Abstractions;

namespace SmartBuy.OrderManagement.Domain.Services
{
    public class Startup : IStartupService
    {
        public void ConfigureService(IServiceCollection services
            , IConfiguration configuration)
        {
            services.TryAddScoped<IDayComparable, DayComparer>();
            services.TryAddScoped<ITimeIntervalComparable, TimeComparer>();
            services.TryAddScoped(typeof(IReferenceRepository<>), typeof(ReferenceRepository<>));
            services.AddScoped<ScheduleOrder>();
            services.AddScoped<EstimateOrder>();
            services.AddScoped<OrderGenerator>();
            services.AddScoped<AutoOrderGenerator>();
        }
    }
}
