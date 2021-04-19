using System.Threading.Tasks;
using CityApp.Interfaces;
using CityApp.Models;
using CityApp.Web.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CityApp.Web.Pages.Info
{
    public class DetailsPageModel : GeneratorBasePageModel
    {
        private readonly ILogger<DetailsPageModel> logger;
        private readonly INewsRepository newsRepository;

        public DetailsPageModel(ILogger<DetailsPageModel> logger, INewsRepository newsRepository)
        {
            this.logger = logger;
            this.newsRepository = newsRepository;
        }

        public async Task OnGetAsync(int newsId)
        {
            logger.LogInformation($"Getting news for {newsId}");
            var news = await newsRepository.GetDetailsAsync(newsId);
            logger.LogInformation($"Received {news.Title} back");
            CurrentNews = news;
        }

        [BindProperty]
        public News CurrentNews { get; set; }
    }
}