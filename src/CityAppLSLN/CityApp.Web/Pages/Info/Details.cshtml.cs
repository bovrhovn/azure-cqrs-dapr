using System;
using System.Threading.Tasks;
using CityApp.Engine;
using CityApp.Interfaces;
using CityApp.Logic.AppServices;
using CityApp.Logic.ViewModels;
using CityApp.Models;
using CityApp.Web.Common;
using MediatR;
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
        private readonly IMediator mediator;

        public DetailsPageModel(ILogger<DetailsPageModel> logger, INewsRepository newsRepository,
            ICityUserRepository cityUserRepository, IUserDataContext userDataContext,
            IMediator mediator)
        {
            this.logger = logger;
            this.newsRepository = newsRepository;
            this.cityUserRepository = cityUserRepository;
            this.userDataContext = userDataContext;
            this.mediator = mediator;
        }

        public async Task OnGetAsync(int newsId)
        {
            var newsDetailsViewModel =
                await mediator.Send(new GetNewsDetailsQuery(newsId,
                    userDataContext.GetCurrentUser()?.CityUserId ?? -1));
            CurrentNews = newsDetailsViewModel;
            IsCurrentLoggedInUserSubscribed = newsDetailsViewModel.IsCurrentLoggedInUserSubscribed;
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

        [BindProperty] public NewsDetailsViewModel CurrentNews { get; set; }
        [BindProperty] public bool IsCurrentLoggedInUserSubscribed { get; set; }
    }
}