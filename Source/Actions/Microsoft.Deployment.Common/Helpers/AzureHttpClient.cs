using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Deployment.Common.Helpers
{
    public class AzureHttpClient
    {
        public Dictionary<string, string> Headers { get; set; }
        public Task Providers { get; set; }
        public string ResourceGroup { get; }
        public string Subscription { get; }
        public string Token { get; set; }

        public AzureHttpClient(string token)
        {
            this.Token = token;
        }

        public AzureHttpClient(string token, string subscriptionId)
        {
            this.Token = token;
            this.Subscription = subscriptionId;
        }

        public AzureHttpClient(string token, string subscription, string resourceGroup)
        {
            this.Token = token;
            this.Subscription = subscription;
            this.ResourceGroup = resourceGroup;
        }

        public AzureHttpClient(string token, Dictionary<string, string> headers)
        {
            this.Headers = headers;
            this.Token = token;
        }

        public async Task<HttpResponseMessage> ExecuteGenericRequestWithHeaderAsync(HttpMethod method, string url, string body)
        {
            using (HttpClient client = new HttpClient())
            {
                string requestUri = url;
                HttpRequestMessage message = new HttpRequestMessage(method, requestUri);

                if (method == HttpMethod.Post || method == HttpMethod.Put)
                {
                    message.Content = new StringContent(body, Encoding.UTF8, "application/json");
                }

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Token);

                if (this.Headers != null)
                {
                    foreach (KeyValuePair<string, string> header in this.Headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }

                return await client.SendAsync(message);
            }
        }

        public async Task<HttpResponseMessage> ExecuteWebsiteAsync(HttpMethod method, string site, string relativeUrl, string body)
        {
            string requestUri = $"https://{site}{Constants.AzureWebSite}{relativeUrl}";

            return await this.ExecuteGenericRequestWithHeaderAsync(method, requestUri, body);
        }

        public async Task<HttpResponseMessage> ExecuteWithSubscriptionAndResourceGroupAsync(HttpMethod method, string relativeUrl, string apiVersion, string body, Dictionary<string, string> queryParameters)
        {
            StringBuilder parameters = new StringBuilder();
            parameters.Append("?");
            foreach (var parameter in queryParameters)
            {
                parameters.Append($"{parameter.Key}={parameter.Value}&");
            }

            string requestUri = Constants.AzureManagementApi + $"/subscriptions/{this.Subscription}/resourceGroups/{this.ResourceGroup}/{relativeUrl}{parameters.ToString()}api-version={apiVersion}";

            return await this.ExecuteGenericRequestWithHeaderAsync(method, requestUri, body);
        }

        public async Task<HttpResponseMessage> ExecuteWithSubscriptionAndResourceGroupAsync(HttpMethod method, string relativeUrl, string apiVersion, string body)
        {
            string requestUri = Constants.AzureManagementApi + $"/subscriptions/{this.Subscription}/resourceGroups/{this.ResourceGroup}/{relativeUrl}?api-version={apiVersion}";

            return await this.ExecuteGenericRequestWithHeaderAsync(method, requestUri, body);
        }

        public async Task<HttpResponseMessage> ExecuteWithSubscriptionAsync(HttpMethod method, string relativeUrl, string apiVersion, string body)
        {
            string requestUri = Constants.AzureManagementApi + $"/subscriptions/{this.Subscription}/{relativeUrl}?api-version={apiVersion}";

            return await this.ExecuteGenericRequestWithHeaderAsync(method, requestUri, body);
        }

        public async Task<HttpResponseMessage> ExecuteGenericRequestNoHeaderAsync(HttpMethod method, string url, string body)
        {
            using (HttpClient client = new HttpClient())
            {
                string requestUri = url;
                HttpRequestMessage message = new HttpRequestMessage(method, requestUri);

                if (method == HttpMethod.Post || method == HttpMethod.Put)
                {
                    message.Content = new StringContent(body, Encoding.UTF8, "application/json");
                }

                return await client.SendAsync(message);
            }
        }

        public async Task<string> Request(HttpMethod method, string url, string body = "")
        {
            HttpResponseMessage response = await this.ExecuteGenericRequestWithHeaderAsync(method, url, body);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Request(string url, byte[] file, string name)
        {
            HttpResponseMessage response = null;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Token);

                using (MultipartFormDataContent content = new MultipartFormDataContent())
                {
                    content.Headers.ContentType.Parameters.Clear();
                    string boundary = "---------------------------8d4b4bdf9867764";
                    content.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("boundary", boundary));

                    HttpContent fileContent = new StreamContent(new MemoryStream(file));
                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
                    fileContent.Headers.ContentDisposition.Name = "\"file0\"";
                    fileContent.Headers.ContentDisposition.FileName = $"\"{name}\"";
                    fileContent.Headers.ContentLength = null;
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-zip-compressed");

                    content.Add(fileContent);
                    content.Headers.ContentLength += file.Length + (boundary.Length - 1) +
                        fileContent.Headers.ContentDisposition.ToString().Length +
                        fileContent.Headers.ContentType.ToString().Length;

                    response = await client.PostAsync(url, content);
                }
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}