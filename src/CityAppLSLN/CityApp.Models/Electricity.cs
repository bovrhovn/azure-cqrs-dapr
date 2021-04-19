using System.Collections.Generic;

namespace CityApp.Models
{
    public class Electricity
    {
        public int ElectricityId { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public int InitialCounter { get; set; }
        public List<ElectricityMeasurement> Measurements { get; set; } = new();
    }
}