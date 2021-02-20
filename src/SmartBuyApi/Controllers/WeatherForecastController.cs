using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartBuy.OrderManagement.Application;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SmartBuyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly OrderApp _orderApp;

        public WeatherForecastController(OrderApp orderApp,
            ILogger<WeatherForecastController> logger)
        {
            _orderApp = orderApp;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WeatherForecast>))]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> Get()
        {
            _logger.LogInformation("Hello from controller");

            var result = await _orderApp.AddAsync(
                 new SmartBuy.OrderManagement.Application.InputDTOs.OrderInputDTO
                 {
                     GasStationId = Guid.NewGuid()
                 });

            // throw new Exception("User should not see this exception");

            return Ok(new List<WeatherForecast>{
                new WeatherForecast{
                    Summary= "Weather is good",
                    Date=DateTime.Now,
                    TemperatureC = 30
                }
            });
        }
    }
}
