using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Clients;

namespace WebServer.Service
{
    public interface IManager
    {
        void Manage(IClient client);
    }
}
