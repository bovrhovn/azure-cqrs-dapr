using System;
using CityApp.Interfaces;
using CityApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CityApp.Generator
{
    public class GenerateElectricityMeasurement
    {
        private readonly IElectricityMeasurementRepository electricityMeasurementRepository;

        public GenerateElectricityMeasurement(IElectricityMeasurementRepository electricityMeasurementRepository) => this.electricityMeasurementRepository = electricityMeasurementRepository;

        [FunctionName("GenerateElectricityMeasurement")]
        public IActionResult RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
            HttpRequest req, ILogger log)
        {
            log.LogInformation($"Generating measurements at {DateTime.Now}");

            string name = req.Query["count"];
            
            int.TryParse(Environment.GetEnvironmentVariable("DefaultUserId"), out var userId);
            int.TryParse(Environment.GetEnvironmentVariable("DefaultElectricityId"), out var electricityId);
            
            int count = 100;
            if (!string.IsNullOrEmpty(name))
                count = int.Parse(name);

            var random = new Random(100);
            for (int counter = 0; counter < count; counter++)
            {
                log.LogInformation($"Adding {counter} to the database");
                var entity = new ElectricityMeasurement
                {
                    ElectricityId = electricityId,
                    CityUserId = userId,
                    HighWats = random.Next(50,1000),
                    LowWatts = random.Next(1,100),
                    EntryDate = DateTime.Now.AddDays(-random.Next(1,100))
                };
                electricityMeasurementRepository.Insert(entity);
            }

            return new OkObjectResult($"Generated {name} items");
            
        }
    }
}