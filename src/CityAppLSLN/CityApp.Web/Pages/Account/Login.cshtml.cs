using System;
using System.Threading.Tasks;
using CityApp.Interfaces;
using CityApp.Web.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CityApp.Web.Pages.Account
{
    [AllowAnonymous]
    public class LoginPageModel : PageModel
    {
        private readonly ICityUserRepository userRepository;
        private readonly ILogger<LoginPageModel> logger;

        [BindProperty] public string Email { get; set; }
        [BindProperty] public string Password { get; set; }
        [BindProperty] public string ReturnUrl { get; set; }
        
        public LoginPageModel(ICityUserRepository userRepository, ILogger<LoginPageModel> logger)
        {
            this.userRepository = userRepository;
            this.logger = logger;
        }

        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl;
            logger.LogInformation($"Return url {returnUrl} has been set...");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            logger.LogInformation(@"Logging in user with {Email}");
            var currentUser = await userRepository.LoginAsync(Email, Password);
            if (currentUser == null) return RedirectToPage("Login");

            logger.LogInformation($"User {Email} logged in at {DateTime.Now}");
            
            await HttpContext.SignInAsync(currentUser.GenerateClaims());

            if (!string.IsNullOrEmpty(ReturnUrl))
                return Redirect(Url.IsLocalUrl(ReturnUrl) ? ReturnUrl : "/");
            
            logger.LogInformation("Redirecting to default route - index");
            return RedirectToPage("/Info/Index");
        }
    }
}