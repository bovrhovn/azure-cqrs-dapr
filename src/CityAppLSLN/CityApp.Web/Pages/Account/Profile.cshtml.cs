using System;
using System.Threading.Tasks;
using CityApp.Engine;
using CityApp.Interfaces;
using CityApp.Models;
using CityApp.Web.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CityApp.Web.Pages.Account
{
    [Authorize]
    public class ProfilePageModel : GeneratorBasePageModel
    {
        private readonly IUserDataContext userDataContext;
        private readonly ICityUserRepository cityUserRepository;
        private readonly ILogger<ProfilePageModel> logger;

        public ProfilePageModel(IUserDataContext userDataContext,
            ICityUserRepository cityUserRepository,
            ILogger<ProfilePageModel> logger)
        {
            this.userDataContext = userDataContext;
            this.cityUserRepository = cityUserRepository;
            this.logger = logger;
        }

        [BindProperty] public CityUser CurrentUser { get; set; }
        [BindProperty] public bool NotificationsEnabled { get; set; }

        public async Task OnGetAsync()
        {
            var cityUserViewModel = userDataContext.GetCurrentUser();
            CurrentUser = await cityUserRepository.GetDetailsAsync(cityUserViewModel.CityUserId);
            logger.LogInformation($"User {CurrentUser.FullName} loaded!");
        }

        public IActionResult OnPost()
        {
            logger.LogInformation($"Updating data for profile {CurrentUser.FullName}");
            try
            {
                cityUserRepository.Update(CurrentUser);
                var infoText = $"User {CurrentUser.FullName} has been updated";
                logger.LogInformation(infoText);
                InfoText = infoText;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                InfoText = "There has been an error updating user - " + e.Message;
            }
            return RedirectToPage("/Account/Profile");
        }
    }
}