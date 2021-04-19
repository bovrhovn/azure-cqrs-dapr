using CityApp.Web.Pages.Info;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CityApp.Web.Pages.Status
{
    public class MessagesPageModel : PageModel
    {
        private readonly ILogger<MessagesPageModel> logger;

        public MessagesPageModel(ILogger<MessagesPageModel> logger) => this.logger = logger;

        public void OnGet()
        {
            logger.LogInformation("Messages page loaded");
        }
    }
}