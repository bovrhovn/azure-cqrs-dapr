using System.Collections.Generic;
using CityApp.Engine;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CityApp.Logic.ViewModels
{
    public class MeasurementViewModel
    {
        public PaginatedList<ElectricityViewModel> Measurements { get; set; }
        public List<SelectListItem> ElectricityList { get; set; }
    }
}