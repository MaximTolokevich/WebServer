using WebServer.Clients;
using WebServer.DI;
using WebServer.HttpRequestReaders;
using WebServer.Listeners;
using WebServer.Middlewares;
using WebServer.Service;
using WebServer.Storage;

namespace WebServer
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var options = new ServerOptions()
            {
                Port = 8080,
                IpAddress = "127.0.0.1",
                ReadTimeOut = 10000,
                ServerName = "qwe"
            };

            var col = new MyServiceCollection();
            col.AddTransient<IListener, TcpListenerAdapter>();
            col.AddTransient<IManager, HttpManager>();
            col.AddTransient<ICookieGenerator, CookieGenerator>();
            col.AddTransient<CookieStorage>();
            col.AddTransient<IHttpRequestReader,HttpRequestReader>();
            col.AddTransient<IMiddleware,CookieMiddleware>();
            col.Add(options);
            col.AddSingleton<IServer,Server>();

            MiddlewareList list = new MiddlewareList();
            
            var container = col.BuildContainer();
            list.Middlewares.Add(BuildMiddleware(container));
            col.Add(list);
            var server = container.GetService<IServer>();
            server.Start();
        }

        
        
        private static IMiddleware BuildMiddleware(MyContainer container)
        {
            return container.GetService<IMiddleware>();
        }
    }
}