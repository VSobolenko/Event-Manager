using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AnalyticsCore.ServerProvider
{
    internal class ServerProvider<T> : IServerProvider<T>
    {
        private readonly string _serverUrl;
        private HttpClient _httpClient;

        public ServerProvider(string serverUrl)
        {
            _serverUrl = serverUrl;
        }

        public async Task<int> Send(T data)
        {
            EnsureHttpClientCreated();
            
            var httpContent = new StringContent(data.ToString(), Encoding.UTF8, "application/json");
            var httpResponse = await _httpClient.PostAsync(_serverUrl, httpContent);
            
            var statusCode = (int)httpResponse.StatusCode;
            Debug.Log("Http response code " + statusCode);
            return statusCode;
        }

        private void EnsureHttpClientCreated()
        {
            if (_httpClient == null)
            {
                _httpClient = new HttpClient();
            }
        }
    }
}