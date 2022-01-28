using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnaluticsExample : MonoBehaviour
{
    [SerializeField] private InputField type;
    [SerializeField] private InputField data;
    [SerializeField] private EventService eventService;

    public void SendData()
    {
        eventService.TrackEvent(type.text, data.text);
    }
}
