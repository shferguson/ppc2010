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
        private IPreValueRepository _PreValueRepository;

        public SermonSearch()
        {
            _PreValueRepository = ServiceLocator.Instance.Locate<IPreValueRepository>();
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

            var years = sermons.Where(s => s.RecordingDate.HasValue).Select(s => s.RecordingDate.Value.Year).Distinct().OrderByDescending(y => y).Select( y => new IdValuePair { Id = y, Value = y.ToString()});
            year.DataSource = PrependEmpyValue(years);
            year.DataValueField = "Id";
            year.DataTextField = "Value";
            year.DataBind();

            var months = Enumerable.Range(0, 13).Select(i => new IdValuePair { Id = i, Value = i == 0 ? "" : new DateTime(2010, i, 1).ToString("MMMM") });
            month.DataSource = months;
            month.DataValueField = "Id";
            month.DataTextField = "Value";
            month.DataBind();

            var speakers = _PreValueRepository.Speakers().Select(s => new IdValuePair { Id = s.Id, Value = s.Value });
            speaker.DataSource = PrependEmpyValue(speakers);
            speaker.DataValueField = "Id";
            speaker.DataTextField = "Value";
            speaker.DataBind();

            var audioTypes = _PreValueRepository.Sessions().Select(a => new IdValuePair { Id = a.Id, Value = a.Value });
            audioType.DataSource = PrependEmpyValue(audioTypes);
            audioType.DataValueField = "Id";
            audioType.DataTextField = "Value";
            audioType.DataBind();

            var series = _PreValueRepository.SermonSeries().Select(s => new IdValuePair { Id = s.Id, Value = s.Value });
            audioSeries.DataSource = PrependEmpyValue(series);
            audioSeries.DataValueField = "Id";
            audioSeries.DataTextField = "Value";
            audioSeries.DataBind();
        }

        private IEnumerable<IdValuePair> PrependEmpyValue(IEnumerable<IdValuePair> list)
        {
            return new IdValuePair[] { new IdValuePair() }.Union(list);
        }

        protected void searchClick(object sender, EventArgs e)
        {
            Response.Redirect(
                string.Format("~/sermonarchive.aspx?year={0}&month={1}&speakerId={2}&typeId={3}&seriesId={4}&title={5}",
                    year.Text, month.SelectedValue,
                    speaker.SelectedValue,
                    audioType.SelectedValue,
                    audioSeries.SelectedValue,
                    Server.UrlEncode(title.Text)));
        }
    }
}