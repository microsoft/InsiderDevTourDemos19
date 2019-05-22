using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGraph
{
    public static class GraphExtensions
    {
        public static DateTimeOffset ToDateTimeOffset(this DateTimeTimeZone dttz)
        {
            // https://docs.microsoft.com/en-us/dotnet/standard/datetime/choosing-between-datetime
            var datetime = DateTime.Parse(dttz.DateTime);
            var timezone = TimeZoneInfo.FindSystemTimeZoneById(dttz.TimeZone);
            var dto = new DateTimeOffset(datetime, timezone.GetUtcOffset(datetime));

            return dto;
        }
    }
}
