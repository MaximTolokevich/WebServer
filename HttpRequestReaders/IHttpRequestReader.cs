using WebServer.Clients;
using WebServer.Models;

namespace WebServer.Service
{
    public interface IHttpRequestReader
    {
        MyHttpRequest Read(IClient client);
    }
}