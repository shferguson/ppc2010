using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;

namespace PPC_2010.Data.Media
{
    public class EmailGroup : IEmailGroup
    {
        private IMedia _media = null;

        public EmailGroup(IMedia media)
        {
            this._media = media;
        }

        public string Name { get { return _media.Name; } }

        public IList<string> EmailAddresses
        {
            get
            {
                return (_media.GetValue<string>("emailAddresses") ?? "")
                    .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(a => a.Trim())
                    .ToList();
            }
        }
    }
}