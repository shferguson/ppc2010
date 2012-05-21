﻿using PPC_2010.Data;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.media;
using System.IO;
using System.Web;
using System;
using PPC_2010.Data.Media;
using System.Collections.Generic;
using System.Linq;

namespace PPC_2010.UmbracoEvents
{
    public class SermonEvents : ApplicationBase
    {
        public SermonEvents()
        {
            Media.New += new Media.NewEventHandler(Media_New);
            Media.AfterSave += new Media.SaveEventHandler(Media_AfterSave);
            Media.BeforeSave += new Media.SaveEventHandler(Media_BeforeSave);

            //SermonMediaRepository.RebuildCache();
        }

        void Media_New(Media sender, umbraco.cms.businesslogic.NewEventArgs e)
        {
            if (sender.ContentType.Alias == SermonMediaRepository.SermonAlias)
            {
                // Automatically populate the sermon title from what the user entered in for the element name
                // The element name will be set in Media_BeforeSave and having the name they type in when
                // creating a new sermon makes more sense to users
                var sermon = new Data.Media.MediaSermon(sender);
                sermon.Title = sender.Text;
                sermon.RecordingDate = DateTime.Today;
            }
        }

        void Media_BeforeSave(Media sender, umbraco.cms.businesslogic.SaveEventArgs e)
        {
            if (sender.ContentType.Alias == SermonMediaRepository.SermonAlias)
            {
                Data.Media.MediaSermon sermon = new Data.Media.MediaSermon(sender);

                // don't update anything recorded before the launch of the new web site
                if (sermon.RecordingDate > new DateTime(2011, 2, 28))
                {
                    // Canonicalize the name of the sermon so they are consistent and easy to find in the list
                    sender.Text = sermon.RecordingDate.Value.ToString("MM/dd/yyyy") + " - " + sermon.RecordingSession;

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
            if (sender.ContentType.Alias == SermonMediaRepository.SermonAlias)
            {
                var sermons = new Data.Media.SermonMediaRepository().LoadAllSermons().Cast<MediaSermon>();

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
}