using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace WebServer.Clients
{
    public interface IClient : IDisposable
    {
        IPAddress GetClientInfo();
        NetworkStream GetStream();
        int GetBufferSize();
    }
}
