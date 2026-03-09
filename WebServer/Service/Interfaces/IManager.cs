using WebServer.Clients;

namespace WebServer.Service.Interfaces
{
    public interface IManager
    {
        void Manage(IClient client);
    }
}
