using WebServer.Clients;
using WebServer.Models;

namespace WebServer.HttpRequestReaders
{
    public interface IHttpRequestReader
    {
        MyHttpRequest Read(IClient client);
    }
}