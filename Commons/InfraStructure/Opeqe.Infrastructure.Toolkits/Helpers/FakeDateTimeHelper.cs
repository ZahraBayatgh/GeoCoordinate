using System;
using System.Collections.ObjectModel;

namespace Opeqe.Infrastructure.Toolkits.Helpers
{
    public class FakeDateTimeHelper : IDateTimeHelper
    {
        public TimeZoneInfo DefaultStoreTimeZone { get; set; }
        public TimeZoneInfo CurrentTimeZone { get; set; }

        public DateTime ConvertToUserTime(DateTime dt)
        {
            throw new NotImplementedException();
        }

        public DateTime ConvertToUserTime(DateTime dt, DateTimeKind sourceDateTimeKind)
        {
            return dt;
        }

        public DateTime ConvertToUserTime(DateTime dt, TimeZoneInfo sourceTimeZone)
        {
            throw new NotImplementedException();
        }

        public DateTime ConvertToUserTime(DateTime dt, TimeZoneInfo sourceTimeZone, TimeZoneInfo destinationTimeZone)
        {
            throw new NotImplementedException();
        }

        public DateTime ConvertToUtcTime(DateTime dt)
        {
            throw new NotImplementedException();
        }

        public DateTime ConvertToUtcTime(DateTime dt, DateTimeKind sourceDateTimeKind)
        {
            throw new NotImplementedException();
        }

        public DateTime ConvertToUtcTime(DateTime dt, TimeZoneInfo sourceTimeZone)
        {
            throw new NotImplementedException();
        }

        public TimeZoneInfo FindTimeZoneById(string id)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<TimeZoneInfo> GetSystemTimeZones()
        {
            throw new NotImplementedException();
        }
    }
}
