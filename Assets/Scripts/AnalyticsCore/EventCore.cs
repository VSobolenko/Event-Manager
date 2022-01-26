using System;
using AnalyticsCore.ServerProvider;
using UnityEngine;

namespace AnalyticsCore
{
    internal class EventCore
    {
        private readonly IServerProvider<string> _server;

        public EventCore(IServerProvider<string> server)
        {
            _server = server;
        }

        public void PostToServer(string type, string data)
        {
            var eventData = new EventData {data = data, type = type};
            var jsonData = JsonUtility.ToJson(eventData);

            _server.Send(jsonData);
        }

        public void PostToServer(EventData[] events)
        {
            var jsonData = ToJson(events);

            _server.Send(jsonData);
        }
        
        public void PostToServer(EventData events)
        {
            var jsonData = JsonUtility.ToJson(events);

            _server.Send(jsonData);
        }

        private static string ToJson<T>(T[] array)
        {
            var wrapper = new Wrapper<T> {events = array};
            return JsonUtility.ToJson(wrapper);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] events;
        }
    }
}