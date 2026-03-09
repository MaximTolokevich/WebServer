using System.Collections.Generic;
using System.Text;
using WebServer.Clients;

namespace WebServer.Models
{
    public class MyHttpResponse
    {
        public string Version { get; set; } = "HTTP/1.1";
        public string StatusCode { get; set; } = "200 OK";
        public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
        public IList<string> CookieList = new List<string>();
        public string ContentType { get; set; }
        public long ContentLength { get; set; }
        public byte[] Body { get; set; }
        public string ServerName { get; }

        public MyHttpResponse(ServerOptions options)
        {
            ServerName = options.ServerName;
        }

        public byte[] Build()
        {
            var builder = new StringBuilder();
            builder.AppendLine($"{Version} {StatusCode}");

            builder.AppendLine($"Server: {ServerName}");
            if (Body is not null)
            {
                ContentLength = Body.Length;
                builder.AppendLine($"Content-Type: {ContentType}");
                builder.AppendLine($"Content-Length: {ContentLength}");
            }

            foreach (var item in CookieList)
            {
                builder.AppendLine($"Set-Cookie: {item}");
            }

            foreach (var (key, value) in Headers)
            {
                builder.AppendLine($"{key}: {value}");
            }

            if (Body is not null)
            {
                builder.AppendLine();

                builder.Append(Encoding.UTF8.GetString(Body));
            }

            return Encoding.UTF8.GetBytes(builder.ToString());
        }
    }
}
