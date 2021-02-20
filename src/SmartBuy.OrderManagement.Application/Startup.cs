using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartBuy.Core.Modules;
using SmartBuy.OrderManagement.Domain.Services;

namespace SmartBuy.OrderManagement.Application
{
    public class Startup : IStartupService
    {
        public void ConfigureService(IServiceCollection services, 
            IConfiguration configuration)
        {
            services.AddScoped<OrderGenerator>();
            services.AddScoped<OrderApp>();
        }
    }
}
