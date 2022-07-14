using System.Collections.Generic;
using WebServer.Clients;
using WebServer.DI;
using WebServer.DI.Interfaces;
using WebServer.DI.JSONSerializer;
using WebServer.HttpRequestReaders;
using WebServer.Listeners;
using WebServer.Middlewares;
using WebServer.Middlewares.Interfaces;
using WebServer.Service;
using WebServer.Service.Interfaces;
using WebServer.Storage;

namespace WebServer.DependencyRegistrar
{
    public static class DependencyRegistrar
    {
        public static void RegisterServerDependencies(IServiceCollection collection)
        {
            collection.AddSingleton<IServer, Server>();
            collection.AddSingleton<IListener, TcpListenerAdapter>();
            collection.AddSingleton<CookieStorage>();

            collection.AddTransient<IMiddleware, CookieMiddleware>();
            collection.AddTransient<IManager, HttpManager>();
            collection.AddTransient<ICookieGenerator, CookieGenerator>();
            collection.AddTransient<IHttpRequestReader, HttpRequestReader>();
            var container = collection.BuildContainer();
            collection.AddTransient<IMiddlewareBuilder, MiddlewareBuilder>();
            collection.Add<IDIContainer>(container);
        }
        public static void RegisterServerDependencies(IServiceCollection collection, string name)
        {
            collection.AddSingletonWithName<IServer, Server>(name);
            collection.AddSingletonWithName<IListener, TcpListenerAdapter>(name);
            collection.AddSingletonWithName<CookieStorage>(name);

            collection.AddTransientWithName<IMiddleware, CookieMiddleware>(name);
            collection.AddTransientWithName<IManager, HttpManager>(name);
            collection.AddTransientWithName<ICookieGenerator, CookieGenerator>(name);
            collection.AddTransientWithName<IHttpRequestReader, HttpRequestReader>(name);
            var container = collection.BuildContainer();
            collection.AddTransientWithName<IMiddlewareBuilder,MiddlewareBuilder>(name);
            var middleware = container.GetService<IMiddlewareBuilder>()
                .BuildMiddleware(container, container.GetService<ServerOptions>(name));
                collection.Add<ICollection<IMiddleware>>(middleware,name);
            collection.AddWithName<IDIContainer>(container, name);
        }

        public static void AddConfig<T>(IServiceCollection collection, string path)
        {
            collection.Add(JSONConfigToObjectMapper.MapConfig<T>(path));
        }
        public static void AddConfig<T>(IServiceCollection collection, string path, string dependencyName)
        {
            collection.AddWithName(JSONConfigToObjectMapper.MapConfig<T>(path), dependencyName);
        }
    }
}
