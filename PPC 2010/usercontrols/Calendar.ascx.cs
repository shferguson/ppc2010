using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaseControls;
using PPC_2010.CalendarInterface;

namespace PPC_2010
{
    public partial class Calendar : System.Web.UI.UserControl
    {
        public string CalendarId { get; set; }
        public string Date { get; set; }

        List<CalendarItem> items = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DateTime date;
                if (!DateTime.TryParse(Date, out date))
                    date = DateTime.Today;
                LoadCalendarItems(date);
            }
        }

        private void LoadCalendarItems(DateTime date)
        {
            DateTime startDate = new DateTime(date.Year, date.Month, 1);
            DateTime endDate = startDate.AddMonths(1);

            GoogleCalendar calendarService = ServiceLocator.Instance.Locate<GoogleCalendar>();
            items = calendarService.GetCalendarItems(CalendarId, startDate, endDate);
            items.Sort();
        }

        protected void CalendarBodyDay(BaseCalendar sender, BaseTagContentDay tag)
        {
            if (!tag.Info.IsOtherMonth)
            {
                tag.Content.WriteLine("<b><span class=\"emph1\">" + tag.DefaultText + "</span></b><br/>");

                var dayItems = items.Where(i => i.Start.GetValueOrDefault().Date == tag.Info.Date.Date).ToList();

                if (dayItems.Count > 0)
                {
                    tag.Content.WriteLine("<div class=\"calendarItems\">");

                    foreach (var item in dayItems)
                    {
                        if (item.AllDay)
                        {
                            tag.Content.WriteLine(string.Format("{0}<br />", item.Title));
                        }
                        else
                        {
                            tag.Content.WriteLine(
                                string.Format("<span class=\"emph1\">{0}:</span> {1}<br />",
                                item.Start.Value.ToString("t"), item.Title)
                            );
                        }
                    }

                    tag.Content.WriteLine("<br />");
                    tag.Content.WriteLine("</div>");
                }
            }
            else
            {
                tag.Content.WriteLine("<span class=\"calendarOtherMonth\">" + tag.DefaultText + "</span>");
            }
        }
    }
}