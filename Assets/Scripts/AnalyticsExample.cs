using UnityEngine;
using UnityEngine.UI;

public class AnalyticsExample : MonoBehaviour
{
    [SerializeField] private InputField type;
    [SerializeField] private InputField data;
    [SerializeField] private EventService eventService;

    public void SendData()
    {
        eventService.TrackEvent(type.text, data.text);
    }
}