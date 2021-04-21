using System;
using CityApp.Generator;
using CityApp.Interfaces;
using CityApp.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]

namespace CityApp.Generator
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var sqlConnection = Environment.GetEnvironmentVariable("SQLConnString");
            var newsRepository = new NewsRepository(sqlConnection);
            builder.Services.AddTransient<INewsRepository, NewsRepository>(_ => newsRepository);
            var electricityMeasurement = new ElectricityMeasurementRepository(sqlConnection);
            builder.Services.AddTransient<IElectricityMeasurementRepository, ElectricityMeasurementRepository>(_ => electricityMeasurement);
        }
    }
}