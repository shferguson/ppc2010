using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;

namespace PPC_2010.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> PatchAsync<T>(this HttpClient client, string requestUri, T content, JsonMediaTypeFormatter formatter)
        {
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(content, formatter.SerializerSettings), Encoding.UTF8, "application/json"),
            };

            var response = await client.SendAsync(request);
            return response;
        }
    }
}