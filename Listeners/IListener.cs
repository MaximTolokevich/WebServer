using WebServer.Clients;

namespace WebServer.Listeners
{
    public interface IListener
    {
        public void Start();
        public void Stop();
        public IClient AcceptClient();
    }
}
