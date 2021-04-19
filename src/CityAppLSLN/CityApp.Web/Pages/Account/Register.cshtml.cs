using System;
using System.Threading.Tasks;
using CityApp.Interfaces;
using CityApp.Models;
using CityApp.Web.Common;
using CityApp.Web.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CityApp.Web.Pages.Account
{
    [AllowAnonymous]
    public class RegisterPageModel : GeneratorBasePageModel
    {
        private readonly ICityUserRepository userRepository;
        private readonly ILogger<RegisterPageModel> logger;

        public RegisterPageModel(ICityUserRepository userRepository,
            ILogger<RegisterPageModel> logger) 
        {
            this.userRepository = userRepository;
            this.logger = logger;
        }

        [BindProperty] public CityUser NewUser { get; set; }

        public void OnGet() => logger.LogInformation("Loading registration form");

        public async Task<IActionResult> OnPostAsync()
        {
            logger.LogInformation($"Registering user at {DateTime.Now}");
            try
            {
                var userId = userRepository.Insert(NewUser);
                var currentUser = await userRepository.GetDetailsAsync((int) userId);

                logger.LogInformation($"Logged in at {DateTime.Now}");
                await HttpContext.SignInAsync(currentUser.GenerateClaims());
                InfoText = "User has been registered and login automatically";
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                InfoText = "There has been error signing up.";
            }

            return RedirectToPage("/Info/Index");
        }
    }
}