using System.Web.UI;

namespace PPC_2010.Social.Twitter
{
    interface ITwitterTagService
    {
        void AddTwitterCardTags(UserControl userControl, string cardType);
    }
}