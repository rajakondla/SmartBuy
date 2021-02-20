using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SmartBuy.Core.Modules
{
    public class ShellContainerFactory
    {
        private readonly IConfiguration _configuration;

        public ShellContainerFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IServiceProvider CreateServiceContainer(IEnumerable<Assembly> assemblies)
        {
            IServiceCollection moduleServices = new ServiceCollection();

            moduleServices.AddSingleton(_configuration);

            var loadedAssemblies = assemblies.ToList();

            foreach (var implementation in loadedAssemblies
                .SelectMany(x => x.GetTypes())
                .Where(t =>
                    !t.IsAbstract
                    && !t.IsInterface
                    && (typeof(IStartupService).IsAssignableFrom(t))))
            {
                moduleServices.AddSingleton(implementation);
            }

            return moduleServices.BuildServiceProvider();
        }
    }
}
