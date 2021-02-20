using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SmartBuy.Tests.Helper
{
    public class BaseTest
    {
        private readonly ILoggerFactory _loggerFactory;
        public BaseTest()
        {
            var serviceProvider = new ServiceCollection()
              .AddLogging()
              .BuildServiceProvider();

            _loggerFactory = serviceProvider.GetService<ILoggerFactory>();
        }

        public ILoggerFactory LoggerFactory => _loggerFactory;
    }
}
