using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Clients;

namespace WebServer.Writers
{
    public interface IClientWriter
    {
        void Write(IClient client, byte[] data);
    }
}
