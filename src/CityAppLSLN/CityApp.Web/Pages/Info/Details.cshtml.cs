using System;
using System.Threading.Tasks;
using CityApp.Engine;
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
        private readonly ICityUserRepository cityUserRepository;
        private readonly IUserDataContext userDataContext;

        public DetailsPageModel(ILogger<DetailsPageModel> logger, INewsRepository newsRepository,
            ICityUserRepository cityUserRepository, IUserDataContext userDataContext)
        {
            this.logger = logger;
            this.newsRepository = newsRepository;
            this.cityUserRepository = cityUserRepository;
            this.userDataContext = userDataContext;
        }

        public async Task OnGetAsync(int newsId)
        {
            logger.LogInformation($"Getting news for {newsId}");
            var news = await newsRepository.GetDetailsAsync(newsId);
            logger.LogInformation($"Received {news.Title} back");
            CurrentNews = news;

            if (User.Identity is {IsAuthenticated: true})
            {
                //check, if subscribed to this news
                IsCurrentLoggedInUserSubscribed =
                    await cityUserRepository.IsSubscribedToNewsAsync(userDataContext.GetCurrentUser().CityUserId,
                        newsId);
            }
        }

        public async Task<IActionResult> OnPostAddSubscriptionAsync()
        {
            var cityUserViewModel = userDataContext.GetCurrentUser();
            logger.LogInformation("Adding subscription for user");
            var form = await Request.ReadFormAsync();
            var newsId = form["newsId"];
            if (string.IsNullOrEmpty(newsId))
            {
                var thereIsNoNewsIdSpecified = "There is no news id specified";
                logger.LogError(thereIsNoNewsIdSpecified);
                InfoText = thereIsNoNewsIdSpecified;
                return RedirectToPage("/Info/News");
            }

            try
            {
                var currentNewsId = int.Parse(newsId);
                await cityUserRepository.AddSubscriptionToNewsAsync(cityUserViewModel.CityUserId, currentNewsId);
                InfoText = $"Subscribed to news with id {currentNewsId} - check profile with submissions";
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                InfoText = "There has been an error subscribing - " + e.Message;
            }

            return RedirectToPage("/Info/Details", new {newsId});
        }

        [BindProperty] public News CurrentNews { get; set; }
        [BindProperty] public bool IsCurrentLoggedInUserSubscribed { get; set; }
    }
}