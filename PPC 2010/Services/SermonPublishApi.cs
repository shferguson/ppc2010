using PPC_2010.Data;
using PPC_2010.TimeZone;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace PPC_2010.Services
{

    public class SermonPublishApi : IDisposable, ISermonPublishApi
    {
        private readonly HttpClient _httpClient;

        private class ApiSermon
        {
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
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "www.providence-pca.net");
            _httpClient.DefaultRequestHeaders.Add("Accept", "*/*");
            _httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
        }

        public void Update(ISermon sermon)
        {
            var recordingDate = sermon.RecordingDate.GetValueOrDefault();
            var recordingDateEastern = TimeZoneConverter.ConvertToEastern(recordingDate);
            recordingDate += recordingDate - recordingDateEastern;

            var apiSermon = new ApiSermon
            {
                dataId = sermon.Id,
                title = sermon.Title,
                recordingDate = recordingDate.ToUniversalTime().ToString("o"),
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

        private async Task HandleHttpResult(Task<HttpResponseMessage> t)
        {
            try
            {
                if (t.IsFaulted)
                {
                    throw new Exception("Error posting sermon to " + _httpClient.BaseAddress, t.Exception);
                }
                else if (t.IsCanceled)
                {
                    throw new TaskCanceledException("Sermon post canceled " + _httpClient.BaseAddress);
                }
                else if (!t.Result.IsSuccessStatusCode)
                {
                    var content = await t.Result.Content.ReadAsStringAsync();
                    throw new Exception("Error uploading sermon to " + t.Result.RequestMessage.RequestUri + " Status Code: " + t.Result.StatusCode + " - " + content);
                }
                else
                {
#if DEBUG
                    var content = await t.Result.Content.ReadAsStringAsync();
                    Debug.WriteLine(t.Result.StatusCode + " - " + content);
#endif
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorLog.GetDefault(null).Log(new Elmah.Error(ex));
            }
        }
    }
}
