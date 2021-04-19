using CityApp.Web.Models;

namespace CityApp.Web.Common
{
    public interface IUserDataContext
    {
        CityUserViewModel GetCurrentUser();
    }
}