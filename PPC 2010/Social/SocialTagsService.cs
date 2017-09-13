using PPC_2010.Social.Facebook;
using PPC_2010.Social.Twitter;
using System.Web.UI;

namespace PPC_2010.Social
{
    class SocialTagsService : ISocialTagsService
    {
        IOpenGraphTagsService _openGraphTagsService;
        ITwitterTagService _twitterTagsService;

        public SocialTagsService(IOpenGraphTagsService openGraphTagsService, ITwitterTagService twitterTagsService) {
            _openGraphTagsService = openGraphTagsService;
            _twitterTagsService = twitterTagsService;
        }

        public void AddSocialTags(UserControl userControl, OpenGraphTags tags)
        {
            _openGraphTagsService.AddOpenTags(userControl, tags);

            // Twitter will use the Facebook Open Graph tags, so we just need to set the card type.
            _twitterTagsService.AddTwitterCardTags(userControl, "summary");
        }
    }
}
