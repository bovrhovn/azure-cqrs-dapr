using System.Threading.Tasks;
using CityApp.Web.Models;

namespace CityApp.Web.Interfaces
{
    public interface IMessageService
    {
        Task<bool> SetMessageAsync(MessageModel model);
        Task<MessageModel> GetMessagesAsync(int cityUserId);
    }
}