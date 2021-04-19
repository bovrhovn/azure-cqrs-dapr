using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using CityApp.Models;

namespace CityApp.Interfaces
{
    public interface ICityUserRepository : IDataRepository<CityUser>
    {
        Task<bool> LoginAsync(string username, string password);
    }
}