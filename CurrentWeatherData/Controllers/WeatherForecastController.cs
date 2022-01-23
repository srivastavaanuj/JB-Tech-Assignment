using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using CurrentWeatherData.Services;
using Microsoft.AspNetCore.Http;

namespace CurrentWeatherData.Controllers
{
    [ApiController]
    [Route("api/weatherforecast")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ICurrentWeatherService _currentWeatherService;

        public WeatherForecastController(ICurrentWeatherService currentWeatherService )
        {
            _currentWeatherService = currentWeatherService;
        }

        [HttpGet("City/{city}/Country/{country}")]
        public async Task<IActionResult> GetCurrentWeather(string city,string country)  
        {
                try
                {
                    var query = $"{city},{country}";
                    var weatherResponse = await _currentWeatherService.GetWeatherReport(query);
                    if (weatherResponse.Weather == null) return NotFound("No Result found!");
                    return Ok(weatherResponse);
                }
                catch (HttpRequestException httpRequestException)
                {
                     return this.StatusCode(StatusCodes.Status500InternalServerError, httpRequestException.Message);
                }
        }
    }
}
