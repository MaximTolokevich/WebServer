using System.Collections.Generic;
using System.IO;
using System.Linq;
using WebServer.Clients;
using WebServer.Exceptions;
using WebServer.Models;

namespace WebServer.HttpRequestReaders
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
            var stream = client.GetStream();

            var streamReader = new StreamReader(stream);

            var list = new List<string>();

            var emptyString = string.Empty;

            string requestLine;

            do
            {
                requestLine = streamReader.ReadLine();

                if (!string.IsNullOrEmpty(requestLine))
                {
                    list.Add(requestLine);
                }


            } while (requestLine != null && !requestLine.Equals(emptyString));


            var httpRequest = MyHttpRequest.Build(list, new byte[] { }, _options);

            if (!httpRequest.Headers.ContainsKey("Content-Length"))
            {
                return httpRequest;
            }

            var isValidContentLengthValue = int.TryParse(httpRequest.Headers["Content-Length"], out var length);

            if (!isValidContentLengthValue)
            {
                throw new InvalidRequestException(
                    $"Invalid Content-Length header value{httpRequest.Headers["Content-Length"]}");
            }


            var binaryReader = new BinaryReader(stream);

            httpRequest.Body = httpRequest.Body.Concat(binaryReader.ReadBytes(length)).ToArray();


            return httpRequest;
        }
    }
}
