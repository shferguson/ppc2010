using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PPC_2010.Data;

namespace PPC_2010
{
    public partial class AllSermons : System.Web.UI.UserControl
    {
        int pageNumber = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (int.TryParse(Request.Params["page"] ?? "1", out pageNumber))
                sermonTitle.Text = pageNumber.ToString();

            if (!Page.IsPostBack)
            {
                LoadSermons();
            }
        }

        private void LoadSermons()
        {
            using (ISermonRepository repository = new SermonLinqToSqlRepository())
            {
                var sermons = repository.LoadAllSermons();
                if (sermons.Count() == 0 && pageNumber != 1)
                    Response.Redirect(Request.Url.AbsolutePath + "?page=1");

                sermonGrid.DataSource = sermons;
                sermonGrid.DataBind();
            }
        }

        protected void previousClick(object sender, EventArgs e)
        {
            if (pageNumber > 1)
            {
                pageNumber -= 1;
                Response.Redirect(Request.Url.AbsolutePath + "?page=" + pageNumber);
            }
        }

        protected void nextClick(object sender, EventArgs e)
        {
            using (ISermonRepository repository = new SermonLinqToSqlRepository())
            {
                if (repository.GetNumberOfSermons() > pageNumber * 10)
                {
                    pageNumber += 1;
                    Response.Redirect(Request.Url.AbsolutePath + "?page=" + pageNumber);
                }
            }
        }
    }
}