namespace PPC_2010.Data
{
    public interface IEmailGroupRepository
    {
        IEmailGroup GetEmailGroupByName(string name);
    }
}