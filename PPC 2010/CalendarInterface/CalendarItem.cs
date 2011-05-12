using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PPC_2010.TimeZone;

namespace PPC_2010.CalendarInterface
{
    public class CalendarItem : IComparable<CalendarItem>
    {
        private DateTime start;
        public DateTime Start
        {
            get { return start; }
            set
            {
                start = TimeZoneConverter.ConvertToEastern(value);
            }
        }

        private DateTime end;
        public DateTime End
        {
            get { return end; }
            set
            {
                end = TimeZoneConverter.ConvertToEastern(value);
            }
        }

        public string Title { get; set; }

        public bool AllDay { get; set; }

        public int CompareTo(CalendarItem other)
        {
            if (Start != other.Start)
                return Start.CompareTo(other.Start);
            if (End != other.End)
                return End.CompareTo(other.End);
            if (AllDay != other.AllDay)
                return AllDay.CompareTo(other.AllDay);
            if (Title != other.Title)
                return Title.CompareTo(other.Title);
            return 0;
        }
    }
}