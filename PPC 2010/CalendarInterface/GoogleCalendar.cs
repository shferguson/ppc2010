using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using Google.Apis.Calendar.v3.Data;

namespace PPC_2010.CalendarInterface
{
    public class GoogleCalendar
    {
        private IGoogleCalendarService _calendarService;

        public GoogleCalendar(IGoogleCalendarService calendarService)
        {
            _calendarService = calendarService;
        }

        public List<CalendarItem> GetCalendarItems(string calendarId, DateTime startDate, DateTime endDate)
        {
            List<CalendarItem> items = HttpContext.Current.Cache[BuildCacheKey(startDate, endDate)] as List<CalendarItem>;
            if (items == null)
            {
                try
                {
                    items = new List<CalendarItem>();

                    var listRequest = _calendarService.Service.Events.List(calendarId);
                    listRequest.TimeMin = startDate;
                    listRequest.TimeMax = endDate;
                    listRequest.SingleEvents = true;    // Show recurring events as invividual events
                    listRequest.Fields = "items(end,start,summary)";
                    
                    var events = listRequest.Execute();

                    foreach (var eventItem in events.Items)
                    {
                        bool isAlldayEvent = eventItem.Start.DateTime == null;
                        if (isAlldayEvent)
                        {
                            AddItemsForAllDateEvent(endDate, items, eventItem);
                        }
                        else {
                            items.Add(new CalendarItem
                            {
                                Start = eventItem.Start.DateTime,
                                End = eventItem.End.DateTime,
                                AllDay = false,
                                Title = eventItem.Summary,
                            });
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

        private static void AddItemsForAllDateEvent(DateTime endDate, List<CalendarItem> items, Event eventItem)
        {
            // Google only returns one event for a multi-day all day event, we need to add an individual CalendarItem
            // for each day

            DateTime eventStartDate, eventEndDate;
            if (DateTime.TryParse(eventItem.Start.Date, out eventStartDate) && DateTime.TryParse(eventItem.End.Date, out eventEndDate))
            {
                while (eventStartDate < eventEndDate && eventStartDate < endDate)
                {
                    items.Add(new CalendarItem
                    {
                        Start = eventStartDate.Date,
                        End = eventStartDate.Date,
                        AllDay = true,
                        Title = eventItem.Summary,
                    });
                    eventStartDate = eventStartDate.AddDays(1);
                }
            }
        }

        private string BuildCacheKey(DateTime startDate, DateTime endDate)
        {
            return "Calendar - " + startDate.ToShortDateString() + " - " + endDate.ToShortDateString();
        }
    }
}