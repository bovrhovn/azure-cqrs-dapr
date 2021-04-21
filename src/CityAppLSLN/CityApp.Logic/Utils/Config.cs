namespace CityApp.Logic.Utils
{
    public class Config
    {
        public int NumberOfDatabaseRetries { get; }

        public Config(int numberOfDatabaseRetries)
        {
            NumberOfDatabaseRetries = numberOfDatabaseRetries;
        }
    }
}