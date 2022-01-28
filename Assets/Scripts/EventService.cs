using System;
using AnalyticsCore;
using AnalyticsCore.SaveSystemProvider;
using AnalyticsCore.ServerProvider;
using UnityEngine;

public class EventService : MonoBehaviour
{
    [SerializeField] private string urlServer;
    
    [Tooltip("In milliseconds")]
    [SerializeField] private int cooldownBeforeSend = 5000;
    private EventCore _eventCore;
    
    private void Start()
    {
        IServerProvider<string> server = new ServerProvider<string>("urlServer");
        ISaveProvider<EventData> save = new FileProvider();
        
        _eventCore = new EventCore(server, save);
        _eventCore.EnableEventTimer(cooldownBeforeSend);
    }

    public void TrackEvent(string type, string data)
    {
        _eventCore.PostToServer(type, data);
    }

    private int i = 0;
    private EventData GetExampleData()
    {
        i++;
        return new EventData {data = $"data{i}", type = $"MyType{i}"};
    }
}