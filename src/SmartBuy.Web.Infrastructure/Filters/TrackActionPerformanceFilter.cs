using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SmartBuy.Web.Infrastructure
{
    public class TrackActionPerformanceFilter : IActionFilter
    {
        private Stopwatch _timer;
        private readonly ILogger<TrackActionPerformanceFilter> _logger;
        private readonly ILoggerInformation _loggerInfo;
        private IDisposable _userScope;
        private IDisposable _hostScope;

        public TrackActionPerformanceFilter(ILogger<TrackActionPerformanceFilter> logger,
            ILoggerInformation loggerInfo)
        {
            _logger = logger;
            _loggerInfo = loggerInfo;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _timer = new Stopwatch();

            var userDict = new Dictionary<string, string>
            {
                { "UserId" , context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value },
                { "Scopes", string.Join(",", context.HttpContext.User.Claims.Where(x => x.Type == "scope")?
                .Select(x => x.Value)) }
            };
            userDict.Add("Email", MaskEmailAddress(context.HttpContext.User.FindFirst("email")?.Value));

            _userScope = _logger.BeginScope(userDict);
            _hostScope = _logger.BeginScope(_loggerInfo.Info);

            _timer.Start();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _timer.Stop();
            if (context.Exception == null)
            {
                _logger.LogRoutePerformance(context.HttpContext.Request.Path,
                    context.HttpContext.Request.Method,
                    _timer.ElapsedMilliseconds);
            }

            _userScope.Dispose();
            _hostScope.Dispose();
        }

        private string MaskEmailAddress(string emailAddress)
        {
            if (emailAddress != null)
            {
                var atIndex = emailAddress.IndexOf("@");
                if (atIndex > 1)
                {
                    return string.Format("{0}{1}******{2}", emailAddress[0], emailAddress[1],
                        emailAddress.Substring(atIndex));
                }
            }
            return emailAddress;
        }
    }
}
