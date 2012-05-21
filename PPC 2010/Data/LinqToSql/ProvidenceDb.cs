namespace PPC_2010.Data.LinqToSql
{
    partial class Sermon : PPC_2010.Data.Sermon
    {
        public override int SortOrder
        {
            get { return -1; }
            set { }
        }


        public override string SpeakerTitle
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        protected override string GetUrl()
        {
            return AudioFile;
        }
    }

    partial class Article : IArticle
    {
    }

    partial class Prevalue : IPrevalue
    {
    }
}
