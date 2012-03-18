using System.Linq;

namespace PPC_2010.Data.Media
{
    using umbraco.cms.businesslogic.media;

    public class MediaRepository
    {
        public Media GetMediaByAliasPath(string aliasPath)
        {
            string[] parts = aliasPath.Split('/');
           
            Media currentMedia = null;
            Media[] currentList = Media.GetRootMedias();

            foreach (var alias in parts)
            {
                if (currentList != null)
                {
                    currentMedia = currentList.FirstOrDefault(m => m.Text == alias);
                    if (currentMedia != null)
                        currentList = currentMedia.Children;
                }
            }

            return currentMedia;
        }
    }
}