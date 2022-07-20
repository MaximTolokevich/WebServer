using System;
using System.Net;

namespace WebServer.Clients
{
    public interface IClient : IDisposable
    {
        IPAddress GetClientInfo();
        byte[] ReadRequest();
        void SendResponse(byte[] data);
    }
}
