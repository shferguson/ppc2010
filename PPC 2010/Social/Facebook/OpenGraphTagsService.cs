using PPC_2010.Services;
using PPC_2010.TimeZone;
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

            _metaTagService.AddMetaTag(control, "og:image", UrlService.MakeFullUrl(FacebookSettings.Logo));
            _metaTagService.AddMetaTag(control, "og:image:width", "300");
            _metaTagService.AddMetaTag(control, "og:image:height", "300");
            _metaTagService.AddMetaTag(control, "og:image:type", "image/jpeg");

            _metaTagService.AddMetaTag(control, "og:locale", "en_US");
            _metaTagService.AddMetaTag(control, "og:site_name", "Providence Presbyterian Church");

            if (tags.Type == "article")
            {
                _metaTagService.AddMetaTag(control, "article:publisher", FacebookSettings.ChurchPageUrl);
                _metaTagService.AddMetaTag(control, "article:author", FacebookSettings.ChurchPageUrl);

                if (tags.Date.HasValue)
                    _metaTagService.AddMetaTag(control, "article:published_time", tags.Date.Value.ToUniversalTime().ToString("o"));
                if (tags.ExpirationDate.HasValue)
                    _metaTagService.AddMetaTag(control, "article:expiration_time", tags.ExpirationDate.Value.ToUniversalTime().ToString("o"));

                if (!string.IsNullOrEmpty(tags.Section))
                    _metaTagService.AddMetaTag(control, "article:section", tags.Section);
            }
        }
    }
}
