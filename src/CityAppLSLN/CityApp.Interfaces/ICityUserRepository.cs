using System.Collections.Generic;
using System.Threading.Tasks;
using CityApp.Models;

namespace CityApp.Interfaces
{
    public interface ICityUserRepository : IDataRepository<CityUser>
    {
        Task<CityUser> LoginAsync(string username, string password);
        Task<bool> AddSubscriptionToNewsAsync(int userId, int newsId);
        Task<bool> RemoveSubscriptionFromNewsAsync(int userId, int newsId);
        Task<List<News>> GetSubscriptionsForUserAsync(int userId);
        Task<bool> IsSubscribedToNewsAsync(int userId, int newsId);
    }
}