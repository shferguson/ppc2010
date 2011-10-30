using System.Linq;
using umbraco.cms.businesslogic.media;

namespace PPC_2010.Data
{
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