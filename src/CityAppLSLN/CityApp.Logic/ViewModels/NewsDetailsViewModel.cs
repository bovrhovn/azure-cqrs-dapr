using System.Collections.Generic;

namespace CityApp.Logic.ViewModels
{
    public class NewsDetailsViewModel
    {
        public string ShortDescription { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public bool IsCurrentLoggedInUserSubscribed { get; set; }
        public int NewsId { get; set; }
        public List<CategoryViewModel> Categories { get; set; } = new();
    }
}