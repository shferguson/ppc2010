using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPC_2010.Calendar
{
    public class CalendarItem : IComparable<CalendarItem>
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Title { get; set; }

        public int CompareTo(CalendarItem other)
        {
            if (Start == other.Start)
            {
                if (End == other.End)
                    return Title.CompareTo(other.Title);
                return End.CompareTo(other.End);
            }
            return Start.CompareTo(other.Start);
        }
    }
}