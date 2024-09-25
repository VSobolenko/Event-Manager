using System.Net;
using System.Threading.Tasks;

namespace AnalyticsService.ServerProviders
{
    internal class ConsoleProvider : IServerProvider
    {
        public object LastSendObject;
        public const int ServerDelay = 1;
        public bool ServerAvailability = true;
        
        public async Task<HttpStatusCode> Send(object data)
        {
            LastSendObject = data;
            await Task.Delay(ServerDelay);
            return HttpStatusCode.OK;
        }

        public async Task<bool> HasConnection()
        {
            await Task.Delay(ServerDelay);
            
            return ServerAvailability;
        }
    }
}