using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnalyticsCore.SaveSystemProvider
{
    public class CollectionsProvider<T> : ISaveProvider<T>
    {
        private List<T> _data = new List<T>();
        
        public async Task SaveData(params T[] data)
        {
            _data.AddRange(data);
            await Task.Delay(10);
        }

        public async Task<T[]> LoadData()
        {
            await Task.Delay(10);
            return _data.ToArray();
        }

        public void ClearData()
        {
            _data.Clear();
        }
    }
}