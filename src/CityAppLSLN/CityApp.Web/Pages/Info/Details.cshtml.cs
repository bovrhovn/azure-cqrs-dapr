using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CityApp.Engine;
using CityApp.Interfaces;
using CityApp.Models;
using CityApp.Web.Common;
using CityApp.Web.Interfaces;
using CityApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CityApp.Web.Pages.Info
{
    public class DetailsPageModel : GeneratorBasePageModel
    {
        private readonly ILogger<DetailsPageModel> logger;
        private readonly INewsService newsService;
        private readonly ICityUserRepository cityUserRepository;
        private readonly IUserDataContext userDataContext;
        private readonly IMessageService messageService;

        public DetailsPageModel(ILogger<DetailsPageModel> logger,
            INewsService newsService,
            ICityUserRepository cityUserRepository,
            IUserDataContext userDataContext, IMessageService messageService)
        {
            this.logger = logger;
            this.newsService = newsService;
            this.cityUserRepository = cityUserRepository;
            this.userDataContext = userDataContext;
            this.messageService = messageService;
        }

        public async Task OnGetAsync(int newsId)
        {
            logger.LogInformation($"Getting news for {newsId}");
            var news = await newsService.GetDetailsAsync(newsId);
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
                await messageService.SetMessageAsync(new MessageModel
                {
                    CityUserId = cityUserViewModel.CityUserId, Messages = new List<CityMessage>
                    {
                        new()
                        {
                            Message =
                                $"News {newsId} for user {cityUserViewModel.CityUserId} has been added to database",
                            EntryDate = DateTime.Now
                        }
                    }
                });
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