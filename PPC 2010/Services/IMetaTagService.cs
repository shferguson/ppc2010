using System.Web.UI;

namespace PPC_2010.Services
{
    interface IMetaTagService
    {
        void AddMetaTag(UserControl control, string property, string content);
    }
}
