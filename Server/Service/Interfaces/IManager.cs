using Server.Clients;

namespace Server.Service.Interfaces
{
    public interface IManager
    {
        void Manage(IClient client);
    }
}
