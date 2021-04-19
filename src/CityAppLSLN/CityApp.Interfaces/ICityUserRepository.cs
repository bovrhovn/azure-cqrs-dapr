using System.Threading.Tasks;
using CityApp.Models;

namespace CityApp.Interfaces
{
    public interface ICityUserRepository : IDataRepository<CityUser>
    {
        Task<CityUser> LoginAsync(string username, string password);
    }
}