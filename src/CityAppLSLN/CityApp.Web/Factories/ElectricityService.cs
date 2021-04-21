using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CityApp.Engine;
using CityApp.Models;
using CityApp.Web.Interfaces;
using CityApp.Web.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CityApp.Web.Factories
{
    public class ElectricityService : IElectricityService
    {
        private readonly HttpClient client;
        private readonly ILogger<ElectricityService> logger;

        public ElectricityService(HttpClient client, ILogger<ElectricityService> logger,
            IOptions<WebSettings> webSettingsValue)
        {
            this.client = client;
            this.logger = logger;
            client.BaseAddress = new Uri(webSettingsValue.Value.ElectricityServiceUrl, UriKind.RelativeOrAbsolute);
        }

        public async Task<PaginatedList<ElectricityMeasurement>> SearchPagedAsync(int cityUserId, int electricityId,
            int page, int pageCount)
        {
            logger.LogInformation($"Calling details for user {cityUserId}");
            var receivedResponse =
                await client.GetStringAsync($"search/{cityUserId}/{electricityId}/{page}/{pageCount}");
            logger.LogInformation($"Received {receivedResponse}");
            return JsonConvert.DeserializeObject<PaginatedList<ElectricityMeasurement>>(receivedResponse);
        }

        public async Task<List<Electricity>> GetElectricityAsync()
        {
            logger.LogInformation($"Calling details for electricity");
            var receivedResponse = await client.GetStringAsync("all");
            logger.LogInformation($"Received {receivedResponse}");
            return JsonConvert.DeserializeObject<List<Electricity>>(receivedResponse);
        }
    }
}