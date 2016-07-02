using System.Web.UI;

namespace PPC_2010.Social.Facebook
{
    interface IOpenGraphTagsService
    {
        void AddOpenTags(UserControl control, OpenGraphTags tags);
    }
}
