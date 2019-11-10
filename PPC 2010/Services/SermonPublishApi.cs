using PPC_2010.Data;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace PPC_2010.Services
{
    public class SermonPublishApi: IDisposable
    {
        private readonly HttpClient _httpClient;

        private class ApiSermon {
            public int dataId { get; set; }
            public string title { get; set; }
            public string recordingDate { get; set; }
            public int speakerTitleId { get; set; }
            public string speakerTitle { get; set; }
            public int speakerNameId { get; set; }
            public string speakerName { get; set; }
            public int recordingSessionId { get; set; }
            public string recordingSession { get; set; }
            public int sermonSeriesId { get; set; }
            public string sermonSeries { get; set; }
            public string scriptureReference { get; set; }
            public string audioFile { get; set; }
        }

        public SermonPublishApi()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["publishSermonUrl"]);
            _httpClient.DefaultRequestHeaders.Add("x-api-key", ConfigurationManager.AppSettings["publishSermonApiKey"]);
        }

        public void Update(ISermon sermon)
        {
            var apiSermon = new ApiSermon
            {
                dataId = sermon.Id,
                title = sermon.Title,
                recordingDate = sermon.RecordingDate.GetValueOrDefault().ToUniversalTime().ToString("o"),
                speakerTitleId = sermon.SpeakerTitleId,
                speakerTitle = sermon.SpeakerTitle,
                speakerNameId = sermon.SpeakerNameId,
                speakerName = sermon.SpeakerName,
                recordingSessionId = sermon.RecordingSessionId,
                recordingSession = sermon.RecordingSession,
                sermonSeriesId = sermon.SermonSeriesId,
                sermonSeries = sermon.SermonSeries,
                scriptureReference = sermon.ScriptureReference.ScriptureString,
                audioFile = UrlService.MakeFullUrl(sermon.RecordingUrl),
            };

            _httpClient.PostAsync("sermon", apiSermon, new JsonMediaTypeFormatter())
                .ContinueWith(HandleHttpResult);
        }

        public void Delete(int sermonId)
        {
            _httpClient.DeleteAsync("sermon/" + sermonId)
                .ContinueWith(HandleHttpResult);
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        private static void HandleHttpResult(Task<HttpResponseMessage> t)
        {
            if (t.IsFaulted)
            {
                Debug.WriteLine(t.Exception);
            }
            else if (t.IsCanceled)
            {
                Debug.WriteLine("Canceled!");
            }
            else
            {
                if (!t.Result.IsSuccessStatusCode)
                {
                    t.Result.Content.ReadAsStringAsync()
                        .ContinueWith(content =>
                        {
                            Debug.WriteLine(t.Result.StatusCode + " - " + content);
                        });
                }
            }
        }
    }
}
