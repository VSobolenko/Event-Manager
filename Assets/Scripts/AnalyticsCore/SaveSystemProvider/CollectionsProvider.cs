using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalyticsCore.SaveSystemProvider
{
    public class CollectionsProvider<T> : ISaveProvider<T>
    {
        private List<T> _data = new List<T>();
        
        public void SaveData(params T[] data)
        {
            _data.AddRange(data);
        }

        public T[] LoadData()
        {
            return _data.ToArray();
        }

        public void ClearData()
        {
            _data.Clear();
        }

        public bool HasData => _data.Count > 0;
    }
}