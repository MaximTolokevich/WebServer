using WebServer.Clients;

namespace WebServer.Listeners
{
    public interface IListener
    {
        void Start();
        void Stop();
        IClient AcceptClient();
    }
}
