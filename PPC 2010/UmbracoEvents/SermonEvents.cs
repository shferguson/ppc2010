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

namespace PPC_2010.UmbracoEvents
{
    public class SermonEvents : IApplicationEventHandler
    {
        public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            MediaService.Created += new TypedEventHandler<IMediaService, NewEventArgs<IMedia>>(MediaService_Created);
            MediaService.Saving += new TypedEventHandler<IMediaService, SaveEventArgs<IMedia>>(MediaService_Saving);
            MediaService.Saved += new TypedEventHandler<IMediaService, SaveEventArgs<IMedia>>(MediaService_Saved);
            MediaService.Deleted += new TypedEventHandler<IMediaService, DeleteEventArgs<IMedia>>(MediaService_Deleted);
            MediaService.Trashed += new TypedEventHandler<IMediaService, MoveEventArgs<IMedia>>(MediaService_Trashed);
        }

        public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext) { }

        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext) { }

        void MediaService_Created(IMediaService sender, NewEventArgs<IMedia> e)
        {
            if (e.Entity.ContentType.Alias == PPC_2010.Data.Constants.SermonAlias)
            {
                // Automatically populate the sermon title from what the user entered in for the element name
                // The element name will be set in Media_BeforeSave and having the name they type in when
                // creating a new sermon makes more sense to users
                var sermon = new Data.Media.MediaSermon(e.Entity);
                sermon.Title = e.Entity.Name;
                sermon.RecordingDate = DateTime.Today;

                CheckForRefresh(e.Entity, e);
            }
        }

        void MediaService_Saving(IMediaService sender, SaveEventArgs<IMedia> e)
        {
            foreach (IMedia entity in e.SavedEntities)
            {
                if (entity.ContentType.Alias == PPC_2010.Data.Constants.SermonAlias && entity.IsValid())
                {
                    if (CheckForRefresh(entity, e))
                        return;

                    Data.Media.MediaSermon sermon = new Data.Media.MediaSermon(entity);

                    // don't update anything recorded before the launch of the new web site
                    if (sermon.RecordingDate > new DateTime(2011, 2, 28))
                    {
                        // Canonicalize the name of the sermon so they are consistent and easy to find in the list
                        entity.Name = sermon.RecordingDate.Value.ToString("MM/dd/yyyy") + " - " + sermon.RecordingSession;

                        // Cananicalize the file name,
                        if (sermon.RecordingDate > new DateTime(2011, 2, 28))
                        {
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
        }

        void MediaService_Saved(IMediaService sender, SaveEventArgs<IMedia> e)
        {
            foreach (IMedia entity in e.SavedEntities.Where(entity => entity.ContentType.Alias == PPC_2010.Data.Constants.SermonAlias))
            {
                if (entity.ContentType.Alias == PPC_2010.Data.Constants.SermonAlias && entity.IsValid())
                {
                    var sermons = new Data.Media.SermonRepository().LoadAllSermons().Cast<MediaSermon>();

                    int i = 1;
                    foreach (var sermon in sermons)
                    {
                        if (sermon.SortOrder != i)
                        {
                            sermon.SortOrder = i;
                            sender.Save(sermon.Media, 0, false);
                        }

                        i++;
                    }

                    ServiceLocator.Instance.Locate<ISermonRepository>().RefreshSermon(entity.Id, entity.Trashed);
                }
            }
        }

        void MediaService_Deleted(IMediaService sender, DeleteEventArgs<IMedia> e)
        {
            foreach (IMedia entity in e.DeletedEntities.Where(entity => entity.ContentType.Alias == PPC_2010.Data.Constants.SermonAlias))
            {
                ServiceLocator.Instance.Locate<ISermonRepository>().RefreshSermon(entity.Id, true);
            }
        }

        void MediaService_Trashed(IMediaService sender, MoveEventArgs<IMedia> e)
        {
            foreach (IMedia entity in e.MoveInfoCollection.Select(m => m.Entity).Where(entity => entity.ContentType.Alias == PPC_2010.Data.Constants.SermonAlias))
            {
                ServiceLocator.Instance.Locate<ISermonRepository>().RefreshSermon(entity.Id, true);
            }
        }

        bool CheckForRefresh(IMedia sender, CancellableEventArgs e)
        {
            // As of Umbraco 6.2.4 if you try to access e.Cancel for an event that isn't cancellable you'll get an exception
            if (sender.Name.Equals(PPC_2010.Data.Constants.RefreshIndicatorTitle, StringComparison.CurrentCultureIgnoreCase))
            {
                ServiceLocator.Instance.Locate<ISermonRepository>().RefreshSermons();
                return true;
            }

            return false;
        }
    }
}