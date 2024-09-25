using AnalyticsService;
using AnalyticsService.SaveSystemProvider;
using AnalyticsService.ServerProviders;
using UnityEngine;

public class EventService : MonoBehaviour
{
    [SerializeField] private string urlServer;

    [Tooltip("In milliseconds")] [SerializeField]
    private int cooldownBeforeSend = 5000;

    private EventCore _eventCore;

    private void Awake()
    {
        var server = new ServerProvider(urlServer);
        var save = new FileProvider();

        _eventCore = new EventCore(server, save, cooldownBeforeSend);
    }

    public void TrackEvent(string type, string data)
    {
        _eventCore.TrackEvent(type, data);
    }

    private void OnDestroy()
    {
        _eventCore.Dispose();
    }
}