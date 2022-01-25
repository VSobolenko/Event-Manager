using System.IO;
using System.Net;

namespace AnalyticsCore.ServerProvider
{
    internal class ServerProvider<T> : IServerProvider<T>
    {
        private readonly string _serverUrl;

        public ServerProvider(string serverUrl)
        {
            _serverUrl = serverUrl;
        }

        public void Send(T data)
        {
            var httpWebRequest = (HttpWebRequest) WebRequest.Create(_serverUrl);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse) httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
        }
    }
}