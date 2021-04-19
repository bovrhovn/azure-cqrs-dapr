using System.Threading.Tasks;
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
        private readonly ILogger<ProfilePageModel> logger;

        public ProfilePageModel(IUserDataContext userDataContext,
            ILogger<ProfilePageModel> logger)
        {
            this.userDataContext = userDataContext;
            this.logger = logger;
        }

        [BindProperty] public CityUser CurrentUser { get; set; }
        [BindProperty] public bool NotificationsEnabled { get; set; }

        public async Task OnGetAsync()
        {
            var cityUserViewModel = userDataContext.GetCurrentUser();
            CurrentUser = new CityUser
            {
                CityUserId = cityUserViewModel.CityUserId, FullName = cityUserViewModel.Fullname,
                Email = cityUserViewModel.Email
            };
            logger.LogInformation($"User {CurrentUser.FullName} loaded!");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            logger.LogInformation($"Updating alerting for ");
            var cityUserViewModel = userDataContext.GetCurrentUser();
            CurrentUser = new CityUser
            {
                CityUserId = cityUserViewModel.CityUserId, FullName = cityUserViewModel.Fullname,
                Email = cityUserViewModel.Email
            };

            return RedirectToPage("/Account/Profile");
        }
    }
}