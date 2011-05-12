using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPC_2010.TimeZone
{
    public class TimeZoneConverter
    {
        public static DateTime ConvertToEastern(DateTime time)
        {
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(time, "US Eastern Standard Time");
        }
    }
}