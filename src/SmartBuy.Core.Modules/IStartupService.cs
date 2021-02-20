using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SmartBuy.Core.Modules
{
    public interface IStartupService
    {
        void ConfigureService(IServiceCollection services
            , IConfiguration configuration);
    }
}
