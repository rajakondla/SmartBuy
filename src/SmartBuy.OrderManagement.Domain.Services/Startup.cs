using Microsoft.Extensions.DependencyInjection;
using SmartBuy.OrderManagement.Domain.Services.Abstractions;
using SmartBuy.OrderManagement.Domain.Services.ScheduleOrderGenerator;

namespace SmartBuy.OrderManagement.Domain.Services
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDayComparable, DayComparer>();
            services.AddScoped<ITimeIntervalComparable, TimeComparer>();
        }
    }
}
