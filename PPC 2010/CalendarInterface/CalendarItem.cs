using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PPC_2010.TimeZone;

namespace PPC_2010.CalendarInterface
{
    public class CalendarItem : IComparable<CalendarItem>
    {
        private DateTime? start;
        public DateTime? Start
        {
            get { return start; }
            set
            {
                if (value.HasValue)
                    start = TimeZoneConverter.ConvertToEastern(value.Value);
                else
                    start = null;
            }
        }

        private DateTime? end;
        public DateTime? End
        {
            get { return end; }
            set
            {
                if (value.HasValue)
                    end = TimeZoneConverter.ConvertToEastern(value.Value);
                else
                    end = null;
            }
        }

        public string Title { get; set; }

        public bool AllDay { get; set; }

        public int CompareTo(CalendarItem other)
        {
            if (Start != other.Start && Start.HasValue && other.Start.HasValue)
                return Start.Value.CompareTo(other.Start.Value);
            if (End != other.End && End.HasValue && other.End.HasValue)
                return End.Value.CompareTo(other.End.Value);
            if (AllDay != other.AllDay)
                return AllDay.CompareTo(other.AllDay);
            if (Title != other.Title)
                return Title.CompareTo(other.Title);
            return 0;
        }
    }
}