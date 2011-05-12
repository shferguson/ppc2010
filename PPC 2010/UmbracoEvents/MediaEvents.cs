using PPC_2010.Data;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.media;
using System.IO;
using System.Web;
using System;

namespace PPC_2010.UmbracoEvents
{
    public class MediaEvents : ApplicationBase
    {
        public MediaEvents()
        {
            Media.AfterSave += new Media.SaveEventHandler(Media_AfterSave);
            Media.BeforeSave += new Media.SaveEventHandler(Media_BeforeSave);

            SermonRepository.RebuildCache();
        }

        void Media_BeforeSave(Media sender, umbraco.cms.businesslogic.SaveEventArgs e)
        {
            if (sender.ContentType.Alias == SermonRepository.SermonAlias)
            {
                Data.Sermon sermon = new Data.Sermon(sender);

                // don't update anything recorded before the launch of the new web site
                if (sermon.RecordingDate > new DateTime(2011, 2, 28))
                {
                    // Canonicalize the name of the sermon so they are consistent and easy to find in the list
                    sender.Text = sermon.RecordingDate.ToString("MM/dd/yyyy") + " - " + sermon.RecordingSession;

                    // Cananicalize the file name,
                    if (sermon.RecordingDate > new DateTime(2011, 2, 28))
                    {
                        var fileProperty = sender.getProperty("audioFile");
                        string oldFileName = fileProperty.Value.ToString();

                        if (!string.IsNullOrWhiteSpace(oldFileName) && File.Exists(HttpContext.Current.Server.MapPath(oldFileName)))
                        {
                            string newFileName = Path.GetDirectoryName(oldFileName) + "/" + sender.Text.Replace("/", "-") + Path.GetExtension(oldFileName);
                            newFileName = newFileName.Replace("\\", "/").Replace(" ", "");

                            if (oldFileName != newFileName)
                            {
                                fileProperty.Value = newFileName;

                                if (File.Exists(HttpContext.Current.Server.MapPath(newFileName)))
                                    File.Delete(HttpContext.Current.Server.MapPath(newFileName));

                                File.Move(HttpContext.Current.Server.MapPath(oldFileName), HttpContext.Current.Server.MapPath(newFileName));

                            }
                        }
                    }
                }
            }
        }

        void Media_AfterSave(Media sender, umbraco.cms.businesslogic.SaveEventArgs e)
        {
            if (sender.ContentType.Alias == SermonRepository.SermonAlias)
                SermonRepository.RebuildCache();
        }
    }
}