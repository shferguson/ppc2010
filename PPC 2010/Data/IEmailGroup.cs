using System.Collections.Generic;

namespace PPC_2010.Data
{
    public interface IEmailGroup
    {
        string Name { get; }
        IList<string> EmailAddresses { get; }
    }
}
