using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Google.GData.Client;
using Google.GData.Calendar;
using Google.GData.Extensions;

namespace PPC_2010.Calendar
{
    public class GoogleCalendar
    {
        private string calendarUrl = null;
        private string username = null;
        private string password = null;

        public GoogleCalendar(string calendarUrl)
        {
            this.calendarUrl = calendarUrl;
        }

        public GoogleCalendar(string calendarUrl, string username, string password)
        {
            this.calendarUrl = calendarUrl;
            this.username = username;
            this.password = password;
        }


        public List<CalendarItem> GetCalendarItems(DateTime startDate, DateTime endDate)
        {
            CalendarService service = new CalendarService("www.providence-pca.net");

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                service.setUserCredentials(username, password);

            EventQuery query = new EventQuery();
            query.Uri = new Uri(calendarUrl);
            
            query.StartTime = startDate;
            query.EndTime = endDate;

            EventFeed feed = service.Query(query);

            List<CalendarItem> items = new List<CalendarItem>();

            foreach (EventEntry entry in feed.Entries)
            {
                foreach (When w in entry.Times)
                {
                    if (w.StartTime < endDate && w.EndTime > startDate)
                    {
                        items.Add(new CalendarItem
                        {
                            Start = w.StartTime,
                            End = w.EndTime,
                            Title = entry.Title.Text
                        });
                    }
                }
            }

            return items;
        }
    }
}