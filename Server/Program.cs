using DI;
using Server.Clients;
using Server.DependencyRegistrars;
using Server.Middlewares;
using Server.Middlewares.Interfaces;
using Server.Service;
using Server.Service.Interfaces;
using Server.Storage;

namespace Server
{
    public class Program
    {

        public static void Main(string[] args)
        {
            const string firstServer = "FirstServer";
            const string secondServer = "SecondServer";

            var col = new MyServiceCollection();

            col.AddSingleton<CookieStorage>(firstServer);
            col.AddTransient<ICookieGenerator, CookieGenerator>(firstServer);

            col.AddSingleton<CookieStorage>(secondServer);
            col.AddTransient<ICookieGenerator, CookieGenerator>(secondServer);

            col.AddTransient<IMiddleware, CookieMiddleware>(firstServer);
            col.AddTransient<IMiddleware, CookieMiddleware>(secondServer);

            DependencyRegistrar.AddConfig<ServerOptions>(col, "C:\\Users\\Trolo\\source\\repos\\Server\\Server\\Config\\jsconfig1.json", firstServer);
            DependencyRegistrar.RegisterHttpServerDependencies(col, firstServer);

            DependencyRegistrar.AddConfig<ServerOptions>(col, "C:\\Users\\Trolo\\source\\repos\\Server\\Server\\Config\\jsconfig2.json", secondServer);
            DependencyRegistrar.RegisterHttpServerDependencies(col, secondServer);

            var container = col.BuildContainer();
            var server = container.GetService<IServer>(firstServer);
            server.Start();
            var server2 = container.GetService<IServer>(secondServer);
            server2.Start();
        }
    }
}