using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.Clients
{
    public class ServerOptions
    {
        public string ServerName { get; set; }
        public int ReadTimeOut { get; set; }
        public int Port { get; set; }
        public string IpAddress { get; set; }

        public ServerOptions()
        {

        }
    }
}
