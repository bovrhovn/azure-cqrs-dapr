using System.Threading.Tasks;
using CityApp.Engine;
using CityApp.Models;

namespace CityApp.Interfaces
{
    public interface INewsRepository : IDataRepository<News>
    {
        Task<PaginatedList<News>> SearchPagedAsync(string query,int page, int pageCount = 20);
    }
}