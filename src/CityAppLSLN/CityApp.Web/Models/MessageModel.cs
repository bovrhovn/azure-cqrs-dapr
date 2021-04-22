using System;
using System.Collections.Generic;

namespace CityApp.Web.Models
{
    public class MessageModel
    {
        public int CityUserId { get; set; }
        public List<CityMessage> Messages { get; set; } = new();
    }

    public class CityMessage
    {
        public string Message { get; set; }
        public DateTime EntryDate { get; set; }
    }
}