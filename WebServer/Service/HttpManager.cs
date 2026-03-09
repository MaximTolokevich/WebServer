using MyDi.DI.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Text;
using WebServer.Clients;
using WebServer.Exceptions;
using WebServer.HttpRequestReaders;
using WebServer.Middlewares.Interfaces;
using WebServer.Models;
using WebServer.Service.Interfaces;
using WebServer.Writers;

namespace WebServer.Service
{
    public class HttpManager : IManager
    {
        private readonly IHttpRequestReader _httpRequestReader;
        private readonly ServerOptions _options;
        private readonly IDIContainer _container;
        private readonly IClientWriter _writer;
        public HttpManager(IHttpRequestReader requestReader, ServerOptions options, IClientWriter writer, IDIContainer container)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _httpRequestReader = requestReader ?? throw new ArgumentNullException(nameof(requestReader));
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }
        public void Manage(IClient client)
        {
            try
            {
                var httpRequest = _httpRequestReader.Read(client);
                var httpContext = new MyHttpContext(_options)
                {
                    HttpRequest = httpRequest
                };

                httpContext.HttpRequest.Headers.Add("IPAddress", client.GetClientInfo().ToString());

                var middlewares = _container.GetAll<IMiddleware>(_options.DependencyGroupName).ToArray();

                foreach (var item in middlewares)
                {
                    item.Invoke(httpContext);
                }

                _writer.Write(client,httpContext.HttpResponse.Build());

            }
            catch (InvalidRequestException e)
            {
                _writer.Write(client,Encoding.UTF8.GetBytes(e.Message));
            }
            catch (IOException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
