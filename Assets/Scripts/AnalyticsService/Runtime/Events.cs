using System;
using System.Collections.Generic;

namespace AnalyticsService
{
    [Serializable]
    public class Events
    {
        public List<EventData> events = new();

        public void Add(EventData eventData) => events.Add(eventData);
        public void AddRange(IEnumerable<EventData> collection) => events.AddRange(collection);
        public bool EventsIsEmpty() => events.Count < 1;
        public int Count() => events.Count;
    }
}