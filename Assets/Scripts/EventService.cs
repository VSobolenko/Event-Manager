using AnalyticsCore;
using AnalyticsCore.ServerProvider;
using UnityEngine;

public class EventService : MonoBehaviour
{
    private void Start()
    {
        /*
         *  Used "http://ptsv2.com/" for test server. Every time need to generate a new url
         *  For a quick test, can use the console as a server that receives events
         */
        
        IServerProvider<string> server = new ServerProvider<string>("http://ptsv2.com/t/yhpqz-1643151353/post");
        //IServerProvider<string> server = new ConsoleProvider<string>();
        var eventCore = new EventCore(server);
        
        eventCore.PostToServer(GetExampleData());
    }

    public void TrackEvent(string type, string data)
    {
    }

    private EventData[] GetExampleData()
    {
        return new[]
        {
            new EventData {data = "data1", type = "MyType1"},
            new EventData {data = "data2", type = "MyType2"},
            new EventData {data = "data3", type = "MyType3"},
            new EventData {data = "data4", type = "MyType4"},
        };
    }
}