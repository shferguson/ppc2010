using PPC_2010.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace PPC_2010.usercontrols
{
    /// <summary>
    /// Summary description for SermonFileters
    /// </summary>
    public class SermonFileters : IHttpHandler
    {
        private IPreValueRepository _PreValueRepository;

        public class DropDownPair
        {
            [JsonProperty("value")]
            public string Value { get; set; }
            [JsonProperty("label")]
            public string Label { get; set; }
        }

        public SermonFileters()
        {
            _PreValueRepository = ServiceLocater.Instance.Locate<IPreValueRepository>();
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            ISermonRepository repository = ServiceLocater.Instance.Locate<ISermonRepository>();
            var sermons = repository.LoadAllSermons();
            var years = sermons.Where(s => s.RecordingDate.HasValue).Select(s => s.RecordingDate.Value.Year).Distinct().OrderByDescending(y => y).Select( y => new DropDownPair { Value = y.ToString(), Label = y.ToString()});
            var speakers = _PreValueRepository.Speakers().OrderBy(s => s.Value).Select(s => new DropDownPair { Value = s.Id.ToString(), Label = s.Value });
            var sessions = _PreValueRepository.Sessions().OrderBy(s => s.Value).Select(a => new DropDownPair { Value = a.Id.ToString(), Label = a.Value });
            var series = _PreValueRepository.SermonSeries().OrderBy(s => s.Value).Select(s => new DropDownPair { Value = s.Id.ToString(), Label = s.Value });

            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { years, speakers, sessions, series }));
        }

        private IEnumerable<IdValuePair> PrependEmpyValue(IEnumerable<IdValuePair> list)
        {
            return new IdValuePair[] { new IdValuePair() }.Union(list);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}