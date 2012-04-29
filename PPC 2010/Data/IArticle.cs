using System;

namespace PPC_2010.Data
{
    public interface IArticle
    {
        int Id { get; }
        string Title { get; }
        DateTime? Date { get; }
        string ScriptureReference { get; }
        string Text { get; }
    }
}