using System.Web.UI;

namespace PPC_2010.Services
{
    class MetaTagService : IMetaTagService
    {
        public void AddMetaTag(UserControl control, string property, string content)
        {
            control.Page.Header.Controls.Add(new LiteralControl(string.Format("<meta property=\"{0}\" content=\"{1}\" />\r\n", property, content)));
        }
    }
}
