using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using Google.GData.Calendar;
using Google.GData.Extensions;

namespace PPC_2010.CalendarInterface
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
            List<CalendarItem> items = HttpContext.Current.Cache[BuildCacheKey(startDate, endDate)] as List<CalendarItem>;
            if (items == null)
            {
                try
                {
                    items = new List<CalendarItem>();



                    CalendarService service = new CalendarService("www.providence-pca.net");

                    if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                        service.setUserCredentials(username, password);

                    EventQuery query = new EventQuery();
                    query.Uri = new Uri(calendarUrl);

                    query.StartTime = startDate;
                    query.EndTime = endDate;
                    query.NumberToRetrieve = 250;

                    EventFeed feed = service.Query(query);

                    var feedEntries = feed.Entries.OfType<EventEntry>().Where(e => !e.Status.Value.Contains("cancel"));
                    foreach (EventEntry entry in feedEntries)
                    {
                        foreach (When w in entry.Times)
                        {
                            // Google seems to still include the overriden recurrance along with the override.
                            // The override has an OriginalEvent.  So when ever we see an element without an override
                            // we check for an element with the same time that has an original event
                            if (entry.OriginalEvent == null)
                            {
                                if (feedEntries.Any(
                                    e => e.OriginalEvent != null &&
                                         e.OriginalEvent.IdOriginal == entry.EventId &&
                                         e.Times.Any(t => t.StartTime == w.StartTime && t.EndTime == w.EndTime)))
                                {
                                    continue;
                                }
                            }

                            if (w.StartTime < endDate && w.EndTime > startDate)
                            {
                                items.Add(new CalendarItem
                                {
                                    Start = w.StartTime,
                                    End = w.EndTime,
                                    Title = entry.Title.Text,
                                    AllDay = w.AllDay
                                });
                            }
                        }
                    }

                    HttpContext.Current.Cache.Add(BuildCacheKey(startDate, endDate), items, null, DateTime.Now.AddSeconds(120), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
                    HttpContext.Current.Application.Contents[BuildCacheKey(startDate, endDate)] = items;
                }
                catch (Exception ex)
                {
                    // The Goolge calendar API seems to fail at times, swallow the exception and try to use a long lived cache item
                    Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(ex, HttpContext.Current));

                    items = HttpContext.Current.Application[BuildCacheKey(startDate, endDate)] as List<CalendarItem>;
                }
            }

            if (items == null)
                items = new List<CalendarItem>();

            return items;
        }

        private string BuildCacheKey(DateTime startDate, DateTime endDate)
        {
            return "Calendar - " + startDate.ToShortDateString() + " - " + endDate.ToShortDateString();
        }
    }
}