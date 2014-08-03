using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PPC_2010.Data;
using PPC_2010.Data.LinqToSql;

namespace PPC_2010
{
    public partial class SermonList : System.Web.UI.UserControl
    {
        public int PageNumber { get; set; }
        public int ItemsPerPage { get; set; }

        public int SearchYear { get; set; }
        public int SearchMonth { get; set; }
        public int SearchSpeakerId { get; set; }
        public string SearchSpeaker { get; set; }
        public int SearchSeriesId { get; set; }
        public string SearchSeries { get; set; }
        public int SearchRecordingTypeId { get; set; }
        public string SearchRecordingType { get; set; }
        public string SearchTitle { get; set; }

        protected int sermonCount { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (PageNumber <= 0)
                PageNumber = 1;

            if (!Page.IsPostBack)
            {
                LoadSermons();
            }
        }

        private void LoadSermons()
        {
            using (ISermonRepository repository = ServiceLocator.Instance.Locate<ISermonRepository>())
            {
                var sermons = repository.LoadAllSermons();

                if (SearchYear != 0)
                    sermons = sermons.Where(s => s.RecordingDate.HasValue && s.RecordingDate.Value.Year == SearchYear);
                if (SearchMonth != 0)
                    sermons = sermons.Where(s => s.RecordingDate.HasValue && s.RecordingDate.Value.Month == SearchMonth);
                if (SearchSpeakerId != 0)
                    sermons = sermons.Where(s => s.SpeakerNameId == SearchSpeakerId);
                if (!string.IsNullOrEmpty(SearchSpeaker))
                    sermons = sermons.Where(s => String.Equals(s.SpeakerName, SearchSpeaker, StringComparison.CurrentCultureIgnoreCase));
                if (SearchSeriesId != 0)
                    sermons = sermons.Where(s => s.SermonSeriesId == SearchSeriesId);
                if (!string.IsNullOrEmpty(SearchSeries))
                    sermons = sermons.Where(s => String.Equals(s.SermonSeries, SearchSeries, StringComparison.CurrentCultureIgnoreCase));
                if (SearchRecordingTypeId != 0)
                    sermons = sermons.Where(s => s.RecordingSessionId == SearchRecordingTypeId);
                if (!string.IsNullOrEmpty(SearchRecordingType))
                    sermons = sermons.Where(s => String.Equals(s.RecordingSession, SearchRecordingType, StringComparison.CurrentCultureIgnoreCase));
                if (!string.IsNullOrEmpty(SearchTitle))
                    sermons = sermons.Where(s => s.Title.ToLower().Contains(SearchTitle));

                sermonCount = sermons.Count();

                sermons = sermons.Skip((PageNumber - 1) * ItemsPerPage).Take(ItemsPerPage);

                if (sermonCount == 0 && PageNumber != 1)
                    RedirectToPage(1);

                sermonGrid.DataSource = sermons;
                sermonGrid.DataBind();

                previous.Enabled = PageNumber > 1;
                next.Enabled = sermonCount > PageNumber * ItemsPerPage;
            }
        }

        protected void sermonGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string baseUrl = string.Format("{0}", (Request.ApplicationPath.Equals("/")) ? string.Empty : Request.ApplicationPath);

            if (e.Row.DataItem != null)
                e.Row.Attributes.Add("onclick", "window.location.href='" + ((ISermon)e.Row.DataItem).SermonUrl.Replace("~", baseUrl) + "'" );
        }

        protected void previousClick(object sender, EventArgs e)
        {
            RedirectToPage(PageNumber - 1);
        }

        protected void nextClick(object sender, EventArgs e)
        {
            RedirectToPage(PageNumber + 1);
        }

        private void RedirectToPage(int pageNumber)
        {
            Response.Redirect(Request.Url.AbsolutePath +
                string.Format("?year={0}&month={1}&speakerId={2}&speaker={3}&typeId={4}&type={5}&seriesId={6}&series={7}&title={8}&page={9}",
                    SearchYear,
                    SearchMonth,
                    SearchSpeakerId,
                    Server.UrlEncode(SearchSpeaker),
                    SearchRecordingTypeId,
                    Server.UrlEncode(SearchRecordingType),
                    SearchSeriesId,
                    Server.UrlEncode(SearchSeries),
                    Server.UrlEncode(SearchTitle),
                    pageNumber));
        }
    }
}