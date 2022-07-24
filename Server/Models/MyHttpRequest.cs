using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Clients;
using Server.Exceptions;

namespace Server.Models
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
            InitializeRequest(indexFirsPairEnvNewLine,out var body, out var requestWithoutBody, data);

            var splitedRequest = requestWithoutBody.Split(Environment.NewLine);
            var firstLineRequest = GetFirstLineRequest(splitedRequest);
            var firstLineSplit = GetSplitedFirstLine(firstLineRequest);
            var method = GetMethod(firstLineSplit);
            var version = GetVersion(firstLineSplit);
            var headers = GetHeaders(splitedRequest[1..]);

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

        private static string GetFirstLineRequest(string[] request)
        {
            return request[0];
        }

        private static string[] GetSplitedFirstLine(string firstLine)
        {
            return firstLine.Split(' ');
        }

        private static string GetMethod(string[] request)
        {
            return request[0];
        }

        private static string GetVersion(string[] request)
        {
            return request[^1];
        }

        private static Dictionary<string, string> GetHeaders(string[] request)
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

        private static void InitializeRequest(int indexFirsPairEnvNewLine,out byte[] body,out string requestWithoutBody,
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
