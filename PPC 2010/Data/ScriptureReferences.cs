using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace PPC_2010.Data
{
    /// <summary>
    /// Represents a scriptrue reference
    /// </summary>
    public class ScriptureReferences
    {
        private List<ScriptureReference> references = new List<ScriptureReference>();

        public IEnumerable<ScriptureReference> References { get { return references; } }

        /// <summary>
        /// Construct from a verse list
        /// </summary>
        public ScriptureReferences(string book, int startChapter, int startVerse, int endChapter, int endVerse)
        {
            if (startChapter != endChapter)
                ScriptureString = string.Format("{0} {1}:{2}-{3}:{4}", book, startChapter, startVerse, endChapter, endVerse);
            else if (startChapter != endVerse)
                ScriptureString = string.Format("{0} {1}:{2}-{3}", book, startChapter, startVerse, endVerse);
            else if (!string.IsNullOrWhiteSpace(book))
                ScriptureString = string.Format("{0} {1}:{2}", book, startChapter, startVerse);
            else
                ScriptureString = string.Empty;

            references.Add(new ScriptureReference()
            {
                Book = book,
                StartChapter = startChapter,
                StartVerse = startVerse,
                EndChapter = endChapter,
                EndVerse = endVerse
            });
        }

        /// <summary>
        /// Construct from a scripture string. e.g. 1
        /// </summary>
        /// <param name="scriptureString"></param>
        public ScriptureReferences(string scriptureString)
        {
            this.ScriptureString = scriptureString;
        }

        public string ScriptureString { get; private set; }

        public bool ContainsReference(ScriptureReference reference)
        {
            return references.Any(r => r.Contains(reference));
        }

        private static Regex scriptureReference = new Regex(@"(/w/+/s*)([/d:,]*)", RegexOptions.Compiled);
       
        private static IEnumerable<ScriptureReference> Parse(string sermonString)
        {
            //foreach (Match m in scriptureReference.Matches(sermonString))
            //{
            //    string book = m.Captures[1].Value;
            //    string chapterAndVerse = m.Captures[2].Value;

            //    int chapter;
            //    int verse;

            //    string current = string.Empty;
            //    bool onChapter = true;

            //    foreach (char c in chapterAndVerse)
            //    {

                    
                    
                    
            //    }
            //}

            return null; 
        }

        public bool HasReference { get { return !string.IsNullOrWhiteSpace(ScriptureString); } }

        public override string ToString()
        {
            return ScriptureString;
        }

        public override int GetHashCode()
        {
            return ScriptureString.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            ScriptureReferences other = obj as ScriptureReferences;
            if (other == null)
                return false;
            return ScriptureString.Equals(other.ScriptureString);
        }
    }
}