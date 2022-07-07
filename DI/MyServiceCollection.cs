using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.DI
{
    public class MyServiceCollection
    {
        private readonly List<MyServiceProvider> serviceProviders = new();

        public void Add(object obj)
        {
            serviceProviders.Add(new MyServiceProvider(obj));
        }

        public void AddSingleton<T>()
        {
            serviceProviders.Add(new MyServiceProvider(typeof(T), ServiceLifeTime.Singleton));
        }
        public void AddSingleton<T, V>()
        {
            serviceProviders.Add(new MyServiceProvider(typeof(T), typeof(V), ServiceLifeTime.Singleton));
        }

        public void AddTransient<T>()
        {
            serviceProviders.Add(new MyServiceProvider(typeof(T), ServiceLifeTime.Transient));
        }

        public void AddTransient<T, V>()
        {
            serviceProviders.Add(new MyServiceProvider(typeof(T), typeof(V), ServiceLifeTime.Transient));
        }
        public void AddWithName(object obj,string name)
        {
            serviceProviders.Add(new MyServiceProvider(obj, name));
        }

        public void AddSingletonWithName<T>(string name)
        {
            serviceProviders.Add(new MyServiceProvider(typeof(T), ServiceLifeTime.Singleton, name));
        }
        public void AddSingletonWithName<T, V>(string name)
        {
            serviceProviders.Add(new MyServiceProvider(typeof(T), typeof(V), ServiceLifeTime.Singleton, name));
        }

        public void AddTransientWithName<T>(string name)
        {
            serviceProviders.Add(new MyServiceProvider(typeof(T), ServiceLifeTime.Transient, name));
        }

        public void AddTransientWithName<T, V>(string name)
        {
            serviceProviders.Add(new MyServiceProvider(typeof(T), typeof(V), ServiceLifeTime.Transient, name));
        }
        public MyContainer BuildContainer()
        {
            return new MyContainer(serviceProviders);
        }
    }
}
