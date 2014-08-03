using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using PPC_2010.Data;
using PPC_2010.Data.Media;
using System.Text;


namespace PPC_2010.Services
{
    public interface IMp3FileService
    {
        void SetMp3FileTags(MediaSermon sermon);
        void SetMp3FileTags(string fileName, ISermon sermon);
        void SetDuration(MediaSermon sermon, bool save = false);
        void CanonicalizeFileName(MediaSermon sermon, bool save = false);
    }

    public class Mp3FileService : IMp3FileService
    {
        public void SetMp3FileTags(MediaSermon sermon)
        {
            SetMp3FileTags(sermon.Media.getProperty("audioFile").Value as string, sermon);
        }

        public void SetMp3FileTags(string fileName, ISermon sermon)
        {
            if (!string.IsNullOrEmpty(fileName))
                return;

            if (sermon.RecordingDate.HasValue)
            {
                var mp3File = TagLib.File.Create(HttpContext.Current.Server.MapPath(fileName));

                mp3File.Tag.Clear();
                mp3File.Tag.Album = sermon.RecordingSession;
                mp3File.Tag.AlbumArtists = new[] { sermon.SpeakerName };
                mp3File.Tag.Comment = "Providence Presbyterian Church - " + sermon.RecordingDate.Value.ToShortDateString() + " - " + sermon.RecordingSession;
                mp3File.Tag.Genres = new[] { "Speech" };
                mp3File.Tag.Title = sermon.Title;
                mp3File.Tag.Year = (uint)sermon.RecordingDate.Value.Year;
                mp3File.Tag.Copyright = "Providence Presbyterian Church - " + sermon.RecordingDate.Value.Year;
                
                var id3v2Tags = (TagLib.Id3v2.Tag)mp3File.GetTag(TagLib.TagTypes.Id3v2);

                // The 'Official artist/performer webpage' frame is a URL pointing at the artists official webpage.
                id3v2Tags.RemoveFrames("WOAR");
                var frame = new TagLib.Id3v2.UnknownFrame("WOAR");
                frame.Data = Encoding.ASCII.GetBytes("www.providence-pca.net");
                id3v2Tags.AddFrame(frame);

                // The 'Official audio file webpage' frame is a URL pointing at a file specific webpage.
                id3v2Tags.RemoveFrames("WOAF");
                frame = new TagLib.Id3v2.UnknownFrame("WOAF");
                frame.Data = Encoding.ASCII.GetBytes(sermon.SermonUrl);
                id3v2Tags.AddFrame(frame);

                // Sub title
                id3v2Tags.RemoveFrames("TIT3");
                var textFrame = new TagLib.Id3v2.TextInformationFrame("TIT3", TagLib.StringType.Latin1);
                textFrame.Text = new[] { sermon.ScriptureReference.ToString() };
                id3v2Tags.AddFrame(textFrame);
            }
        }

        public void SetDuration(MediaSermon sermon, bool save = false)
        {
            var fileName = sermon.Media.getProperty("audioFile").Value as string;

            if (string.IsNullOrEmpty(fileName))
                return;

            try
            {
                var mp3File = TagLib.File.Create(HttpContext.Current.Server.MapPath(fileName));

                var id3v2Tags = (TagLib.Id3v2.Tag)mp3File.GetTag(TagLib.TagTypes.Id3v2);
                var lenFrame = id3v2Tags.GetFrames<TagLib.Id3v2.TextInformationFrame>("TLEN").FirstOrDefault();
                if (lenFrame != null)
                {
                    int lenghtMs;
                    if (int.TryParse(lenFrame.Text[0], out lenghtMs))
                    {
                        sermon.Duration = TimeSpan.FromMilliseconds(lenghtMs);
                    }
                }
                else
                {
                    sermon.Duration = mp3File.Properties.Duration;
                }

                if (save)
                    sermon.Media.Save();
            }
            catch (Exception ex)
            {
                throw new Exception("Error setting duration on " + sermon.Media.Text + " - " + fileName, ex);
            }
        }

        public void CanonicalizeFileName(MediaSermon sermon, bool save = false)
        {
            var fileProperty = sermon.Media.getProperty("audioFile");
            string oldFileName = fileProperty.Value.ToString();

            if (!string.IsNullOrWhiteSpace(oldFileName) && File.Exists(HttpContext.Current.Server.MapPath(oldFileName)))
            {
                string newFileName = Path.GetDirectoryName(oldFileName) + "/" + sermon.Media.Text.Replace("/", "-") + Path.GetExtension(oldFileName);
                newFileName = newFileName.Replace("\\", "/").Replace(" ", "");

                if (oldFileName != newFileName)
                {
                    fileProperty.Value = newFileName;

                    if (File.Exists(HttpContext.Current.Server.MapPath(newFileName)))
                        File.Delete(HttpContext.Current.Server.MapPath(newFileName));

                    File.Move(HttpContext.Current.Server.MapPath(oldFileName), HttpContext.Current.Server.MapPath(newFileName));

                    if (save)
                        sermon.Media.Save();
                }
            }
        }
    }
}