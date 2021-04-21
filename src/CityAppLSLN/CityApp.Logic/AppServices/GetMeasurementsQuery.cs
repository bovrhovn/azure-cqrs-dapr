using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CityApp.Engine;
using CityApp.Interfaces;
using CityApp.Logic.ViewModels;
using CityApp.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace CityApp.Logic.AppServices
{
    public class GetMeasurementsQuery : IRequest<MeasurementViewModel>
    {
        public int CityUserId { get; }
        public string Electricityid { get; }
        public int Page { get; }
        public int DefaultPageCount { get; }

        public GetMeasurementsQuery(int cityUserId, string electricityid, int page, int defaultPageCount)
        {
            CityUserId = cityUserId;
            Electricityid = electricityid;
            Page = page;
            DefaultPageCount = defaultPageCount;
        }
    }

    public class GetMeasurementsQueryHandler : IRequestHandler<GetMeasurementsQuery, MeasurementViewModel>
    {
        private readonly IElectricityRepository electricityRepository;
        private readonly IElectricityMeasurementRepository electricityMeasurementRepository;
        private readonly ILogger<GetMeasurementsQuery> logger;

        public GetMeasurementsQueryHandler(IElectricityRepository electricityRepository,
            IElectricityMeasurementRepository electricityMeasurementRepository,
            ILogger<GetMeasurementsQuery> logger)
        {
            this.electricityRepository = electricityRepository;
            this.electricityMeasurementRepository = electricityMeasurementRepository;
            this.logger = logger;
        }

        public async Task<MeasurementViewModel> Handle(GetMeasurementsQuery request,
            CancellationToken cancellationToken)
        {
            logger.LogInformation($"Electricity page loaded with query string {request.Electricityid}");
            var measurementViewModel = new MeasurementViewModel();

            logger.LogInformation("Loading data for electricity");
            var list = await electricityRepository.GetAllAsync();
            var currentData = list.Select(currentItem => new SelectListItem
            {
                Text = currentItem.Name, Value = currentItem.ElectricityId.ToString(),
                Selected = currentItem.ElectricityId.ToString() == request.Electricityid
            }).ToList();

            measurementViewModel.ElectricityList = currentData;
            measurementViewModel.ElectricityList.Insert(0, new SelectListItem("--- Pick from list ---", ""));

            var measurements = new PaginatedList<ElectricityMeasurement>();
            if (string.IsNullOrEmpty(request.Electricityid))
            {
                logger.LogInformation($"Doing filter without electricity");
                measurements = await electricityMeasurementRepository.GetPagedForUserAsync(request.CityUserId, null,
                    request.Page,
                    request.DefaultPageCount);
            }
            else
            {
                logger.LogInformation($"Doing filter with electricity {request.Electricityid}");
                var electricityId = int.Parse(request.Electricityid);
                measurements = await electricityMeasurementRepository.GetPagedForUserAsync(request.CityUserId,
                    electricityId,
                    request.Page,
                    request.DefaultPageCount);
            }

            var currentList = measurements.Select(measurement => new ElectricityViewModel
                {
                    EntryDate = measurement.EntryDate, HighWats = measurement.HighWats, LowWatts = measurement.LowWatts
                }).ToList();
            measurementViewModel.Measurements =
                PaginatedList<ElectricityViewModel>.Create(currentList.AsQueryable(), request.Page,
                    request.DefaultPageCount);
            logger.LogInformation("Returning results to the user");
            return measurementViewModel;
        }
    }
}