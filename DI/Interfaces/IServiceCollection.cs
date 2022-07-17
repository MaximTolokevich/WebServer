using System.Collections.Generic;

namespace WebServer.DI.Interfaces
{
    public interface IServiceCollection
    {
        void AddTransientWithName<T, V>(string name);
        void AddTransientWithName<T>(string name);
        void AddSingletonWithName<T, V>(string name);
        void AddSingletonWithName<T>(string name);
        void AddWithName(object obj, string name);
        void AddWithName<TInterface>(object obj, string name);
        void AddTransient<T, V>();
        void AddTransient<T>();
        void AddSingleton<T, V>();
        void AddSingleton<T>();
        void Add(object obj);
        void Add<TInterface>(object obj);
        MyContainer BuildContainer();
    }
}