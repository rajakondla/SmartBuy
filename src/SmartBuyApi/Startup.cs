using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmartBuy.Core.Modules;
using SmartBuy.Web.Infrastructure;
using System;
using System.Net.Mime;
using SmartBuyApi.Model;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Linq;
//using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace SmartBuyApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddScoped<ILoggerInformation, LoggerInformation>();
            ServiceLoader.Load(services, Configuration);
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(TrackActionPerformanceFilter));
            });

            services.AddHealthChecks()
                     .AddSqlServer(
#pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
                            connectionString: Configuration["ConnectionStrings:SmartButDataContext"],
#pragma warning restore ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
                            healthQuery: "SELECT 1;",
                            name: "sql",
                            failureStatus: HealthStatus.Unhealthy,
                            tags: new string[] { "ready" })
                     .AddUrlGroup(new Uri($"{ Configuration["DownStreamUrl:Url"] }/WeatherForecast"),
                                          "down stream api check",
                                           HealthStatus.Degraded,
                                           timeout: new TimeSpan(0, 0, 5),
                                           tags: new string[] { "ready" });

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options => {
                    options.Authority = "http://localhost:57748";
                    options.RequireHttpsMetadata = false;

                    options.Audience = "SmartBuyAPI";
                });

            //services.AddHealthChecksUI(s =>
            //{
            //    s.AddHealthCheckEndpoint("UIendpoint", "https://localhost:4020/healthui");
            //});

            //services.Scan(scan => scan.FromApplicationDependencies()
            //    .AddClasses(c => c.AssignableTo<IStartupService>())
            //    .AsImplementedInterfaces()
            //    .WithSingletonLifetime());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseApiExceptionHandler(options =>
                       {
                           options.AddResponseDetails = UpdateApiErrorResponse;
                           options.DetermineLogLevel = DetermineLogLevel;
                       });

            app.UseRouting();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    context.Response.StatusCode = 200;
                    context.Response.ContentType = MediaTypeNames.Application.Json;

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new MessageModel()));
                });

                endpoints.MapGet("/_status", async context =>
                {
                    context.Response.StatusCode = 200;
                    context.Response.ContentType = MediaTypeNames.Application.Json;

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new StatusModel()));
                });

                endpoints.MapControllers();

                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions
                {
                    ResultStatusCodes = {
                    [HealthStatus.Healthy] = StatusCodes.Status200OK,
                    [HealthStatus.Unhealthy] = StatusCodes.Status500InternalServerError,
                    [HealthStatus.Degraded] = StatusCodes.Status503ServiceUnavailable
                },
                    ResponseWriter = WriteHealthCheckReadyResponse,
                    Predicate = (check) => check.Tags.Contains("ready"),
                    AllowCachingResponses = false
                });

                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
                {
                    Predicate = (check) => !check.Tags.Contains("ready"),
                    ResponseWriter = WriteHealthCheckLiveResponse,
                    AllowCachingResponses = false
                });

                //endpoints.MapHealthChecksUI();

                //endpoints.MapHealthChecks("/healthui", new HealthCheckOptions { 
                //    Predicate = _ => true,
                //    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                //});
            });
        }

        private Task WriteHealthCheckLiveResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";
            var json = new JObject {
                new JProperty("OverallStatus", result.Status.ToString()),
                new JProperty("TotalChecksDuration",
                           result.TotalDuration.TotalSeconds.ToString("0:0.00"))
            };

            return httpContext.Response.WriteAsync(json.ToString(Formatting.Indented));
        }

        private Task WriteHealthCheckReadyResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";
            var json = new JObject {
                new JProperty("OverallStatus", result.Status.ToString()),
                new JProperty("TotalChecksDuration",
                           result.TotalDuration.TotalSeconds.ToString("0:0.00")),
                new JProperty("DependencyHealthChecks", new JObject(
                    result.Entries.Select(item =>
                    new JProperty(item.Key, new JObject(
                        new JProperty("Status", item.Value.Status.ToString()),
                        new JProperty("Duration",
                                  item.Value.Duration.TotalSeconds.ToString("0:0.00"))
                        ))
                       )
                    ))
            };

            return httpContext.Response.WriteAsync(json.ToString(Formatting.Indented));
        }

        private LogLevel DetermineLogLevel(Exception ex)
        {
            if (ex.Message.StartsWith("cannot open database", StringComparison.InvariantCultureIgnoreCase) ||
               ex.Message.StartsWith("a network-related", StringComparison.InvariantCultureIgnoreCase))
            {
                return LogLevel.Critical;
            }

            return LogLevel.Error;
        }

        private void UpdateApiErrorResponse(HttpContext context, Exception ex,
            ApiError error)
        {
            if (ex.GetType().Name == typeof(SqlException).Name)
            {
                error.Detail = "Exception was a database exception";
            }
        }
    }
}
