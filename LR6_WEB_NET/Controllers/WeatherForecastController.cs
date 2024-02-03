using LR6_WEB_NET.Models.API;
using Microsoft.AspNetCore.Mvc;

namespace LR6_WEB_NET.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get array of daily forecasts.
        /// </summary>
        /// <param name="daysCount" >Number of days to forecast (between 1 inclusively and 14 inclusively). Default is 1</param>
        /// <returns></returns>
        [HttpGet("{daysCount:int:range(1,14)}",Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get(int daysCount)
        {
            return Enumerable.Range(1, daysCount).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}