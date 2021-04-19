using System;
using System.Collections.Generic;

namespace CityApp.Models
{
    public class CityUser
    {
        public int CityUserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime ApprovalDate { get; set; }
        public bool Approved { get; set; }
        public List<ElectricityMeasurement> ElectricityMeasurement { get; set; } = new();
        public List<News> Subscriptions { get; set; } = new();
    }
}