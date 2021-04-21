using System.Threading.Tasks;
using CityApp.Engine;
using CityApp.Models;

namespace CityApp.Web.Interfaces
{
    public interface INewsService
    {
        Task<News> GetDetailsAsync(int newsId);
        Task<PaginatedList<News>> SearchAsync(string query, int page, int pageCount);
    }
}