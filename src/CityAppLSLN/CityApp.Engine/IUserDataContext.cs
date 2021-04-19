namespace CityApp.Engine
{
    public interface IUserDataContext
    {
        CityUserViewModel GetCurrentUser();
    }
}