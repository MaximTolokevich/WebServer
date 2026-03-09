using MyDi.DI;
using System;
using System.Collections.Generic;
using System.IO;
using WebServer.Clients;
using WebServer.DependencyRegistrars;
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
            const string secondServer = "SecondServer";

            var col = new MyServiceCollection(new List<MyServiceProvider>());

            col.AddSingleton<CookieStorage>();
            col.AddTransient<ICookieGenerator, CookieGenerator>();

            col.AddSingleton<CookieStorage>(secondServer);
            col.AddTransient<ICookieGenerator, CookieGenerator>(secondServer);

            col.AddTransient<IMiddleware, CookieMiddleware>();
            col.AddTransient<IMiddleware, CookieMiddleware>(secondServer);

            var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));

            var config1 = Path.Combine(projectRoot, "Config", "jsconfig1.json");
            var config2 = Path.Combine(projectRoot, "Config", "jsconfig2.json");

            col.AddConfig<ServerOptions>(config1);
            col.RegisterHttpServerDependencies();
            col.AddConfig<ServerOptions>(config2, secondServer);
            col.RegisterHttpServerDependencies(secondServer);

            var container = col.BuildContainer();
            var server = container.GetService<IServer>();
            server.Start();
            var server2 = container.GetService<IServer>(secondServer);
            server2.Start();
        }
    }
}