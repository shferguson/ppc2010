using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using umbraco.cms.businesslogic.media;
using System.Threading;
using System.Configuration;
using PPC_2010.Data.LinqToSql;

namespace PPC_2010.Data
{
    #region Sermon Query

    // Here is the SQL query to use to get all the sermons:

    /*
        select
        c.nodeId as sermonID,
        n.text as umbracoTitle,
        recordingDate, recordingDate,
        title.title, 
        speakerName.speakerName,
        recordingSession.recordingSession,
        sermonSeries.sermonSeries,
        book.book,
        startChapter.startChapter,
        startVerse.startVerse,
        endChapter.endChapter,
        endVerse.endVerse,
        scriptureReferenceText.scriptureReferenceText,
        audioFile.audioFile
        from cmsContent c
        inner join cmsContentType ct on ct.alias = 'Sermon' and c.contentType = ct.nodeId
        inner join umbracoNode n on n.id = c.nodeId and n.trashed = 0
        left outer join (select d.contentNodeId, d.dataNVarChar as title from cmsPropertyData d, cmsPropertyType pt where pt.id = d.propertytypeId and pt.alias = 'title') title on title.contentnodeid = c.nodeId
        left outer join (select d.contentNodeId, d.dataDate as recordingDate from cmsPropertyData d, cmsPropertyType pt where pt.id = d.propertytypeId and pt.alias = 'recordingDate') recordingDate on recordingDate.contentnodeid = c.nodeId
        left outer join (select d.contentNodeId, pv.Value as speakerTitle from cmsPropertyData d, cmsPropertyType pt, cmsDataTypePreValues pv where pt.id = d.propertytypeId and pt.dataTypeId = pv.dataTypeNodeId and cast(cast(d.dataNVarChar as nvarchar(10)) as int) = pv.id and pt.alias = 'speakerTitle') speakerTitle on speakerTitle.contentnodeid = c.nodeId
        left outer join (select d.contentNodeId, pv.Value as speakerName from cmsPropertyData d, cmsPropertyType pt, cmsDataTypePreValues pv where pt.id = d.propertytypeId and pt.dataTypeId = pv.dataTypeNodeId and cast(cast(d.dataNText as nvarchar(10)) as int) = pv.id and pt.alias = 'speakerName') speakerName on speakerName.contentnodeid = c.nodeId
        left outer join (select d.contentNodeId, pv.Value as recordingSession from cmsPropertyData d, cmsPropertyType pt, cmsDataTypePreValues pv where pt.id = d.propertytypeId and pt.dataTypeId = pv.dataTypeNodeId and cast(cast(d.dataNText as nvarchar(10)) as int) = pv.id and pt.alias = 'recordingSession') recordingSession on recordingSession.contentnodeid = c.nodeId
        left outer join (select d.contentNodeId, pv.Value as sermonSeries from cmsPropertyData d, cmsPropertyType pt, cmsDataTypePreValues pv where pt.id = d.propertytypeId and pt.dataTypeId = pv.dataTypeNodeId and cast(cast(d.dataNText as nvarchar(10)) as int) = pv.id and pt.alias = 'sermonSeries') sermonSeries on sermonSeries.contentnodeid = c.nodeId
        left outer join (select d.contentNodeId, pv.Value as book from cmsPropertyData d, cmsPropertyType pt, cmsDataTypePreValues pv where pt.id = d.propertytypeId and pt.dataTypeId = pv.dataTypeNodeId and cast(cast(d.dataNVarChar as nvarchar(10)) as int) = pv.id and pt.alias = 'book') book on book.contentnodeid = c.nodeId
        left outer join (select d.contentNodeId, d.dataInt as startChapter from cmsPropertyData d, cmsPropertyType pt where pt.id = d.propertytypeId and pt.alias = 'startChapter') startChapter on startChapter.contentnodeid = c.nodeId
        left outer join (select d.contentNodeId, d.dataInt as startVerse from cmsPropertyData d, cmsPropertyType pt where pt.id = d.propertytypeId and pt.alias = 'startVerse') startVerse on startVerse.contentnodeid = c.nodeId
        left outer join (select d.contentNodeId, d.dataInt as endChapter from cmsPropertyData d, cmsPropertyType pt where pt.id = d.propertytypeId and pt.alias = 'endChapter') endChapter on endChapter.contentnodeid = c.nodeId
        left outer join (select d.contentNodeId, d.dataInt as endVerse from cmsPropertyData d, cmsPropertyType pt where pt.id = d.propertytypeId and pt.alias = 'endVerse') endVerse on endVerse.contentnodeid = c.nodeId
        left outer join (select d.contentNodeId, d.dataNVarChar as scriptureReferenceText from cmsPropertyData d, cmsPropertyType pt where pt.id = d.propertytypeId and pt.alias = 'scriptureReferenceText') scriptureReferenceText on scriptureReferenceText.contentnodeid = c.nodeId
        left outer  join (select d.contentNodeId, d.dataNVarChar as audioFile from cmsPropertyData d, cmsPropertyType pt where pt.id = d.propertytypeId and pt.alias = 'audioFile') audioFile on audioFile.contentnodeid = c.nodeId
        order by recordingDate.recordingDate
   
    */
    #endregion

    public interface ISermonRepository: IDisposable
    {
        ISermon LoadCurrentSermon(string recordingSession);
        ISermon LoadSermon(int sermonId);
        IEnumerable<ISermon> LoadLastSermons(int count);
        IEnumerable<ISermon> LoadSermonsByPage(int pageNumber, int itemsPerPage);
        IEnumerable<ISermon> LoadAllSermons();
        int GetNumberOfSermons();
    }

