using System.Web.UI;
using PPC_2010.Social.Facebook;

namespace PPC_2010.Social
{
    interface ISocialTagsService
    {
        void AddSocialTags(UserControl userControl, OpenGraphTags tags);
    }
}