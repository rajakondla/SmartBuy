using Microsoft.Extensions.Logging;
using System;

namespace SmartBuy.Web.Infrastructure
{
    public static class LogMessages
    {
        private static readonly Action<ILogger, string, string, long, Exception> _routePerformance;

        static LogMessages()
        {
            _routePerformance = LoggerMessage.Define<string, string, long>(LogLevel.Information
                , 0, "{RouteName} {MethodName} code took {ElapsedMilliseconds}.");
        }

        public static void LogRoutePerformance(this ILogger logger, string pageName
            , string methodName, long elapsedMilliSeconds)
        {
            _routePerformance(logger, pageName, methodName, elapsedMilliSeconds, null);
        }
    }
}
