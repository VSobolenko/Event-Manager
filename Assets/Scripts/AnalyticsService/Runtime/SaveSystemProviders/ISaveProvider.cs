
namespace AnalyticsService.SaveSystemProvider
{
    public interface ISaveProvider<T>
    {
        void SaveData(params T[] data);
        T[] LoadData();
        void ClearData(); 
    }
}