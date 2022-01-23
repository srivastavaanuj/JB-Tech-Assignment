using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CurrentWeatherData.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreRateLimit;
using CurrentWeatherData.Middleware;

namespace CurrentWeatherData
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
            services.AddOptions();
            services.AddMemoryCache();
            services.Configure<ClientRateLimitOptions>(Configuration.GetSection("ClientRateLimiting"));
            services.AddInMemoryRateLimiting();

            services.AddControllers();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<ICurrentWeatherService, CurrentWeatherService>();

            services.AddHttpClient("CurrentWeatherDataAPI", c => c.BaseAddress = new Uri("http://api.openweathermap.org"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseClientRateLimiting();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthorization();
            app.UseMiddleware<ApiKeyMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            


        }
    }
}
