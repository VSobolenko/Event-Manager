using System.Threading.Tasks;
using UnityEngine;

namespace AnalyticsCore.ServerProvider
{
    internal class ConsoleProvider<T> : IServerProvider<T>
    {
        public async Task<int> Send(T data)
        {
            await Task.Delay(1000);
            
            return 200;
        }

        public async Task<bool> HasConnection()
        {
            await Task.Delay(1000);
            
            return true;
        }
    }
}