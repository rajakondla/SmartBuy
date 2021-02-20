using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SmartBuy.Administration.Infrastructure.Abstraction;
using SmartBuy.Common.Utilities.Repository;
using SmartBuy.Core.Modules;

namespace SmartBuy.Administration.Infrastructure
{
    public class Startup : IStartupService
    {
        public void ConfigureService(IServiceCollection services
            , IConfiguration configuration)
        {
            var connectionString = configuration["ConnectionStrings:SmartButDataContext"];
            //, o => o.MigrationsAssembly(this.GetType().Assembly.FullName)
            services.AddDbContext<AdministrationContext>(options => options.UseSqlServer(connectionString));

            services.TryAddScoped(typeof(IAdministrationRepository<>), 
                typeof(AdminstrationRepository<>));
        }
    }
}
