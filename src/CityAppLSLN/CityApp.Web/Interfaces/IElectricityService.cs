using System.Collections.Generic;
using System.Threading.Tasks;
using CityApp.Engine;
using CityApp.Models;

namespace CityApp.Web.Interfaces
{
    public interface IElectricityService
    {
        Task<PaginatedList<ElectricityMeasurement>> SearchPagedAsync(int cityUserId, int electricityId, int page,
            int pageCount);
        Task<List<Electricity>> GetElectricityAsync();
    }
}