    public class SermonLinqToSqlRepository : ISermonRepository
    {
        ProvidenceDbDataContext providence = new ProvidenceDbDataContext(ConfigurationManager.AppSettings["umbracoDbDSN"]);

        public ISermon LoadCurrentSermon(string recordingSession)
        {
            return providence.Sermons
                .Where(s => s.RecordingSession == recordingSession)
                .OrderByDescending(s => s.RecordingDate)
                .ThenByDescending(s => s.RecordingSession)
                .Take(1)
                .FirstOrDefault();
        }

        public ISermon LoadSermon(int sermonId)
        {
            return providence
                .Sermons
                .Single(s => ((LinqToSql.Sermon)s).Id == sermonId);
        }

        public IEnumerable<ISermon> LoadLastSermons(int count)
        {
            return providence.Sermons
                .OrderByDescending(s => s.RecordingDate)
                .ThenByDescending(s => s.RecordingSession)
                .Take(count);
        }

        public IEnumerable<ISermon> LoadSermonsByPage(int pageNumber, int itemsPerPage)
        {
            return providence.Sermons
                .OrderByDescending(s => s.RecordingDate)
                .ThenByDescending(s => s.RecordingSession)
                .Skip((pageNumber-1) * itemsPerPage)
                .Take(itemsPerPage);
        }

        public IEnumerable<ISermon> LoadAllSermons()
        {
            return providence.Sermons
                .OrderByDescending(s => s.RecordingDate)
                .ThenByDescending(s => s.RecordingSession);
        }

        public int GetNumberOfSermons()
        {
            return providence.Sermons.Count();
        }

        public void Dispose()
        {
            providence.Dispose();
        }
    }

    public class SermonMediaRepository : ISermonRepository
    {
        public const string SermonFolderAlias = "SermonFolder";
        public const string SermonAlias = "Sermon";

        public ISermon LoadCurrentSermon(string recordingSession)
        {
            IEnumerable<ISermon> sermons = LoadAllSermons();

            ISermon sermon = sermons.FirstOrDefault(s => s.RecordingSession == recordingSession);

            // Reload the sermon so we don't run into caching issues
            return LoadSermon(sermon.Id);
        }

        public ISermon LoadSermon(int sermonId)
        {
            Media media = new Media(sermonId);
            if (media != null)
                return SermonFromMedia(media);
            return null; 
        }

        public IEnumerable<ISermon> LoadLastSermons(int count)
        {
            IEnumerable<ISermon> sermons = LoadAllSermons();

            return sermons.Take(count);
        }

        public IEnumerable<ISermon> LoadSermonsByPage(int pageNumber, int itemsPerPage)
        {
            var sermons = LoadAllSermons();
            return sermons.Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage);
        }

        public IEnumerable<ISermon> LoadAllSermons()
        {
            return GetSermons();
        }

        public int GetNumberOfSermons()
        {
            return GetSermons().Count();
        }

        public void Dispose() { }

        private static readonly object cacheLock = new object();
        private static Dictionary<int, SermonFromMedia> sermonCache 
        {
            get
            {
                Dictionary<int, SermonFromMedia> cache = HttpContext.Current.Application["SermonCache"] as Dictionary<int, SermonFromMedia>;
                if (cache == null)
                    cache = new Dictionary<int, SermonFromMedia>();
                return cache;
            }
            set
            {
                HttpContext.Current.Application["SermonCache"] = value;
            }
        }

        private static IEnumerable<SermonFromMedia> GetSermons()
        {
            Media sermonRoot = Media.GetRootMedias().FirstOrDefault(m => m != null && m.ContentType != null && m.ContentType.Alias == SermonFolderAlias);

            lock (sermonCache)
            {
                if (sermonRoot.ChildCount != sermonCache.Count)
                {
                    RebuildCache();
                }

                return sermonCache.Values.AsEnumerable<SermonFromMedia>()
                    .OrderByDescending(s => s.RecordingDate)
                    .ThenByDescending(s => s.RecordingSession)
                    .ToList();
            }
        }

        public static void UpdateCache(Media media)
        {
            lock (cacheLock)
            {
                if (media.ContentType.Alias == SermonAlias)
                    sermonCache[media.Id] = new SermonFromMedia(media);
            }
        }

        public static void InvalidateCache()
        {
            lock (cacheLock)
            {
                sermonCache = new Dictionary<int, SermonFromMedia>();
            }
        }

        public static void RebuildCache()
        {
            lock (cacheLock)
            {
                InvalidateCache();
                Media sermonRoot = Media.GetRootMedias().FirstOrDefault(m => m != null && m.ContentType != null && m.ContentType.Alias == SermonFolderAlias);
                if (sermonRoot != null)
                    sermonRoot.Children.Select(m => SermonFromMedia(m)).ToArray();
            }
        }


        private static SermonFromMedia SermonFromMedia(Media media)
        {
            SermonFromMedia sermon = null;
            lock (cacheLock)
            {
                if (sermonCache.ContainsKey(media.Id))
                {
                    sermon = sermonCache[media.Id];
                }
                else
                {
                    sermon = new SermonFromMedia(media);
                    sermonCache[media.Id] = sermon;
                }
            }

            return sermon;
        }

        public static void OrderSermons()
        {
            IEnumerable<ISermon> sermons = GetSermons();

            int i = 1;
            foreach (var sermon in sermons)
            {
                if (sermon.SortOrder != i)
                {
                   // sermon.SortOrder = i;
                }
                
                i++;
            }
        }
    }
}