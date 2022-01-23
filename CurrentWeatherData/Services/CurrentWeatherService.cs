using CurrentWeatherData.Middleware;
using CurrentWeatherData.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CurrentWeatherData.Services
{
    public class CurrentWeatherService : ICurrentWeatherService
    {
        private readonly HttpClient _httpclient;
        private const string WEATHER_APP_ID = "8b7535b42fe1c551f18028f64e8688f7";
        public CurrentWeatherService(IHttpClientFactory httpClientFactory)
        {
            _httpclient = httpClientFactory.CreateClient("CurrentWeatherDataAPI");
        }
        public async Task<WeatherDataResponse> GetWeatherReport(string query)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(query))
                {
                    throw new ArgumentNullException(nameof(query));
                }
                var response = await _httpclient.GetAsync($"/data/2.5/weather?q={query}&appId={WEATHER_APP_ID}");
                //response.EnsureSuccessStatusCode();
                if(response==null)
                {
                    throw new ArgumentNullException(nameof(response));
                }

                var stringResult = await response.Content.ReadAsStringAsync();
                var weatherDataResponse = JsonConvert.DeserializeObject<Model.WeatherDataResponse>(stringResult);
                return weatherDataResponse;
            }
            catch (HttpRequestException httpRequestException)
            {
                throw new HttpRequestException(httpRequestException.Message);
               // return BadRequest($"Error getting weather from OpenWeather: {httpRequestException.Message}");
            }
        }

       
    }
}
