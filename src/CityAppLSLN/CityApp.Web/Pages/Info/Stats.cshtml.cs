using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CityApp.Web.Pages.Info
{
    public class StatsPageModel : PageModel
    {
        private readonly ILogger<StatsPageModel> logger;

        public StatsPageModel(ILogger<StatsPageModel> logger) => this.logger = logger;

        public void OnGet()
        {
            logger.LogInformation("Loading statistics for electricity page");
        }
    }
}