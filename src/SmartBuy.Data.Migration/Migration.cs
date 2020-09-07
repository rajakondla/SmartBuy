using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SmartBuy.Administration.Infrastructure;
using Microsoft.Extensions.Configuration;
using SmartBuy.OrderManagement.Infrastructure;

namespace SmartBuy.Data.Migration
{
    public static class Migration
    {
        public static void RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AdministrationContext>(options => options.UseSqlServer(configuration.GetConnectionString("SmartBuy")));
            services.AddDbContext<OrderContext>(options => options.UseSqlServer(configuration.GetConnectionString("SmartBuy")));
            services.AddDbContext<ReferenceContext>(options => options.UseSqlServer(configuration.GetConnectionString("SmartBuy")));
        }
    }
}

