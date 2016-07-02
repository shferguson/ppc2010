using PPC_2010.Services;
using System.Web.UI;

namespace PPC_2010.Social.Facebook
{
    class OpenGraphTagsService : IOpenGraphTagsService
    {
        private IMetaTagService _metaTagService;

        public OpenGraphTagsService(IMetaTagService metaTagService)
        {
            _metaTagService = metaTagService;
        }

        public void AddOpenTags(UserControl control, OpenGraphTags tags)
        {
            if (!string.IsNullOrEmpty(tags.Type))
                _metaTagService.AddMetaTag(control, "og:type", tags.Type);
            if (!string.IsNullOrEmpty(tags.Url))
                _metaTagService.AddMetaTag(control, "og:url", tags.Url);
            if (!string.IsNullOrEmpty(tags.Title))
                _metaTagService.AddMetaTag(control, "og:title", tags.Title);
            if (!string.IsNullOrEmpty(tags.Description))
                _metaTagService.AddMetaTag(control, "og:description", tags.Description);

            _metaTagService.AddMetaTag(control, "og:locale", "en_US");
            _metaTagService.AddMetaTag(control, "og:site_name", "Providence Presbyterian Church");

            if (tags.Type == "article")
            {
                _metaTagService.AddMetaTag(control, "article:publisher", FacebookSettings.ChurchPageUrl);
                _metaTagService.AddMetaTag(control, "article:author", FacebookSettings.ChurchPageUrl);

                if (!string.IsNullOrEmpty(tags.Section))
                    _metaTagService.AddMetaTag(control, "article:section", tags.Section);
            }
        }
    }
}
