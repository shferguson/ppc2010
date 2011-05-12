using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPC_2010.Data
{
    public class ScriptureReference
    {
        public string Book { get; set; }
        public int? StartChapter { get; set; }
        public int? StartVerse { get; set; }
        public int? EndChapter { get; set; }
        public int? EndVerse { get; set; }

        public bool Contains(ScriptureReference other)
        {
            if (Book == other.Book)
            {
                return StartChapter.GetValueOrDefault(0) >= other.StartChapter.GetValueOrDefault(0) &&
                       StartVerse.GetValueOrDefault(0) >= other.StartVerse.GetValueOrDefault(0) &&
                       EndChapter.GetValueOrDefault(999) <= other.EndChapter.GetValueOrDefault(999) &&
                       EndVerse.GetValueOrDefault(999) <= other.EndVerse.GetValueOrDefault(999);
            }

            return false;
        }

        public string ScriptureText
        {
            get
            {
                string scriptureText = null;
                if (StartChapter != EndChapter)
                    scriptureText = string.Format("{0} {1}:{2}-{3}:{4}", Book, StartChapter, StartVerse, EndChapter, EndVerse);
                else if (StartChapter != EndVerse)
                    scriptureText = string.Format("{0} {1}:{2}-{3}", Book, StartChapter, StartVerse, EndVerse);
                else
                    scriptureText = string.Format("{0} {1}:{2}", Book, StartChapter, StartVerse);

                return scriptureText;
            }
        }
    }
}