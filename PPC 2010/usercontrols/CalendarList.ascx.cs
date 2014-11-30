using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PPC_2010.CalendarInterface;
using System.Text;

namespace PPC_2010
{
    public partial class CalendarList : System.Web.UI.UserControl
    {
        public string CalendarId { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadCalendar();
            }
        }

        public class CalendarItemDayGroup
        {
            public string Date { get; set; }
            public IEnumerable<CalendarItemDisplay> Items { get; set; }
            
        }

        public class CalendarItemDisplay
        {
            public string Start { get; set; }
            public string Title { get; set; }
        }

        private void LoadCalendar()
        {
            GoogleCalendar calendar = ServiceLocator.Instance.Locate<GoogleCalendar>();

            var items = calendar.GetCalendarItems(CalendarId, DateTime.Today, DateTime.Today.AddDays(14));

            items.Sort();

            var itemsGroup = from i in items
                             where i.Start.HasValue
                             group i by i.Start.Value.Date into g
                             select new CalendarItemDayGroup { Date = g.Key.ToString("D"),
                                 Items = g.Select(i => new CalendarItemDisplay { Start = i.AllDay ? "" : i.Start.Value.ToString("t") + ": ", Title = i.Title }).ToList() };

            calendarList.DataSource = itemsGroup;
            calendarList.DataBind();
        }
    }
}