using System.Web;
using PPC_2010.Data;
using PPC_2010.Data.Media;
using System.Text;
using System.IO;

namespace PPC_2010.Services
{
    public interface IMp3FileService
    {
        void SetMp3FileTags(MediaSermon sermon);
    }

    public class Mp3FileService : IMp3FileService
    {
        public void SetMp3FileTags(MediaSermon sermon)
        {
            SetMp3FileTags(HttpContext.Current.Server.MapPath(sermon.AudioFile), sermon);
        }

        private void SetMp3FileTags(string fileName, ISermon sermon)
        {
            if (string.IsNullOrEmpty(fileName))
                return;

            if (!File.Exists(fileName))
                return;

            if (!sermon.RecordingDate.HasValue)
                return;

            using (var mp3File = TagLib.File.Create(fileName))
            {
                mp3File.Tag.Clear();
                mp3File.Tag.Album = sermon.RecordingSession;
                mp3File.Tag.AlbumArtists = new[] { sermon.SpeakerFormalName };
                mp3File.Tag.Comment = "Providence Presbyterian Church - " + sermon.RecordingDate.Value.ToShortDateString() + " - " + sermon.RecordingSession;
                mp3File.Tag.Genres = new[] { "Speech" };
                mp3File.Tag.Title = sermon.Title;
                mp3File.Tag.Year = (uint)sermon.RecordingDate.Value.Year;
                mp3File.Tag.Copyright = "Providence Presbyterian Church - " + sermon.RecordingDate.Value.Year;

                var id3v2Tags = (TagLib.Id3v2.Tag)mp3File.GetTag(TagLib.TagTypes.Id3v2);

                // The 'Official artist/performer webpage' frame is a URL pointing at the artists official webpage.
                id3v2Tags.RemoveFrames("WOAR");
                var frame = new TagLib.Id3v2.UnknownFrame("WOAR");
                frame.Data = Encoding.ASCII.GetBytes("https://www.providencepgh.org/");
                id3v2Tags.AddFrame(frame);

                // The 'Official audio file webpage' frame is a URL pointing at a file specific webpage.
                id3v2Tags.RemoveFrames("WOAF");
                frame = new TagLib.Id3v2.UnknownFrame("WOAF");
                frame.Data = Encoding.ASCII.GetBytes(string.Format("https://www.providencepgh.org/sermon/{0}/{1}", sermon.Id, sermon.Title.Replace(" ", "-")));
                id3v2Tags.AddFrame(frame);

                // Sub title
                id3v2Tags.RemoveFrames("TIT3");
                if (sermon.ScriptureReference.HasReference)
                {
                    var textFrame = new TagLib.Id3v2.TextInformationFrame("TIT3", TagLib.StringType.Latin1);
                    textFrame.Text = new[] { sermon.ScriptureReference.ToString() };
                    id3v2Tags.AddFrame(textFrame);
                }

                var cover = new TagLib.Id3v2.AttachedPictureFrame
                {
                    Type = TagLib.PictureType.FrontCover,
                    Description = "Cover",
                    MimeType = "image/png",
                    Data = File.ReadAllBytes(HttpContext.Current.Server.MapPath(@"..\Images\ppc_showcover.png")),
                    TextEncoding = TagLib.StringType.UTF16,
                };

                mp3File.Tag.Pictures = new TagLib.IPicture[] { cover };

                mp3File.Save();
            }
        }
    }
}