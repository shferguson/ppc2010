using Google.Apis.Calendar.v3;
using System;

namespace PPC_2010.CalendarInterface
{
    public interface IGoogleCalendarService : IDisposable
    {
        CalendarService Service { get; }
    }
}