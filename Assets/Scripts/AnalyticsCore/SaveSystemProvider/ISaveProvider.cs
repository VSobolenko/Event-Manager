using System.Threading.Tasks;

namespace AnalyticsCore.SaveSystemProvider
{
    public interface ISaveProvider<T>
    {
        Task SaveData(params T[] data);
        Task<T[]> LoadData();
        void ClearData();
    }
}