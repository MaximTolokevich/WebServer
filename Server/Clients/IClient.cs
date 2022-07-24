using System;
using System.Net;

namespace Server.Clients
{
    public interface IClient : IDisposable
    {
        IPAddress GetClientInfo();
        byte[] ReadRequest();
        void SendResponse(byte[] data);
    }
}
