using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebServer.Clients;
using WebServer.Exceptions;

namespace WebServer.Models
{
    public class MyHttpRequest
    {
        public Uri Uri { get; set; }
        public string Method { get; set; }
        public string Version { get; set; }
        public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        public byte[] Body { get; set; }

        public static MyHttpRequest Build(IEnumerable<string> data, byte[] body, ServerOptions options)
        {
            if (!data.Any())
            {
                throw new InvalidRequestException("Empty request");
            }

            var firstLineRequest = GetFirstLineRequest(data);
            var firstLineSplit = GetSplitedFirstLine(firstLineRequest);
            var method = GetMethod(firstLineSplit);
            var version = GetVersion(firstLineSplit);
            var headers = GetHeaders(data.ToArray()[1..]);

            var uri = GetUri(firstLineSplit, options.Port);
            return new MyHttpRequest
            {
                Method = method,
                Uri = uri,
                Version = version,
                Headers = headers,
                Body = body,
            };
        }

        private static string GetFirstLineRequest(IEnumerable<string> request)
        {
            return string.Join(null, request.Take(3));
        }

        private static string[] GetSplitedFirstLine(string firstLine)
        {
            return firstLine.Split(' ');
        }

        private static string GetMethod(IReadOnlyList<string> request)
        {
            return request[0];
        }

        private static string GetVersion(IReadOnlyList<string> request)
        {
            return request[^1];
        }

        private static Dictionary<string, string> GetHeaders(IEnumerable<string> request)
        {
            return request.Select(t => t.Split(": "))
                .ToDictionary(headerElement => headerElement[0], headerElement =>
                    string.Join(null, headerElement[1..]));

        }

        private static Uri GetUri(string[] request, int port)
        {
            var uri = new Uri(request[1], UriKind.RelativeOrAbsolute);
            var baseUri = new UriBuilder()
            {
                Scheme = Uri.UriSchemeHttp,
                Port = port
            };

            return new Uri(baseUri.Uri, uri);
        }

        private static void InitializeRequest(int indexFirsPairEnvNewLine, out byte[] body, out string requestWithoutBody,
            byte[] data)
        {
            if (indexFirsPairEnvNewLine > 0)
            {
                var bytesToSkip = 3;
                if (!OperatingSystem.IsWindows())
                {
                    bytesToSkip = 1;
                }

                requestWithoutBody = Encoding.UTF8.GetString(data[..indexFirsPairEnvNewLine]);
                body = data[(indexFirsPairEnvNewLine + bytesToSkip + 1)..];
            }
            else
            {
                requestWithoutBody = Encoding.UTF8.GetString(data);
                body = Array.Empty<byte>();
            }
        }
    }
}
