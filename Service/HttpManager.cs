using System;
using WebServer.Clients;
using WebServer.Middlewares;

namespace WebServer.Service
{
    public class HttpManager : IManager
    {
        private readonly IHttpRequestReader _httpRequestReader;
        private readonly ServerOptions _options;
        private readonly MiddlewareList _middlewareList;
        public HttpManager(IHttpRequestReader requestReader, ServerOptions options, MiddlewareList middleware)
        {
            _options = options;
            _httpRequestReader = requestReader;
            _middlewareList = middleware;
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

                foreach (var item in _middlewareList.Middlewares)
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
