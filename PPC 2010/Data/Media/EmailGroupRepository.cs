namespace PPC_2010.Data.Media
{
    public class EmailGroupRepository : IEmailGroupRepository
    {
        public const string EmailGroupFolderAlias = "Email Groups";

        public IEmailGroup GetEmailGroupByName(string name)
        {
            var repository = new MediaRepository();
            var media = repository.GetMediaByAliasPath(string.Format("{0}/{1}", EmailGroupFolderAlias, name));
            if (media != null)
                return new EmailGroup(media);
            return null;
        }
    }
}