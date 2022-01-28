using System;
using AnalyticsCore;
using AnalyticsCore.SaveSystemProvider;
using AnalyticsCore.ServerProvider;
using UnityEngine;

public class EventService : MonoBehaviour
{
    private EventCore _eventCore;
    
    private void Start()
    {
        /*
         *  Used "http://ptsv2.com/" for test server. Every time need to generate a new url
         *  For a quick test, can use the console as a server that receives events
         */
        
        IServerProvider<string> server = new ServerProvider<string>("http://ptsv2.com/t/0ib3x-1643315681/post");
        ISaveProvider<EventData> save = new FileProvider<EventData>();
        _eventCore = new EventCore(server, save);
        _eventCore.SetEventTimer(5000);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _eventCore.PostToServer(GetExampleData());
        }
    }

    public void TrackEvent(string type, string data)
    {
        _eventCore.PostToServer(type, data);
    }

    private EventData[] GetExampleArrayData()
    {
        return new[]
        {
            new EventData {data = "data1", type = "MyType1"},
            new EventData {data = "data2", type = "MyType2"},
            new EventData {data = "data3", type = "MyType3"},
            new EventData {data = "data4", type = "MyType4"},
        };
    }

    private int i = 0;
    private EventData GetExampleData()
    {
        i++;
        return new EventData {data = $"data{i}", type = $"MyType{i}"};
    }
}