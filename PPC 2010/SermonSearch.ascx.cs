using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using PPC_2010.Data;
using PPC_2010.Data.Media;

namespace PPC_2010
{
    public partial class SermonSearch : System.Web.UI.UserControl
    {
        private IPrevalueRepository _prevalueRepository;

        public SermonSearch()
        {
            _prevalueRepository = ServiceLocator.Instance.Locate<IPrevalueRepository>();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                PopulateListBoxes();
            }
        }

        private void PopulateListBoxes()
        {
            ISermonRepository repository = ServiceLocator.Instance.Locate<ISermonRepository>();
            var sermons = repository.LoadAllSermons();

            var years = sermons.Where(s => s.RecordingDate.HasValue).Select(s => s.RecordingDate.Value.Year).Distinct().OrderByDescending(y => y).Select(y => y.ToString());
            year.DataSource = PrependEmptyString(years);
            year.DataBind();

            var months = Enumerable.Range(0, 13).Select(i => new { Month = i, String = i == 0 ? "" : new DateTime(2010, i, 1).ToString("MMMM") });
            month.DataSource = months;
            month.DataValueField = "Month";
            month.DataTextField = "String";
            month.DataBind();

            var speakers = _prevalueRepository.Speakers().Select(s => s.Value);
            speaker.DataSource = PrependEmptyString(speakers);
            speaker.DataBind();

            var audioTypes = _prevalueRepository.Sessions().Select(s => s.Value);
            audioType.DataSource = PrependEmptyString(audioTypes);
            audioType.DataBind();

            var series = _prevalueRepository.SermonSeries().Select(s => s.Value);
            audioSeries.DataSource = PrependEmptyString(series);
            audioSeries.DataBind();
        }

        private IEnumerable<string> PrependEmptyString(IEnumerable<string> list)
        {
            return new string[] { "" }.Union(list);
        }

        protected void searchClick(object sender, EventArgs e)
        {
            Response.Redirect(
                string.Format("~/sermonarchive.aspx?year={0}&month={1}&speaker={2}&type={3}&series={4}&title={5}",
                    year.Text, month.SelectedValue,
                    Server.UrlEncode(speaker.Text),
                    Server.UrlEncode(audioType.Text),
                    Server.UrlEncode(audioSeries.Text),
                    Server.UrlEncode(title.Text)));
        }
    }
}