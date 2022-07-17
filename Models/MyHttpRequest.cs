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
        public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
        public byte[] Body { get; set; }

        public static MyHttpRequest Build(byte[] data, ServerOptions options)
        {
            if (!data.Any())
            {
                throw new InvalidRequestException("Empty request");
            }
            var indexFirsPairEnvNewLine = FindFirstPairEnvironmentNewLine(data);
            string requestWithoutBody;
            var body = Array.Empty<byte>();
            if (indexFirsPairEnvNewLine >0)
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
            }
             
            var splitedRequest = requestWithoutBody.Split(Environment.NewLine);
            var firstLineRequest = splitedRequest[0];
            var firstLineSplit = firstLineRequest.Split(' ');
            var method = firstLineSplit[0];
            var version = firstLineSplit[^1];
            var headers = new Dictionary<string, string>();
            
            for (var i = 1; i < splitedRequest.Length; i++)
            {
                var headerElement = splitedRequest[i].Split(": ");
                headers.Add(headerElement[0], string.Join(null, headerElement[1..]));
            }

            var uri = new Uri(firstLineSplit[1], UriKind.RelativeOrAbsolute);
            var baseUri = new UriBuilder()
            {
                Scheme = Uri.UriSchemeHttp,
                Port = options.Port
            };

             uri = new Uri(baseUri.Uri, uri);
            return new MyHttpRequest
            {
                Method = method,
                Uri = uri,
                Version = version,
                Headers = headers,
                Body = body,
            };
        }

        private static int FindFirstPairEnvironmentNewLine(byte[] data)
        {
            var envNewLineAsByte = Encoding.UTF8.GetBytes($"{Environment.NewLine}{Environment.NewLine}");
            for (var i = 0; i < data.Length; i++)
            {
                var isContained = data.Skip(i).Take(envNewLineAsByte.Length).SequenceEqual(envNewLineAsByte);
                if (isContained)
                    return i;
            }

            return -1;
        }
    }
}
