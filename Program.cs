using System.Collections.Generic;
using WebServer.Clients;
using WebServer.DI;
using WebServer.DI.Interfaces;
using WebServer.HttpRequestReaders;
using WebServer.JSONSerializer;
using WebServer.Listeners;
using WebServer.Middlewares;
using WebServer.Middlewares.Interfaces;
using WebServer.Service;
using WebServer.Service.Interfaces;
using WebServer.Storage;

namespace WebServer
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var firstServer = "FirstServer";
            var secondServer = "SecondServer";

            var firstSetting =
                JSONConfigToObjectMapper.MapConfig<ServerOptions>(
                    "C:\\Users\\Trolo\\source\\repos\\WebServer\\Config\\jsconfig2.json");
            var secondSetting = JSONConfigToObjectMapper.MapConfig<ServerOptions>(
                "C:\\Users\\Trolo\\source\\repos\\WebServer\\Config\\jsconfig1.json");

            var col = new MyServiceCollection();
            //registering dependencies of the first server
            col.AddSingletonWithName<IServer, Server>(firstServer);
            col.AddSingletonWithName<IListener, TcpListenerAdapter>(firstServer);
            col.AddSingletonWithName<CookieStorage>(firstServer);
            col.AddSingletonWithName<IMiddleware, CookieMiddleware>(firstServer);
            col.AddWithName(firstSetting, firstServer);

            col.AddTransientWithName<IManager, HttpManager>(firstServer);
            col.AddTransientWithName<ICookieGenerator, CookieGenerator>(firstServer);
            col.AddTransientWithName<IHttpRequestReader,HttpRequestReader>(firstServer);

            //registering dependencies of the first server
            col.AddSingletonWithName<IServer, Server>(secondServer);
            col.AddSingletonWithName<IListener, TcpListenerAdapter>(secondServer);
            col.AddSingletonWithName<CookieStorage>(secondServer);
            col.AddSingletonWithName<IMiddleware, CookieMiddleware>(secondServer);
            col.AddWithName(secondSetting, secondServer);

            col.AddTransientWithName<IManager, HttpManager>(secondServer);
            col.AddTransientWithName<ICookieGenerator, CookieGenerator>(secondServer);
            col.AddTransientWithName<IHttpRequestReader, HttpRequestReader>(secondServer);

            var container = col.BuildContainer();

            col.AddWithName<ICollection<IMiddleware>>(BuildMiddleware(container, firstSetting), firstServer);
            col.AddWithName<IDIContainer>(container, firstServer);

            col.AddWithName<ICollection<IMiddleware>>(BuildMiddleware(container, secondSetting), secondServer);
            col.AddWithName<IDIContainer>(container, secondServer);

            var server = container.GetService<IServer>(firstServer);
            server.Start();
            var server2 = container.GetService<IServer>(secondServer);
            server2.Start();

        }

        
        
        private static ICollection<IMiddleware> BuildMiddleware(MyContainer container,ServerOptions options)
        { 
            var list = new MiddlewareList().GetMiddlewares();
            list.Add(container.GetService<IMiddleware>(options.DependencyGroupName));
            return list;
        }

    }
}