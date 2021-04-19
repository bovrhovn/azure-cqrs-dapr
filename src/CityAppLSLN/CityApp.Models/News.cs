using System.Collections.Generic;

namespace CityApp.Models
{
    public class News
    {
        public int NewsId { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string ExternalLink { get; set; }
        public string Content { get; set; }
        public List<Category> Categories { get; set; } = new();
        public List<CityUser> Users { get; set; } = new();
    }
}