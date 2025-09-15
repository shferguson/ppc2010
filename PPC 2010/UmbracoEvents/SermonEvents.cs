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
using System.Collections.Generic;
using PPC_2010.Extensions;

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

                        bool fileChanged = false;

                        if (!System.IO.File.Exists(HttpContext.Current.Server.MapPath(sermon.AudioFile)))
                            sermon.AudioFile = null;

                        string oldFileName = sermon.AudioFile;

                        if (!string.IsNullOrWhiteSpace(oldFileName))
                        {
                            string newFileName = Path.GetDirectoryName(oldFileName) + "/" + entity.Name.Replace("/", "-") + Path.GetExtension(oldFileName);
                            newFileName = newFileName.Replace("\\", "/").Replace(" ", "");

                            if (oldFileName != newFileName)
                            {
                                fileChanged = true;
                                sermon.AudioFile = newFileName;

                                if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(newFileName)))
                                    System.IO.File.Delete(HttpContext.Current.Server.MapPath(newFileName));

                                System.IO.File.Move(HttpContext.Current.Server.MapPath(oldFileName), HttpContext.Current.Server.MapPath(newFileName));
                            }
                        }

                        if (sermon.RecordingDate > new DateTime(2023, 10, 1)) // Don't send anything old to SermonAudio
                        {
                            var sermonAudioApi = ServiceLocater.Instance.Locate<ISermonAudioApi>();
                            var filePath = HttpContext.Current.Server.MapPath(sermon.AudioFile);
                            var podSermon = PodSermon.Clone(sermon);

                            try
                            {
                                if (string.IsNullOrEmpty(sermon.SermonAudioId))
                                {
                                    var sermonId = System.Threading.Tasks.Task.Run(async () => await sermonAudioApi.Create(podSermon)).Result;
                                    if (sermonId != null)
                                        sermon.SermonAudioId = sermonId;
                                }
                                else
                                {
                                    System.Threading.Tasks.Task.Run(() => sermonAudioApi.Update(sermon.SermonAudioId, podSermon)).Wait();
                                }
                            }
                            catch (Exception ex) 
                            {
                                Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(ex, HttpContext.Current));
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
                    repository.UpdateSermonSort(entity);

                    var mediaSermon = new MediaSermon(entity);
                    ServiceLocater.Instance.Locate<ISermonPublishApi>().Update(mediaSermon);

                    var filePath = HttpContext.Current.Server.MapPath(mediaSermon.AudioFile);

                    // Was the audio file updated?
                    // Assume it was if it was written to in the last 5 minutes
                    if (System.IO.File.GetLastWriteTime(filePath) > DateTime.Now.AddMinutes(-5))
                    {
                        ServiceLocater.Instance.Locate<IMp3FileService>().SetMp3FileTags(mediaSermon);

                        if (!string.IsNullOrEmpty(mediaSermon.SermonAudioId))
                        {
                            var sermonAudioApi = ServiceLocater.Instance.Locate<ISermonAudioApi>();
                            var sermonAudioId = mediaSermon.SermonAudioId;
                            TaskExtensions.RunInBackground(() => sermonAudioApi.UploadFile(sermonAudioId, filePath));
                        }
                    }
                }
            }
        }

        void MediaService_Deleted(IMediaService sender, DeleteEventArgs<IMedia> e)
        {
            SermonsDeleted(e.DeletedEntities.Where(entity => entity.ContentType.Alias == PPC_2010.Data.Constants.SermonAlias));
        }

        void MediaService_Trashed(IMediaService sender, MoveEventArgs<IMedia> e)
        {
            SermonsDeleted(e.MoveInfoCollection.Select(m => m.Entity).Where(entity => entity.ContentType.Alias == PPC_2010.Data.Constants.SermonAlias));
        }

        void SermonsDeleted(IEnumerable<IMedia> entities)
        {
            foreach (IMedia entity in entities)
            {
                ServiceLocater.Instance.Locate<ISermonRepository>().RefreshSermon(entity.Id, true);
                ServiceLocater.Instance.Locate<ISermonPublishApi>().Delete(entity.Id);

                var mediaSermon = new MediaSermon(entity);
                if (!string.IsNullOrEmpty(mediaSermon.SermonAudioId))
                {
                    var sermonAudioId = mediaSermon.SermonAudioId; 
                    TaskExtensions.RunInBackground(() => ServiceLocater.Instance.Locate<ISermonAudioApi>().Delete(sermonAudioId));
                }
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