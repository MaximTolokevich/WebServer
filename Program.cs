using WebServer.Clients;
using WebServer.DependencyRegistrars;
using WebServer.DI;
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
            const string firstServer = "FirstServer";
            const string secondServer = "SecondServer";

            var col = new MyServiceCollection();

            col.AddSingletonWithName<CookieStorage>(firstServer);
            col.AddTransientWithName<ICookieGenerator, CookieGenerator>(firstServer);

            col.AddSingletonWithName<CookieStorage>(secondServer);
            col.AddTransientWithName<ICookieGenerator, CookieGenerator>(secondServer);

            col.AddTransientWithName<IMiddleware, CookieMiddleware>(firstServer);
            col.AddTransientWithName<IMiddleware, CookieMiddleware>(secondServer);

            DependencyRegistrar.AddConfig<ServerOptions>(col, "C:\\Users\\Trolo\\source\\repos\\WebServer\\Config\\jsconfig1.json", firstServer);
            DependencyRegistrar.RegisterHttpServerDependencies(col, firstServer);

            DependencyRegistrar.AddConfig<ServerOptions>(col, "C:\\Users\\Trolo\\source\\repos\\WebServer\\Config\\jsconfig2.json", secondServer);
            DependencyRegistrar.RegisterHttpServerDependencies(col, secondServer);

            var container = col.BuildContainer();

            var server = container.GetService<IServer>(firstServer);
            server.Start();
            var server2 = container.GetService<IServer>(secondServer);
            server2.Start();

        }
    }
}