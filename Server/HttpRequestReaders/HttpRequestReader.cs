using System.Linq;
using Server.Clients;
using Server.Exceptions;
using Server.Models;
using WebServer.HttpRequestReaders;

namespace Server.HttpRequestReaders
{
    public class HttpRequestReader : IHttpRequestReader
    {
        private readonly ServerOptions _options;

        public HttpRequestReader(ServerOptions options)
        {
            _options = options;
        }
        public MyHttpRequest Read(IClient client)
        {
            var httpRequest = MyHttpRequest.Build(client.ReadRequest(), _options);
            if (!httpRequest.Headers.ContainsKey("Content-Length")) return httpRequest;
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

            while (httpRequest.Body.Length < length)
            {
                httpRequest.Body = httpRequest.Body.Concat(client.ReadRequest()).ToArray();
            }

            return httpRequest;
        }
    }
}
