using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CityApp.Web.Common
{
    public class GeneratorBasePageModel : PageModel
    {
        [BindProperty, TempData] public string InfoText { get; set; }
    }
}