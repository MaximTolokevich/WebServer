using System;
using System.Collections.Generic;
using WebServer.Clients;
using WebServer.DI.Interfaces;
using WebServer.HttpRequestReaders;
using WebServer.Middlewares.Interfaces;
using WebServer.Models;
using WebServer.Service.Interfaces;

namespace WebServer.Service
{
    public class HttpManager : IManager
    {
        private readonly IHttpRequestReader _httpRequestReader;
        private readonly ServerOptions _options;
        private readonly IDIContainer _container;
        public HttpManager(IHttpRequestReader requestReader, ServerOptions options, IDIContainer container)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _httpRequestReader = requestReader ?? throw new ArgumentNullException(nameof(requestReader));
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

                var middlewareList = _options.DependencyGroupName is null ? 
                    _container.GetService<ICollection<IMiddleware>>(null) :
                    _container.GetService<ICollection<IMiddleware>>(_options.DependencyGroupName);

                foreach (var item in middlewareList)
                {
                    item.Invoke(httpContext);
                }

                client.SendResponse(httpContext.HttpResponse.Build());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
