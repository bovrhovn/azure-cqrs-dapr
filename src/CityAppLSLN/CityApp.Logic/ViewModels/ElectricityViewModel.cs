using System;

namespace CityApp.Logic.ViewModels
{
    public class ElectricityViewModel
    {
        public DateTime EntryDate { get; set; }
        public int LowWatts { get; set; }
        public int HighWats { get; set; }
    }
}