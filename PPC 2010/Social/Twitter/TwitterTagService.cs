using PPC_2010.Services;
using System.Web.UI;

namespace PPC_2010.Social.Twitter
{
    class TwitterTagService : ITwitterTagService
    {
        private IMetaTagService _metaTagService;

        public TwitterTagService(IMetaTagService metaTagService)
        {
            _metaTagService = metaTagService;
        }

        public void AddTwitterCardTags(UserControl userControl, string cardType)
        {
            _metaTagService.AddMetaTag(userControl, "twitter:card", cardType);
        }
    }
}
