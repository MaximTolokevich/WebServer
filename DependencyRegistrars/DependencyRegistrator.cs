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

namespace WebServer.DependencyRegistrars
{
    public static class DependencyRegistrar
    {
        public static void RegisterHttpServerDependencies(IServiceCollection collection)
        {
            collection.AddSingleton<IServer, Server>();
            collection.AddSingleton<IListener, TcpListenerAdapter>();

            collection.AddTransient<IManager, HttpManager>();
            
            collection.AddTransient<IHttpRequestReader, HttpRequestReader>();
            var container = collection.BuildContainer();

            collection.Add<IDIContainer>(container);
        }
        public static void RegisterHttpServerDependencies(IServiceCollection collection, string name)
        {
            collection.AddSingletonWithName<IServer, Server>(name);
            collection.AddSingletonWithName<IListener, TcpListenerAdapter>(name);

            collection.AddTransientWithName<IManager, HttpManager>(name);
            
            collection.AddTransientWithName<IHttpRequestReader, HttpRequestReader>(name);
            var container = collection.BuildContainer();

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
