using System;
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
    public class NewsService : INewsService
    {
        private readonly HttpClient client;
        private readonly ILogger<NewsService> logger;

        public NewsService(HttpClient client, ILogger<NewsService> logger, 
            IOptions<WebSettings> webSettingsValue)
        {
            this.client = client;
            this.logger = logger;
            client.BaseAddress = new Uri(webSettingsValue.Value.NewsServiceUrl, UriKind.RelativeOrAbsolute);
        }

        public async Task<News> GetDetailsAsync(int newsId)
        {
            logger.LogInformation($"Calling details for news {newsId}");
            var receivedResponse = await client.GetStringAsync($"details/{newsId}");
            logger.LogInformation($"Received {receivedResponse}");
            return JsonConvert.DeserializeObject<News>(receivedResponse);
        }

        public async Task<PaginatedList<News>> SearchAsync(string query, int page, int pageCount)
        {
            logger.LogInformation($"Calling search for news with query {query}");
            var receivedResponse = await client.GetStringAsync($"search/{query}/{page}/{pageCount}");
            logger.LogInformation($"Received {receivedResponse}");
            return JsonConvert.DeserializeObject<PaginatedList<News>>(receivedResponse);
        }
    }
}