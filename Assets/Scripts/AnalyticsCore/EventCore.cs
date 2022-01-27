using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using AnalyticsCore.ServerProvider;
using UnityEngine;

namespace AnalyticsCore
{
    internal class EventCore : IDisposable
    {
        public float CooldownBeforeSend { get; set; }
        
        private readonly IServerProvider<string> _server;
        private Queue<EventData> _queueData = new Queue<EventData>();
        private Timer _timer;

        public EventCore(IServerProvider<string> server)
        {
            _server = server;
        }

        public void PostToServer(string type, string data)
        {
            Debug.Log($"<color=#00FF00>[info]</color> Add new event to queue:\n {type}; {data}");
            var eventData = new EventData {data = data, type = type};
            _queueData.Enqueue(eventData);
        }

        public void PostToServer(params EventData[] events)
        {
            foreach (var item in events)
            {
                _queueData.Enqueue(item);
            }
            Debug.Log("<color=#00FF00>[info]</color> Add new array events to queue");
        }

        public void SetEventTimer(float cooldownBeforeSend)
        {
            CooldownBeforeSend = cooldownBeforeSend;
            _timer = new Timer(cooldownBeforeSend) {AutoReset = true};
            _timer.Elapsed += OnTimedEvent;
            _timer.Start();
        }

        private async void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            if (_queueData.Count < 1)
            {
                return;
            }
            _timer.Stop();
            var sendData = new StringBuilder();
            sendData.Append(ToJson(_queueData.ToArray()));
            var statusCode = await _server.Send(sendData.ToString());
            _queueData.Clear();
            _timer.Start();
            
            Debug.Log($"<color=#00FF00>[info]</color> Send events to server:\n {sendData}");
            Debug.Log($"<color=#00FF00>[info]</color> Http response code {statusCode}");
        }

        ~EventCore()
        {
            Dispose();
        }
        
        public void Dispose()
        {
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