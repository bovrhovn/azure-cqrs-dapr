using System.Threading.Tasks;
using CityApp.Engine;
using CityApp.Models;
using CityApp.Web.Common;
using CityApp.Web.Interfaces;
using CityApp.Web.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CityApp.Web.Pages.Info
{
    public class NewsPageModel : GeneratorBasePageModel
    {
        private readonly ILogger<NewsPageModel> logger;
        private readonly INewsService newsService;
        private readonly WebSettings webSettings;

        public NewsPageModel(ILogger<NewsPageModel> logger, INewsService newsService,
            IOptions<WebSettings> webSettingsValue)
        {
            this.logger = logger;
            this.newsService = newsService;
            webSettings = webSettingsValue.Value;
        }

        public async Task OnGetAsync(int? pageIndex)
        {
            var page = pageIndex ?? 1;
            logger.LogInformation($"Loading news for {Query}");
            var data = await newsService.SearchAsync(Query, page, webSettings.DefaultPageCount);
            NewsList = data;
            logger.LogInformation($"Loaded {data.Count} items from database");
        }
        
        [BindProperty] public PaginatedList<News> NewsList { get; set; }
        [BindProperty(SupportsGet = true)] public string Query { get; set; }
    }
}