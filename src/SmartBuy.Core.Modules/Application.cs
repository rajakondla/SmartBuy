using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SmartBuy.Core.Modules
{
    public class Application
    {
        public Application(AssemblyName name)
        {
            Name = name;
        }

        public AssemblyName Name { get; }

        public IServiceProvider InitializeServices(IConfiguration configuration)
        {
            var factory = new ShellContainerFactory(configuration);

            var assemblies = ScanAssembly("SmartBuy.*.dll");

            var serviceProvider = factory.CreateServiceContainer(assemblies);
            ServiceLocator.SetLocatorProvider(serviceProvider);
            return serviceProvider;
        }

        public static IEnumerable<Assembly> ScanAssembly(string searchPattern)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");

            return Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories).
                Select(Assembly.LoadFrom);
        }
    }
}
