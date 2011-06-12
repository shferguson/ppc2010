using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using umbraco.cms.businesslogic.media;
using System.Threading;

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

    public class SermonRepository
    {
        public const string SermonFolderAlias = "SermonFolder";
        public const string SermonAlias = "Sermon";

        public Sermon LoadCurrentSermon(string recordingSession)
        {
            IEnumerable<Sermon> sermons = LoadAllSermons();

            Sermon sermon = sermons.FirstOrDefault(s => s.RecordingSession == recordingSession);

            // Reload the sermon so we don't run into caching issues
            return LoadSermon(sermon.Id);
        }

        public Sermon LoadSermon(int sermonId)
        {
            Media media = new Media(sermonId);
            if (media != null)
                return SermonFromMedia(media);
            return null; 
        }

        public IEnumerable<Sermon> LoadLastSermons(int count)
        {
            IEnumerable<Sermon> sermons = LoadAllSermons();

            return sermons.Take(count);
        }

        public IEnumerable<Sermon> LoadAllSermons()
        {
            return GetSermons();
        }

        public int GetNumberOfSermons()
        {
            return GetSermons().Count();
        }

        private static readonly object cacheLock = new object();
        private static Dictionary<int, Sermon> sermonCache 
        {
            get
            {
                Dictionary<int, Sermon> cache = HttpContext.Current.Application["SermonCache"] as Dictionary<int, Sermon>;
                if (cache == null)
                    cache = new Dictionary<int, Sermon>();
                return cache;
            }
            set
            {
                HttpContext.Current.Application["SermonCache"] = value;
            }
        }

        private static IEnumerable<Sermon> GetSermons()
        {
            Media sermonRoot = Media.GetRootMedias().FirstOrDefault(m => m != null && m.ContentType != null && m.ContentType.Alias == SermonFolderAlias);

            lock (sermonCache)
            {
                if (sermonRoot.ChildCount != sermonCache.Count)
                {
                    RebuildCache();
                }

                return sermonCache.Values.AsEnumerable<Sermon>()
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
                    sermonCache[media.Id] = new Sermon(media);
            }
        }

        public static void InvalidateCache()
        {
            lock (cacheLock)
            {
                sermonCache = new Dictionary<int, Sermon>();
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


        private static Sermon SermonFromMedia(Media media)
        {
            Sermon sermon = null;
            lock (cacheLock)
            {
                if (sermonCache.ContainsKey(media.Id))
                {
                    sermon = sermonCache[media.Id];
                }
                else
                {
                    sermon = new Sermon(media);
                    sermonCache[media.Id] = sermon;
                }
            }

            return sermon;
        }

        public static void OrderSermons()
        {
            IEnumerable<Sermon> sermons = GetSermons();

            int i = 1;
            foreach (var sermon in sermons)
            {
                if (sermon.SortOrder != i)
                {
                    sermon.SortOrder = i;
                }
                
                i++;
            }
        }
    }
}