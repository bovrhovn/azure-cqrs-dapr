using System.Threading.Tasks;
using CityApp.Engine;
using CityApp.Models;

namespace CityApp.Interfaces
{
    public interface IElectricityMeasurementRepository : IDataRepository<ElectricityMeasurement>
    {
        Task<PaginatedList<ElectricityMeasurement>> GetPagedForUserAsync(int cityUserId, int? electricityId, int page,
            int pageCount = 20);
    }
}