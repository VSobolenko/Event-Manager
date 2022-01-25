using UnityEngine;

namespace AnalyticsCore.ServerProvider
{
    internal class ConsoleProvider<T> : IServerProvider<T>
    {
        public void Send(T data)
        {
            Debug.Log(data);
        }
    }
}