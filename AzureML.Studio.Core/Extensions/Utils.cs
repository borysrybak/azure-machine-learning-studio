using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AzureML.Studio.Core.Extensions
{
    internal static class Utils
    {
        internal static Task<HttpResponseMessage> PatchAsJsonAsync(this HttpClient client, string requestUri, string jsonBody)
        {
            var stringContent = new StringContent(jsonBody, Encoding.ASCII, "application/json");
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri) { Content = stringContent };

            return client.SendAsync(request);
        }
    }
}
