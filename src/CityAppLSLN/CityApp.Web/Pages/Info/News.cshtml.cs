using System.Threading.Tasks;
using CityApp.Engine;
using CityApp.Interfaces;
using CityApp.Models;
using CityApp.Web.Common;
using CityApp.Web.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CityApp.Web.Pages.Info
{
    public class NewsPageModel : GeneratorBasePageModel
    {
        private readonly ILogger<NewsPageModel> logger;
        private readonly INewsRepository newsRepository;
        private readonly WebSettings webSettings;

        public NewsPageModel(ILogger<NewsPageModel> logger, INewsRepository newsRepository,IOptions<WebSettings> webSettingsValue)
        {
            this.logger = logger;
            this.newsRepository = newsRepository;
            webSettings = webSettingsValue.Value;
        }

        public async Task OnGetAsync(int? pageIndex)
        {
            var page = pageIndex ?? 1;
            logger.LogInformation($"Loading news for {Query}");
            var data = await newsRepository.SearchPagedAsync(Query, page, webSettings.DefaultPageCount);
            NewsList = data;
            logger.LogInformation($"Loaded {data.Count} items from database");
        }
        
        [BindProperty] public PaginatedList<News> NewsList { get; set; }
        [BindProperty(SupportsGet = true)] public string Query { get; set; }
    }
}