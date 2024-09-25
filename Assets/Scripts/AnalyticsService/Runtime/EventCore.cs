using System;
using System.Net;
using System.Threading.Tasks;
using System.Timers;
using AnalyticsService.SaveSystemProvider;
using AnalyticsService.ServerProviders;
using UnityEngine;
using Timer = System.Timers.Timer;

namespace AnalyticsService
{
    public class EventCore : IDisposable
    {
        private readonly ISaveProvider<EventData> _save;
        private readonly IServerProvider _server;

        private Events _events = new();
        private readonly int _cooldownBeforeSend;
        private Timer _timer;

        public EventCore(IServerProvider server, ISaveProvider<EventData> save, int cooldownBeforeSend)
        {
            _server = server;
            _save = save;
            _cooldownBeforeSend = cooldownBeforeSend;

            SetupTimer();
            InitializeExistsEvents();
            InvokeFirstInitialize();
        }

        public void TrackEvent(string type, string data)
        {
            var eventData = new EventData {data = data, type = type};
            _events.Add(eventData);

            if (_timer.Enabled == false)
                EnableEventTimer();
        }

        public void Dispose()
        {
            _timer?.Dispose();
            SaveActiveEvents();
        }

        private void SetupTimer()
        {
            _timer = new Timer(_cooldownBeforeSend) {AutoReset = true};
            _timer.Elapsed += OnTimedEvent;
        }
        
        private async void InvokeFirstInitialize()
        {
            await ProcessEvents();
        }

        private void EnableEventTimer()
        {
            _timer.Start();
        }

        private async void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            await ProcessEvents();
        }

        private async Task ProcessEvents()
        {
            if (_events.EventsIsEmpty())
                return;

            if (await _server.HasConnection() == false)
                return;
            
            var statusCode = await SendDataToServer();
            ProcessResponse(statusCode);
        }

        private async Task<HttpStatusCode> SendDataToServer()
        {
            return await _server.Send(_events);
        }

        private void ProcessResponse(HttpStatusCode code)
        {
            if (code == HttpStatusCode.OK)
            {
                Debug.Log($"Complete load data to server; CountEvents={_events.Count()}");
                _save.ClearData();
                _events = new Events();
            }
            else
            {
                Debug.Log($"No internet connection; ActiveEvents={_events.Count()}");
            }
        }

        private void SaveActiveEvents()
        {
            _save.SaveData(_events.events.ToArray());
        }

        private void InitializeExistsEvents()
        {
            var data = _save.LoadData();
            _save.ClearData();
            _events = new Events();
            _events.AddRange(data);
            Debug.Log($"Complete loading existing data. Count = {data.Length}");
        }
    }
}