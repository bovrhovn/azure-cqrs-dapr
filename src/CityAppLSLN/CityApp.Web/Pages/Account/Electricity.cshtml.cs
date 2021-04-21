using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityApp.Engine;
using CityApp.Interfaces;
using CityApp.Logic.AppServices;
using CityApp.Logic.ViewModels;
using CityApp.Models;
using CityApp.Web.Common;
using CityApp.Web.Helpers;
using CityApp.Web.Settings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CityApp.Web.Pages.Account
{
    [Authorize]
    public class ElectricityPageModel : GeneratorBasePageModel
    {
        private readonly ILogger<ElectricityPageModel> logger;
        private readonly IMediator mediator;
        private readonly IUserDataContext userDataContext;
        private readonly WebSettings webSettings;

        public ElectricityPageModel(ILogger<ElectricityPageModel> logger,
            IMediator mediator,
            IUserDataContext userDataContext,
            IOptions<WebSettings> webSettingsValue)
        {
            this.logger = logger;
            this.mediator = mediator;
            this.userDataContext = userDataContext;
            webSettings = webSettingsValue.Value;
        }

        public async Task OnGetAsync(int? pageIndex)
        {
            var dataModel = await mediator.Send(new GetMeasurementsQuery(
                userDataContext.GetCurrentUser().CityUserId,
                Electricity,
                pageIndex ?? 1, webSettings.DefaultPageCount));
            ElectricityList = dataModel.ElectricityList;
            Measurements = dataModel.Measurements;
        }

        [BindProperty(SupportsGet = true)] public string Electricity { get; set; }
        [BindProperty] public List<SelectListItem> ElectricityList { get; set; }
        public PaginatedList<ElectricityViewModel> Measurements { get; set; }
    }
}