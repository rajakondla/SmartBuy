using System.Collections.Generic;

namespace SmartBuy.Web.Infrastructure
{
    public interface ILoggerInformation
    {
        IDictionary<string, string> Info { get; }
    }
}
