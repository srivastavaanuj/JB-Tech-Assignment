using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrentWeatherData.Model;

namespace CurrentWeatherData.Services
{
    public interface ICurrentWeatherService
    {
        Task<WeatherDataResponse> GetWeatherReport(string query);
    }
}
