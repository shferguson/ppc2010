using System;
using System.IO;
using System.Linq;
using System.Web;
using PPC_2010.Data;
using PPC_2010.Data.Media;
using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using PPC_2010.Services;

namespace PPC_2010.UmbracoEvents
{
    public class SermonEvents : IApplicationEventHandler
    {
        public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            MediaService.Saving += new TypedEventHandler<IMediaService, SaveEventArgs<IMedia>>(MediaService_Saving);
            MediaService.Saved += new TypedEventHandler<IMediaService, SaveEventArgs<IMedia>>(MediaService_Saved);
            MediaService.Deleted += new TypedEventHandler<IMediaService, DeleteEventArgs<IMedia>>(MediaService_Deleted);
            MediaService.Trashed += new TypedEventHandler<IMediaService, MoveEventArgs<IMedia>>(MediaService_Trashed);
        }

        public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext) { }

        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext) { }

        void MediaService_Saving(IMediaService sender, SaveEventArgs<IMedia> e)
        {
            foreach (IMedia entity in e.SavedEntities)
            {
                if (entity.ContentType.Alias == PPC_2010.Data.Constants.SermonAlias)
                {
                    if (CheckForRefresh(entity, e))
                        return;

                    var sermon = new Data.Media.MediaSermon(entity);
                    if (!entity.HasIdentity)
                    {
                        // Automatically populate the sermon title from what the user entered in for the element name
                        // The element name will be set in Media_BeforeSave and having the name they type in when
                        // creating a new sermon makes more sense to users
                        sermon.Title = entity.Name;
                        sermon.RecordingDate = DateTime.Today;
                    }
                    else if (entity.IsValid() && sermon.RecordingDate > new DateTime(2011, 2, 28)) // don't update anything recorded before the launch of the new web site
                    {
                        // Canonicalize the name of the sermon so they are consistent and easy to find in the list
                        entity.Name = sermon.RecordingDate.Value.ToString("MM/dd/yyyy") + " - " + sermon.RecordingSession;

                        string oldFileName = entity.GetValue<string>("audioFile"); ;
                        if (!string.IsNullOrWhiteSpace(oldFileName) && System.IO.File.Exists(HttpContext.Current.Server.MapPath(oldFileName)))
                        {
                            string newFileName = Path.GetDirectoryName(oldFileName) + "/" + entity.Name.Replace("/", "-") + Path.GetExtension(oldFileName);
                            newFileName = newFileName.Replace("\\", "/").Replace(" ", "");

                            if (oldFileName != newFileName)
                            {
                                entity.SetValue("audioFile", newFileName);

                                if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(newFileName)))
                                    System.IO.File.Delete(HttpContext.Current.Server.MapPath(newFileName));

                                System.IO.File.Move(HttpContext.Current.Server.MapPath(oldFileName), HttpContext.Current.Server.MapPath(newFileName));
                            }
                        }
                    }
                }
            }
        }

        void MediaService_Saved(IMediaService sender, SaveEventArgs<IMedia> e)
        {
            foreach (IMedia entity in e.SavedEntities.Where(entity => entity.ContentType.Alias == PPC_2010.Data.Constants.SermonAlias))
            {
                if (entity.ContentType.Alias == PPC_2010.Data.Constants.SermonAlias && entity.IsValid())
                {
                    var repository = ServiceLocater.Instance.Locate<ISermonRepository>();

                    repository.RefreshSermon(entity.Id, entity.Trashed);
                    repository.UpdateSermonSort();
                    var mediaSermon = new MediaSermon(entity);
                    ServiceLocater.Instance.Locate<SermonPublishApi>().Update(mediaSermon);
                    ServiceLocater.Instance.Locate<IMp3FileService>().SetMp3FileTags(mediaSermon);
                }
            }
        }

        void MediaService_Deleted(IMediaService sender, DeleteEventArgs<IMedia> e)
        {
            foreach (IMedia entity in e.DeletedEntities.Where(entity => entity.ContentType.Alias == PPC_2010.Data.Constants.SermonAlias))
            {
                ServiceLocater.Instance.Locate<ISermonRepository>().RefreshSermon(entity.Id, true);

                ServiceLocater.Instance.Locate<SermonPublishApi>().Delete(entity.Id);
            }
        }

        void MediaService_Trashed(IMediaService sender, MoveEventArgs<IMedia> e)
        {
            foreach (IMedia entity in e.MoveInfoCollection.Select(m => m.Entity).Where(entity => entity.ContentType.Alias == PPC_2010.Data.Constants.SermonAlias))
            {
                ServiceLocater.Instance.Locate<ISermonRepository>().RefreshSermon(entity.Id, true);
            }
        }

        bool CheckForRefresh(IMedia sender, CancellableEventArgs e)
        {
            // As of Umbraco 6.2.4 if you try to access e.Cancel for an event that isn't cancellable you'll get an exception
            if (sender.Name.Equals(PPC_2010.Data.Constants.RefreshIndicatorTitle, StringComparison.CurrentCultureIgnoreCase))
            {
                ServiceLocater.Instance.Locate<ISermonRepository>().RefreshSermons();
                if (e.CanCancel)
                    e.Cancel = true;
                return true;
            }

            return false;
        }
    }
}