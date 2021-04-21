using System.Threading;
using System.Threading.Tasks;
using CityApp.Engine;
using CityApp.Logic.AppServices;
using CityApp.Logic.ViewModels;
using CityApp.Web.Common;
using CityApp.Web.Settings;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CityApp.Web.Pages.Info
{
    public class NewsPageModel : GeneratorBasePageModel
    {
        private readonly ILogger<NewsPageModel> logger;
        private readonly IMediator mediator;
        private readonly WebSettings webSettings;

        public NewsPageModel(ILogger<NewsPageModel> logger, IMediator mediator,
            IOptions<WebSettings> webSettingsValue)
        {
            this.logger = logger;
            this.mediator = mediator;
            webSettings = webSettingsValue.Value;
        }

        public async Task OnGetAsync(int? pageIndex) =>
            NewsList = await mediator.Send(new SearchNewsQuery(Query, pageIndex ?? 1, webSettings.DefaultPageCount),
                CancellationToken.None);

        [BindProperty] public PaginatedList<NewsViewModel> NewsList { get; set; }
        [BindProperty(SupportsGet = true)] public string Query { get; set; }
    }
}