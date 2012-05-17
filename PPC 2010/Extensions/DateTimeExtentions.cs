using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPC_2010.Extensions
{
    public static class DateTimeExtentions
    {
        public static DateTime GetDateOfNext(this DateTime dateTime, DayOfWeek dayOfWeek)
        {
            return dateTime.AddDays(7 - (dateTime.DayOfWeek - dayOfWeek));
        }
    }
}