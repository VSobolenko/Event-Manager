using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AnalyticsService.ServerProviders
{
    public class ServerProvider : IServerProvider
    {
        private readonly string _serverUrl;
        private HttpClient _httpClient;

        public ServerProvider(string serverUrl)
        {
            _serverUrl = serverUrl;
        }

        public async Task<HttpStatusCode> Send(object data)
        {
            EnsureHttpClientCreated();

            var json = ToJson(data);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var httpResponse = await _httpClient.PostAsync(_serverUrl, httpContent);
            return httpResponse.StatusCode;
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
            _httpClient ??= new HttpClient {Timeout = TimeSpan.FromMilliseconds(5000)};
        }

        private static string ToJson(object data)
        {
            return JsonUtility.ToJson(data);
        }
    }
}