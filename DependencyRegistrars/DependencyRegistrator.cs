using System.Collections.Generic;
using MyDi.DI.ConfigMapper;
using MyDi.DI.Interfaces;
using WebServer.HttpRequestReaders;
using WebServer.Listeners;
using WebServer.Middlewares;
using WebServer.Middlewares.Interfaces;
using WebServer.Service;
using WebServer.Service.Interfaces;

namespace WebServer.DependencyRegistrars
{
    public static class DependencyRegistrar
    {
        public static void RegisterHttpServerDependencies(IServiceCollection collection, string name= null)
        {
            collection.AddSingleton<IServer, Server>(name);
            collection.AddSingleton<IListener, TcpListenerAdapter>(name);

            collection.AddTransient<IManager, HttpManager>(name);

            collection.AddTransient<IHttpRequestReader, HttpRequestReader>(name);
            var container = collection.BuildContainer();

            collection.Add<IDIContainer>(container, name);
            collection.AddTransient<ICollection<IMiddleware>>(MiddlewareBuilder.Build(collection, container, name), name);
            
        }
        public static void AddConfig<T>(IServiceCollection collection, string path, string dependencyName = null)
        {
            collection.Add(JSONConfigToObjectMapper.MapConfig<T>(path), dependencyName);
        }
    }
}
