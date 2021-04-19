using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace CityApp.Web.Pages.Info
{
    public class IndexPageModel : PageModel
    {
        private readonly ILogger<IndexPageModel> logger;

        public IndexPageModel(ILogger<IndexPageModel> logger) => this.logger = logger;

        public void OnGet()
        {
            logger.LogInformation("Info page loaded");
        }
    }
}