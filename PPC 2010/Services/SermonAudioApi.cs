using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PPC_2010.Data;
using PPC_2010.Extensions;
using PPC_2010.Services.SermonAudio;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace PPC_2010.Services
{
    public class SermonAudioApi : ISermonAudioApi
    {
        private readonly HttpClient _httpClient;
        private readonly JsonMediaTypeFormatter _formatter;

        private const string BroadcasterId = "providencepgh";
        private const string FallbackSpeakerName = "Various Speakers";

        // Only publish sermons from sessions to sermon audio
        private static Dictionary<string, string> RecordingSessionMap = new Dictionary<string, string>()
        {
           {"Sunday Evening", "Sunday - PM" },
           {"Sunday Morning", "Sunday - AM" },
           {"Sunday School", "Sunday School" },
           /*
           {"Biblical Counseling Course - Session 1", "Teaching" },
           {"Biblical Counseling Course - Session 2", "Teaching" },
           {"Christmas Eve", "Special Meeting" },
           {"Good Friday", "Special Meeting" },
           {"Thursday Evening", "Special Meeting" },
           {"Maundy Thursday", "Special Meeting" },
           {"Special Event", "Special Meeting" },
           {"Covenant Parenting Conference", "Conference" },
           {"Creation Conference", "Conference" },
           {"Imago Dei Conference", "Conference" },
           {"Memorial Service", "Funeral Service" },
           {"Truth In Love", "Conference" },
           {"WIC Conference", "Conference" },
           */
        };

        private static Dictionary<string, string> SeriesNameMap = new Dictionary<string, string>()
        {
            { "Non-Series", null },
            { "James, The Epistle of Gold and Silver", "James" }
        };


        // Only publish sermons from these speakers to sermon audio

        private static Dictionary<string, string> SpeakerNameMap = new Dictionary<string, string>()
        {
           {"Pastor Appleton", "Rick Appleton" },
           {"Pastor Appleton [Rev. Richard Appleton]", "Rick Appleton" },
           {"Pastor Heiple", "Ray Heiple" },
           {"Pastor Heiple [Rev. Dr. Ray E. Heiple, Jr.]", "Ray Heiple" },

           /*
           {"Adam Thomas", "Adam Thomas" },
           {"Bob Alouise", "Bob Alouise" },
           {"Brad Winsted", "Brad Winsted" },
           {"Brandon Wilcox", "Brandon Wilcox" },
           {"Brett Wirebaugh", "Brett Wirebaugh" },
           {"Dan Valentine", "Dan Valentine" },
           {"Dave Douglas", "Dave Douglas" },
           {"Dean Falavolito", "Dean Falavolito" },
           {"Denny Baker", "Denny Baker" },
           {"Denny Stewart", "Denny Stewart" },
           {"Don Maurer", "Don Maurer" },
           {"Dr. Benjamin Shaw", "Dr. Benjamin Shaw" },
           {"Dr. C.J. Williams", "C.J. Williams" },
           {"Dr. Denny Putrow", "Dennis Prutow" },
           {"Dr. George Scipione", "George Scipione" },
           {"Dr. Jack Kinneer", "Rev. Dr. Jack Kinneer" },
           {"Dr. Jerry Bergman, Ph.D.", "Dr. Jerry Bergman" },
           {"Dr. Jim Weidemaar", "Jim Weidemaar" },
           {"Dr. Joseph Pipa", "Joseph A. Pipa Jr." },
           {"Dr. T. David Gordon", "T. David Gordon" },
           {"Greg Mead", "Greg Mead" },
           {"John Hartline", "John Hartline" },
           {"Jonathan Landry Cruse", "Jonathan Landry Cruse" },
           {"Justin Kunkle", "Justin Kunkle" },
           {"Kevin Hilliker", "Kevin Hilliker" },
           {"Luke Bluhm", "Luke Bluhm" },
           {"Mark Brown", "Mark Brown" },
           {"Mrs. Ann O'Neill", "Ann O'Neill" },
           {"Mrs. Eileen Scipione", "Eileen Scipione" },
           {"Nathaniel Keisel", "Nathaniel Keisel" },
           {"Pastor Adrian Armel", "Adrian Armel" },
           {"Pastor John McComb", "John McCombs" },
           {"Paul Deffenbaugh", "Paul Deffenbaugh" },
           {"Phil Amaismeier", "Phil Amaismeier" },
           {"Randy Johovich", "Randy Johovich" },
           {"Rev. Alan Johnson", "Rev. Alan Johnson" },
           {"Rev. Almir Pehlic", "Rev. Almir Pehlić" },
           {"Rev. Almir Pehlić", "Rev. Almir Pehlić" },
           {"Rev. David Kenyon", "Rev. David Kenyon" },
           {"Rev. Derek Bates", "Derek Bates" },
           {"Rev. Don Clements", "Don Clements" },
           {"Rev. Donnie Williams", "Donnie Williams" },
           {"Rev. Doug Comin", "Rev. Doug Comin" },
           {"Rev. Douglas E. Lee", "Rev. Douglas E. Lee" },
           {"Rev. Frank D. Moser", "Rev. Frank D. Moser" },
           {"Rev. Harold Roth", "Harold Roth" },
           {"Rev. Jeffery A. Garrett", "Rev. Jeffery A. Garrett" },
           {"Rev. Jeffrey A. Garrett", "Rev. Jeffrey A. Garrett" },
           {"Rev. Larry Bowlin", "Rev. Larry Bowlin" },
           {"Rev. Mark Garcia", "Rev. Mark Garcia" },
           {"Rev. Mark Robinson", "Rev. Mark Robinson" },
           {"Rev. Oliver Pierce", "Rev. Oliver Pierce" },
           {"Rev. Peter Doerfler", "Peter Doerfler" },
           {"Rev. Phil Gelston", "Phil Gelston" },
           {"Rev. Richard Lang", "Rich Lang" },
           {"Rev. Shaun Nolan", "Shaun Nolan" },
           {"Rev. Stephen Atkinson", "Stephen Atkinson" },
           {"Rev. Tom Smith", "Rev. Tom Smith" },
           {"Rev. Tom Stein", "Rev. Tom Stein" },
           {"Rick Appleton", "Rick Appleton" },
           {"Robert Gray", "Robert Gray" },
           {"Sam Mateer", "Sam Mateer" },
           {"Stewart E. Bair Jr.", "Stewart E. Bair Jr." },
           {"Tim Bayly", "Tim Bayly" },
           {"Todd Williams", "Todd Williams" },
           */
        };

        public SermonAudioApi()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://api.sermonaudio.com/v2/");
            _httpClient.DefaultRequestHeaders.Add("x-api-key", ConfigurationManager.AppSettings["sermonAudioApiKey"]);
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "www.providencepgh.org");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            _formatter = new JsonMediaTypeFormatter();
            _formatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
        public Task<string> Create(ISermon sermon)
        {
            return CreateOrUpdate(null, sermon);
        }

        public Task Update(string id, ISermon sermon)
        {
            return CreateOrUpdate(id, sermon);    
        }

        async Task<string> CreateOrUpdate(string id, ISermon sermon)
        {
            bool isNormalyPublishedSpeaker = false;

            if (!SpeakerNameMap.TryGetValue(sermon.SpeakerName, out var speakerName))
            {
                isNormalyPublishedSpeaker = true;
                speakerName = FallbackSpeakerName;
            }

            if (!RecordingSessionMap.TryGetValue(sermon.RecordingSession, out var sessionName))
                return null;

            int? seriesId = await GetOrCreateSeries(sermon.SermonSeries, createIfNonExisting: isNormalyPublishedSpeaker);

            if (seriesId == null && !isNormalyPublishedSpeaker)
                return null;

            if (id == null)
                id = await TryFindByContent(sermon);

            var saSermon = new SermonAudio.Sermon
            {
                BroadcasterId = BroadcasterId,
                SeriesId = seriesId,
                FullTitle = SermonAudioStrings.TruncateSermonTitle(sermon.Title),
                SpeakerName = speakerName,
                PublishTimestamp = 0,
                Subtitle = null,
                NewsInFocus = false,
                PreachDate = sermon.RecordingDate.GetValueOrDefault().ToString("yyyy-MM-dd"),
                EventType = sessionName,
                BibleText = sermon.ScriptureReference.ToString(),
                MoreInfoText = null,
                LanguageCode = "en",
                Keywords = null,
                AcceptCopyright = true,
            };

            HttpResponseMessage resp;
            if (id == null)
                resp = await _httpClient.PostAsync("node/sermons", saSermon, _formatter);
            else
                resp = await _httpClient.PutAsync($"node/sermons/{id}", saSermon, _formatter);

            await CheckForError(resp);

            dynamic sermonResult = JsonConvert.DeserializeObject(await resp.Content.ReadAsStringAsync(), _formatter.SerializerSettings);
            string sermonId = sermonResult.sermonID;

            if (seriesId != null)
            {
                resp = await _httpClient.PatchAsync($"node/sermons/{sermonId}", new { series_id = seriesId, }, _formatter);
                await CheckForError(resp);
            }

            return sermonId;
        }

        public async Task UploadFile(string id, string filePath)
        {
            var createMedia = new SermonAudio.Media.Create
            {
                SermonID = id,
                UploadType = "original-audio",
            };

            var resp = await _httpClient.PostAsync("media", createMedia, _formatter);
            await CheckForError(resp);
            var createResult = JsonConvert.DeserializeObject<SermonAudio.Media.CreateResponse>(await resp.Content.ReadAsStringAsync(), _formatter.SerializerSettings);

            if (!string.IsNullOrEmpty(filePath) && System.IO.File.Exists(filePath))
            {
                using (var fileStream = System.IO.File.OpenRead(filePath))
                {
                    resp = await _httpClient.PostAsync(createResult.uploadURL, new StreamContent(fileStream));
                    await CheckForError(resp);
                }
            }
        }

        public async Task Delete(string id)
        {
            await _httpClient.DeleteAsync($"node/sermons/{id}");
        }

        public async Task<string> TryFindByContent(ISermon sermon)
        {
            if (!SpeakerNameMap.TryGetValue(sermon.SpeakerName, out var speakerName))
                return null;

            var seriesName = sermon.SermonSeries;
            if (SeriesNameMap.TryGetValue(sermon.SermonSeries, out var overrideSeriesName))
                seriesName = overrideSeriesName;

            var queryStringParams = new NameValueCollection
            {
                { "broadcasterID", BroadcasterId },
                { "speakerName", speakerName },
                { "year", sermon.RecordingDate.Value.Year.ToString() },
                { "month", sermon.RecordingDate.Value.Month.ToString() },
                { "day", sermon.RecordingDate.Value.Day.ToString() },
                { "series", SermonAudioStrings.TruncateSeriesName(seriesName) },
                { "includeDrafts", "true" },
                { "includeScheduled", "true" },
                { "includePublished", "true" },
                { "lite", "true" },
                { "liteBroadcaster", "true" },
            };

            if (sermon.Book != null) queryStringParams.Add("book", Osis.IdForBibleBookName(sermon.Book));
            if (sermon.StartChapter.HasValue) queryStringParams.Add("chapter", sermon.StartChapter.Value.ToString());
            if (sermon.EndChapter.HasValue) queryStringParams.Add("chapterEnd", sermon.EndChapter.Value.ToString());
            if (sermon.StartVerse.HasValue) queryStringParams.Add("verse", sermon.StartVerse.Value.ToString());
            if (sermon.EndVerse.HasValue) queryStringParams.Add("verseEnd", sermon.EndVerse.Value.ToString());

            var resp = await _httpClient.GetAsync($"node/sermons{queryStringParams.ToQueryString()}");
            if (resp.StatusCode == HttpStatusCode.NotFound)
                return null;

            await CheckForError(resp);

            dynamic output = JsonConvert.DeserializeObject(await resp.Content.ReadAsStringAsync(), _formatter.SerializerSettings);

            if (output.totalCount > 0)
                return output.results[0].sermonID;

            return null;
        }

        public async Task<int?> GetOrCreateSeries(string seriesName, bool createIfNonExisting)
        {
            if (seriesName == null || seriesName.Length > SermonAudioStrings.SeriesNameMaxLength)
                return await Task.FromResult(new int?());

            if (SeriesNameMap.TryGetValue(seriesName, out var overrideSeriesName))
            {
                seriesName = overrideSeriesName;
            }

            if (seriesName == null)
                return await Task.FromResult(new int?());

            HttpResponseMessage resp = await _httpClient.GetAsync($"node/broadcasters/{BroadcasterId}/series/{seriesName}");
            if (!resp.IsSuccessStatusCode)
            {
                if (resp.StatusCode == HttpStatusCode.NotFound)
                {
                    if (createIfNonExisting)
                    {
                        resp = await _httpClient.PostAsync($"node/broadcasters/{BroadcasterId}/series", new { series_name = seriesName }, _formatter);
                        await CheckForError(resp);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    await CheckForError(resp);
                }
            }

            dynamic seriesResult = JsonConvert.DeserializeObject(await resp.Content.ReadAsStringAsync(), _formatter.SerializerSettings);
            int seriesId = seriesResult.seriesID;
            return seriesId;
        }
     
        private async Task CheckForError(HttpResponseMessage resp)
        {
            if (!resp.IsSuccessStatusCode)
            {
                var error = await resp.Content.ReadAsStringAsync();
                throw new Exception($"Error posting sermon to SermonAudio: - {resp.RequestMessage.RequestUri} {resp.StatusCode}: {error}");
            }
        }
    }
}