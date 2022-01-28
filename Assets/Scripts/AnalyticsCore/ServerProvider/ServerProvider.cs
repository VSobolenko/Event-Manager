using System;
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
            return (int)httpResponse.StatusCode;
        }

        public async Task<bool> HasConnection()
        {
            var ping = new System.Net.NetworkInformation.Ping();
            try
            {
                var result = await ping.SendPingAsync("www.google.com", 500);
                
                if (result.Status == System.Net.NetworkInformation.IPStatus.Success)
                    return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void EnsureHttpClientCreated()
        {
            if (_httpClient == null)
            {
                _httpClient = new HttpClient {Timeout = TimeSpan.FromMilliseconds(5000)};
            }
        }
    }
}