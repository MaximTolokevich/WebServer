using MyDi.DI.ConfigMapper;
using MyDi.DI.Interfaces;
using WebServer.HttpRequestReaders;
using WebServer.Listeners;
using WebServer.Service;
using WebServer.Service.Interfaces;
using WebServer.Writers;

namespace WebServer.DependencyRegistrars
{
    public static class DependencyRegistrar
    {
        public static void RegisterHttpServerDependencies(this IServiceCollection collection, string name = null)
        {
            collection.AddSingleton<IServer, Server>(name);
            collection.AddSingleton<IListener, TcpListenerAdapter>(name);

            collection.AddTransient<IManager, HttpManager>(name);
            collection.AddTransient<IClientWriter, RequestWriter>(name);
            collection.AddTransient<IHttpRequestReader, HttpRequestReader>(name);
            var container = collection.BuildContainer();

            collection.Add<IDIContainer>(container, name);
        }
        public static void AddConfig<T>(this IServiceCollection collection, string path, string dependencyName = null)
        {
            collection.Add(JSONConfigToObjectMapper.MapConfig<T>(path), dependencyName);
        }
    }
}
