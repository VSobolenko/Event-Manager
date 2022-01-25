namespace AnalyticsCore.ServerProvider
{
    internal interface IServerProvider<T>
    {
        void Send(T data);
    }
}