using System.Threading.Tasks;

namespace AnalyticsCore.ServerProvider
{
    internal interface IServerProvider<T>
    {
        Task<int> Send(T data);

        Task<bool> HasConnection();
    }
}