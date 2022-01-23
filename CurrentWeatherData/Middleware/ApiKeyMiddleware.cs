using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrentWeatherData.Middleware 
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private const string API_KEY = "ApiKey";
        public ApiKeyMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }
        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(API_KEY, out var clientApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Api Key was not provided.");
                return;
            }

            var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();

            var apiKey = appSettings.GetValue<string>(API_KEY);

            if (!apiKey.Equals(clientApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync
                      ("Unauthorized client.");
                return;
            }

            await _requestDelegate(context);
        }
    }
}
