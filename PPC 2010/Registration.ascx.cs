using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using umbraco.cms.businesslogic.web;
using umbraco.presentation.webservices;

namespace PPC_2010
{
    public partial class Registration : System.Web.UI.UserControl
    {
        public string NodeId { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Document document = new Document(Int32.Parse(NodeId));

            foreach (var prop in document.GenericProperties)
            {
                text.Text += prop.PropertyType.DataTypeDefinition.DataType.ToString() + "<br />";
            }
        }
    }
}