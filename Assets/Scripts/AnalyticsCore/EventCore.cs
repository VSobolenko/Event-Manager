using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using AnalyticsCore.SaveSystemProvider;
using AnalyticsCore.ServerProvider;
using UnityEngine;

namespace AnalyticsCore
{
    internal class EventCore : IDisposable
    {
        public double CooldownBeforeSend
        {
            get => _timer.Interval;
            set => _timer.Interval = value;
        }
        
        private readonly IServerProvider<string> _server;
        private readonly ISaveProvider<EventData> _save;
        
        private Queue<EventData> _queueData = new Queue<EventData>();
        private Timer _timer;

        public EventCore(IServerProvider<string> server, ISaveProvider<EventData> save)
        {
            _server = server;
            _save = save;

            SendDataToServer();
        }

        public void PostToServer(string type, string data)
        {
            Debug.Log($"<color=#00FF00>[events]</color> Add new event to queue:\n {type}; {data}");
            var eventData = new EventData {data = data, type = type};
            _queueData.Enqueue(eventData);
        }

        public void PostToServer(params EventData[] events)
        {
            foreach (var item in events)
            {
                _queueData.Enqueue(item);
            }
            Debug.Log("<color=#00FF00>[events]</color> Add new array events to queue");
        }

        public void EnableEventTimer(float cooldownBeforeSend)
        {
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

            var statusCode = await SendDataToServer();

            if (statusCode != 200)
            {
                _save.SaveData(_queueData.ToArray());
                Debug.Log("<color=#00FF00>[events]</color> The server is not available or there is no internet connection");
            }
            else
            {
                _save.ClearData();
                Debug.Log($"<color=#00FF00>[events]</color> Complete load data to server");
            }
            
            _queueData.Clear();
            _timer.Start();
        }

        private async Task<int> SendDataToServer()
        {
            var statusCode = 0;
            if (await _server.HasConnection())
            { 
                Debug.Log("<color=#00FF00>[events]</color> Attempt to send data to the server ...");
                LoadExistsEvents();
                var sendData = ToJson(_queueData.ToArray());
                statusCode = await _server.Send(sendData);
                return statusCode;
            }
            
            return statusCode;
        }
        
        private void LoadExistsEvents()
        {
            var data = _save.LoadData();
            foreach (var eventData in data)
            {
                _queueData.Enqueue(eventData);
            }
            Debug.Log($"<color=#00FF00>[events]</color> Complete load data. Count = {data.Length}\n Data: {ToJson(data)}");
        }

        ~EventCore()
        {
            Dispose();
        }
        
        public void Dispose()
        {
            if(_queueData.Count < 1) return;
            
            Debug.Log("<color=#00FF00>[events]</color> Unsent data saved");
            _save.SaveData(_queueData.ToArray());
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