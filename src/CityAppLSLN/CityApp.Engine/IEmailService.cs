using System.Threading.Tasks;

namespace CityApp.Engine
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string from, string to,string subject, string body);
    }
}