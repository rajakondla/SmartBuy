using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using System.IO;

namespace SmartBuy.Core.Modules
{
    public static class ServiceLoader
    {
        public static void Load(IServiceCollection services
            , IConfiguration configuration)
        {
            //var type = typeof(IStartupService);
            //var types = Assembly.GetExecutingAssembly().GetTypes()
            //    .Where(x => x.GetInterface("IStartupService") == type);

            var path = AppDomain.CurrentDomain.BaseDirectory;
            var loadedAssemblies = Directory.GetFiles(path,
                "SmartBuy.*.dll",
                SearchOption.AllDirectories)
                .Select(Assembly.LoadFrom)
                .SelectMany(x => x.GetTypes())
                .Where(t =>
                     !t.IsAbstract
                     && !t.IsInterface
                     && (typeof(IStartupService).IsAssignableFrom(t)));

            foreach (var implementation in loadedAssemblies)
            {
                //  services.AddSingleton(typeof(IStartupService), implementation);

                ((IStartupService)Activator.CreateInstance(implementation))
                    .ConfigureService(services, configuration);
            }

            // services.BuildServiceProvider();
        }
    }
}

