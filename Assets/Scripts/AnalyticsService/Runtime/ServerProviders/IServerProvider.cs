using System.Net;
using System.Threading.Tasks;

namespace AnalyticsService.ServerProviders
{
    public interface IServerProvider
    {
        Task<HttpStatusCode> Send(object data);

        Task<bool> HasConnection();
    }
}