using System.Linq;
using WebServer.Clients;
using WebServer.Exceptions;
using WebServer.Models;
using WebServer.Service;

namespace WebServer.HttpRequestReaders
{
    public class HttpRequestReader : IHttpRequestReader
    {
        public MyHttpRequest Read(IClient client)
        {
            var httpRequest = new MyHttpRequest().Build(client.ReadRequest());
            if (httpRequest.Headers.ContainsKey("Content-Length"))
            {
                var isSuccess = int.TryParse(httpRequest.Headers["Content-Length"], out var length);
                if (!isSuccess)
                {
                    throw new InvalidRequestException(
                        $"Invalid Content-Length header value{httpRequest.Headers["Content-Length"]}");
                }

                if (httpRequest.Body.Length == length)
                {
                    return httpRequest;
                }

                while (httpRequest.Body.Length != length)
                {
                    httpRequest.Body = httpRequest.Body.Concat(client.ReadRequest()).ToArray();
                }

            }

            return httpRequest;
        }
    }
}
