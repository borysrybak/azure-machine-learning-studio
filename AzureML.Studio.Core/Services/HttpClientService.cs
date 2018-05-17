using AzureML.Studio.Core.Models;
using AzureML.Studio.Core.Utils;
using AzureML.Studio.Models;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AzureML.Studio.Core.Services
{
    internal class HttpClientService
    {
        private string _sdkName { get; set; }
        internal string AuthorizationToken { get; set; }

        internal HttpClientService(string sdkName)
        {
            _sdkName = sdkName;
        }

        internal HttpClient GetAuthenticatedHttpClient()
        {
            return GetAuthenticatedHttpClient(AuthorizationToken);
        }

        internal HttpClient GetAuthenticatedHttpClient(string authCode)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authCode);
            httpClient.DefaultRequestHeaders.Add("x-ms-metaanalytics-authorizationtoken", authCode);
            httpClient.DefaultRequestHeaders.Add("x-aml-sdk", _sdkName);

            return httpClient;
        }

        internal async Task<HttpResult> HttpPost(string url, string jsonBody)
        {
            if (jsonBody == null)
                jsonBody = string.Empty;
            var httpClient = GetAuthenticatedHttpClient();
            var stringContent = new StringContent(jsonBody, Encoding.ASCII, "application/json");
            var httpResponseMessage = await httpClient.PostAsync(url, stringContent);
            var httpResult = await CreateHttpResult(httpResponseMessage);

            return httpResult;
        }

        internal async Task<HttpResult> HttpPatch(string url, string jsonBody)
        {
            if (jsonBody == null)
                jsonBody = string.Empty;
            var httpClient = GetAuthenticatedHttpClient();
            var httpResponseMessage = await httpClient.PatchAsJsonAsync(url, jsonBody);
            var httpResult = await CreateHttpResult(httpResponseMessage);

            return httpResult;
        }

        internal async Task<HttpResult> HttpPostFile(string url, string filePath)
        {
            var httpClient = GetAuthenticatedHttpClient();
            var streamContent = new StreamContent(File.OpenRead(filePath));
            var httpResponseMessage = await httpClient.PostAsync(url, streamContent);
            var httpResult = await CreateHttpResult(httpResponseMessage);

            return httpResult;
        }

        internal async Task<HttpResult> HttpDelete(string url)
        {
            var httpClient = GetAuthenticatedHttpClient();
            var httpResponseMessage = await httpClient.DeleteAsync(url);
            var httpResult = await CreateHttpResult(httpResponseMessage);

            return httpResult;
        }

        internal async Task<HttpResult> HttpPut(string url, string body)
        {
            return await HttpPut(AuthorizationToken, url, body);
        }

        internal async Task<HttpResult> HttpPut(string authCode, string url, string jsonBody)
        {
            var httpClient = GetAuthenticatedHttpClient();
            if (authCode != string.Empty)
                httpClient = GetAuthenticatedHttpClient(authCode);
            var stringContent = new StringContent(jsonBody, Encoding.ASCII, "application/json");
            var httpResponseMessage = await httpClient.PutAsync(url, stringContent);
            var httpResult = await CreateHttpResult(httpResponseMessage);

            return httpResult;
        }

        internal async Task<HttpResult> HttpGet(string url)
        {
            return await HttpGet(AuthorizationToken, url, true);
        }

        internal async Task<HttpResult> HttpGet(string url, bool withAuthHeader)
        {
            return await HttpGet(AuthorizationToken, url, withAuthHeader);
        }

        internal async Task<HttpResult> HttpGet(string authCode, string url, bool withAutHeader)
        {
            var httpClient = new HttpClient();
            if (withAutHeader)
                httpClient = GetAuthenticatedHttpClient(authCode);
            var httpResponseMessage = await httpClient.GetAsync(url);
            var httpResult = await CreateHttpResult(httpResponseMessage);

            return httpResult;
        }

        internal async Task<HttpResult> CreateHttpResult(HttpResponseMessage hrm)
        {
            var httpResult = new HttpResult
            {
                StatusCode = (int)hrm.StatusCode,
                Payload = await hrm.Content.ReadAsStringAsync(),
                PayloadStream = await hrm.Content.ReadAsStreamAsync(),
                IsSuccess = hrm.IsSuccessStatusCode,
                ReasonPhrase = hrm.ReasonPhrase
            };

            return httpResult;
        }
    }
}
