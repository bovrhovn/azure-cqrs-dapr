using System;

namespace CityApp.Models
{
    public class ElectricityMeasurement
    {
        public int ElectricityMeasurementId { get; set; }
        public DateTime EntryDate { get; set; }
        public int LowWatts { get; set; }
        public int HighWats { get; set; }
        public int CityUserId { get; set; }
        public CityUser CityUser { get; set; }
        public int ElectricityId { get; set; }
        public Electricity Electricity { get; set; }
    }
}