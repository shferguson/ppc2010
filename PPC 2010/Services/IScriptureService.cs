using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PPC_2010.Data;

namespace PPC_2010.Services
{
    public interface IScriptureService
    {
        string GetScriptureTextHtml(ScriptureReferences scriptureReferences);
    }
}
