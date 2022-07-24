using Server.Clients;

namespace Server.Listeners
{
    public interface IListener
    {
        public void Start();
        public void Stop();
        public IClient AcceptClient();
    }
}
