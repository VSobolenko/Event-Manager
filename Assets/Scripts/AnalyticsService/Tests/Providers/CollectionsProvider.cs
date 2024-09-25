using System.Collections.Generic;

namespace AnalyticsService.SaveSystemProvider
{
    internal class CollectionsProvider<T> : ISaveProvider<T>
    {
        public List<T> Data = new();

        public void SaveData(params T[] data) => Data.AddRange(data);

        public T[] LoadData() => new List<T>(Data).ToArray();

        public void ClearData() => Data.Clear();
    }
}