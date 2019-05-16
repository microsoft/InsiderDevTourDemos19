using System;

namespace Uwp.App.Models
{
    public class Item
    {
        public Item(string title, string description, double distance, DateTime date)
        {
            Title = title;
            Description = description;
            Distance = distance;
            Date = date;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public double Distance { get; set; }
        public DateTime Date { get; set; }
    }
}
