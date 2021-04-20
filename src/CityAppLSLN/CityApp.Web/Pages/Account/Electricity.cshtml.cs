using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityApp.Engine;
using CityApp.Interfaces;
using CityApp.Models;
using CityApp.Web.Common;
using CityApp.Web.Helpers;
using CityApp.Web.Settings;
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
        private readonly IElectricityRepository electricityRepository;
        private readonly IElectricityMeasurementRepository electricityMeasurementRepository;
        private readonly IUserDataContext userDataContext;
        private WebSettings webSettings;

        public ElectricityPageModel(ILogger<ElectricityPageModel> logger,
            IElectricityRepository electricityRepository,
            IElectricityMeasurementRepository electricityMeasurementRepository,
            IUserDataContext userDataContext,
            IOptions<WebSettings> webSettingsValue)
        {
            this.logger = logger;
            this.electricityRepository = electricityRepository;
            this.electricityMeasurementRepository = electricityMeasurementRepository;
            this.userDataContext = userDataContext;
            webSettings = webSettingsValue.Value;
        }

        public async Task OnGetAsync(int? pageIndex)
        {
            logger.LogInformation($"Electricity page loaded with query string {Electricity}");
            var list = await electricityRepository.GetAllAsync();
            ElectricityList =
                list.ToList().GetSelectedItems(d => d.Name + " " + d.Place, d => d.ElectricityId.ToString(),
                    d => d.ElectricityId.ToString() == Electricity);

            ElectricityList.Insert(0, new SelectListItem("--- Pick from list ---", ""));

            var userId = userDataContext.GetCurrentUser().CityUserId;
            var page = pageIndex ?? 1;
            if (string.IsNullOrEmpty(Electricity))
            {
                logger.LogInformation($"Doing filter without electricity");
                Measurements =
                    await electricityMeasurementRepository.GetPagedForUserAsync(userId, null, page,
                        webSettings.DefaultPageCount);
            }
            else
            {
                logger.LogInformation($"Doing filter with electricity {Electricity}");
                var electricityId = int.Parse(Electricity);
                Measurements =
                    await electricityMeasurementRepository.GetPagedForUserAsync(userId, electricityId, page,
                        webSettings.DefaultPageCount);
            }
        }

        [BindProperty(SupportsGet = true)] public string Electricity { get; set; }
        [BindProperty] public List<SelectListItem> ElectricityList { get; set; }
        public PaginatedList<ElectricityMeasurement> Measurements { get; set; }
    }
}