using System;
using Demo.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class UnixTime
    {
        [TestMethod]
        public void GetUtcTimeFromUnixTimeStamp()
        {
            var epoch = DateTimeHelper.GetUtcTimeFromUnixTimestamp(0);
            var utcTime = DateTimeHelper.GetUtcTimeFromUnixTimestamp(1455667199);
            Assert.AreEqual(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), epoch);
            Assert.AreEqual(new DateTime(2016, 2, 16, 23, 59, 59, 0, DateTimeKind.Utc), utcTime);
        }

        [TestMethod]
        public void GetLocalTimeFromUnixTime()
        {
            var localTime = DateTimeHelper.GetLocalTimeFromUnixTimestamp(1455591238);
            var aucklandTime = new DateTime(2016, 2, 16, 15, 53, 58, 0);
            TimeZoneInfo aucklandTimeZone = TimeZoneInfo.FindSystemTimeZoneById("New Zealand Standard Time");
            Assert.AreEqual(aucklandTime, TimeZoneInfo.ConvertTime(localTime, aucklandTimeZone));
        }

        [TestMethod]
        public void GetUnixTime()
        {
            var epochUnixTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).GetUnixTimestamp();
            var unixTime = new DateTime(2016, 2, 16, 23, 59, 59, 0, DateTimeKind.Utc).GetUnixTimestamp();
            Assert.AreEqual(0, epochUnixTime);
            Assert.AreEqual(1455667199, unixTime);
        }

        [TestMethod]
        public void GetUnixTimeFromLocalTime()
        {
            var aucklandTime = new DateTime(2016, 2, 16, 15, 53, 58, 0);
            TimeZoneInfo aucklandTimeZone = TimeZoneInfo.FindSystemTimeZoneById("New Zealand Standard Time");
            var localTime = TimeZoneInfo.ConvertTime(aucklandTime, aucklandTimeZone, TimeZoneInfo.Local);
            Assert.AreEqual(1455591238, localTime.GetUnixTimestampFromLocalTime());
        }
    }
}
