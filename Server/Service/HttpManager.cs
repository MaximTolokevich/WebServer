using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Server.Clients;
using Server.Exceptions;
using Server.Middlewares.Interfaces;
using Server.Models;
using Server.Service.Interfaces;
using WebServer.HttpRequestReaders;

namespace Server.Service
{
    public class HttpManager : IManager
    {
        private readonly IHttpRequestReader _httpRequestReader;
        private readonly ServerOptions _options;
        private readonly ICollection<IMiddleware> _middlewares;
        public HttpManager(IHttpRequestReader requestReader, ServerOptions options, ICollection<IMiddleware> middleware)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _httpRequestReader = requestReader ?? throw new ArgumentNullException(nameof(requestReader));
            _middlewares = middleware ?? throw new ArgumentNullException(nameof(middleware));
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


                foreach (var item in _middlewares)
                {
                    item.Invoke(httpContext);
                }

                client.SendResponse(httpContext.HttpResponse.Build());
            }
            catch (InvalidRequestException e)
            {
                client.SendResponse(Encoding.UTF8.GetBytes(e.Message));
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
