using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace Demo.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTime GetUtcTimeFromUnixTimestamp(double unixTime)
        {
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return unixEpoch.AddSeconds(unixTime);
        }

        public static DateTime GetLocalTimeFromUnixTimestamp(double unixTime)
        {
            return GetUtcTimeFromUnixTimestamp(unixTime).ToLocalTime();
        }

        public static double GetUnixTimestamp(this DateTime utcTime)
        {
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var timeSpan = utcTime - unixEpoch;
            return timeSpan.TotalSeconds;
        }

        public static double GetUnixTimestampFromLocalTime(this DateTime localTime)
        {
            return GetUnixTimestamp(localTime.ToUniversalTime());
        }
    }
}