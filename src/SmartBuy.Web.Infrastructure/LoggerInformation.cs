using System;
using System.Collections.Generic;
using System.Reflection;

namespace SmartBuy.Web.Infrastructure
{
    public class LoggerInformation : ILoggerInformation
    {
        public LoggerInformation()
        {
            Info = new Dictionary<string, string> {
                { "MachineName", Environment.MachineName},
                { "EntryPoint", Assembly.GetEntryAssembly().GetName().Name }
            };
        }

        public IDictionary<string, string> Info { get; }
    }
}
