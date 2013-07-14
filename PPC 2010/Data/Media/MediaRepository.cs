using System.Linq;
using Umbraco.Core.Services;
using Umbraco.Core.Models;

namespace PPC_2010.Data.Media
{
    public class MediaRepository
    {
        public IMedia GetMediaByAliasPath(string aliasPath)
        {
            string[] parts = aliasPath.Split('/');

            var mediaService = ServiceLocator.Instance.Locate<IMediaService>();

            var currentList = mediaService.GetRootMedia();

            IMedia currentMedia = null;
            foreach (var alias in parts)
            {
                if (currentList != null)
                {
                    currentMedia = currentList.FirstOrDefault(m => m.Name == alias);
                    if (currentMedia != null)
                        currentList = currentMedia.Children();
                }
            }

            return currentMedia;
        }
    }
}