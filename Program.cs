using MyDi.DI;
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
            const string firstServer = "FirstServer";
            const string secondServer = "SecondServer";

            var col = new MyServiceCollection();

            col.AddSingleton<CookieStorage>(firstServer);
            col.AddTransient<ICookieGenerator, CookieGenerator>(firstServer);

            col.AddSingleton<CookieStorage>(secondServer);
            col.AddTransient<ICookieGenerator, CookieGenerator>(secondServer);

            col.AddTransient<IMiddleware, CookieMiddleware>(firstServer);
            col.AddTransient<IMiddleware, CookieMiddleware>(secondServer);

            DependencyRegistrar.AddConfig<ServerOptions>(col, "C:\\Users\\Trolo\\Source\\Repos\\MaximTolokevich\\WebServer\\Config\\jsconfig1.json", firstServer);
            DependencyRegistrar.RegisterHttpServerDependencies(col, firstServer);

            DependencyRegistrar.AddConfig<ServerOptions>(col, "C:\\Users\\Trolo\\Source\\Repos\\MaximTolokevich\\WebServer\\Config\\jsconfig2.json", secondServer);
            DependencyRegistrar.RegisterHttpServerDependencies(col, secondServer);

            var container = col.BuildContainer();
            var server = container.GetService<IServer>(firstServer);
            server.Start();
            var server2 = container.GetService<IServer>(secondServer);
            server2.Start();
        }
    }
}