using Server.Clients;
using Server.Models;

namespace WebServer.HttpRequestReaders
{
    public interface IHttpRequestReader
    {
        MyHttpRequest Read(IClient client);
    }
}