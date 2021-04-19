using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CityApp.Web.Pages.Status
{
    public class MessagesPageModel : PageModel
    {
        private readonly ILogger<IndexPageModel> logger;

        public MessagesPageModel(ILogger<IndexPageModel> logger) => this.logger = logger;

        public void OnGet()
        {
            logger.LogInformation("Messages page loaded.");
        }
    }
}