using System;

namespace Uwp.App.Models
{
    public class Journal
    {
        public Journal(string @event, short year, string code = default, string place = default)
        {
            Event = @event;
            Code = code;
            Place = place;
            Year = year;
        }

        public string Event { get; set; }
        public string Code { get; set; }
        public string Place { get; set; }
        public short Year { get; set; }
    }
}
