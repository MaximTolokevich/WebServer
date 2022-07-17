using System;
using System.Collections.Generic;
using WebServer.DI.Interfaces;

namespace WebServer.DI
{
    public class MyServiceCollection:IServiceCollection
    {
        private readonly List<MyServiceProvider> _serviceProviders = new();

        public void Add<TInterface>(object obj)
        {
            CheckIsInterfaceType(typeof(TInterface));
            _serviceProviders.Add(new MyServiceProvider(obj, typeof(TInterface)));
        }
        public void Add(object obj)
        {
            _serviceProviders.Add(new MyServiceProvider(obj));
        }
        public void Add(object obj, string name)
        {
            _serviceProviders.Add(new MyServiceProvider(obj, name));
        }

        public void AddSingleton<TImplementation>()
        {
            CheckIsConcreteClassType(typeof(TImplementation));
            _serviceProviders.Add(new MyServiceProvider(typeof(TImplementation), ServiceLifeTime.Singleton));
        }
        public void AddSingleton<TInterface, TImplementation>()
        {
            CheckIsInterfaceType(typeof(TInterface));
            CheckIsConcreteClassType(typeof(TImplementation));
            _serviceProviders.Add(new MyServiceProvider(typeof(TInterface), typeof(TImplementation), ServiceLifeTime.Singleton));
        }

        public void AddTransient<TImplementation>()
        {
            CheckIsConcreteClassType(typeof(TImplementation));
            _serviceProviders.Add(new MyServiceProvider(typeof(TImplementation), ServiceLifeTime.Transient));
        }

        public void AddTransient<TInterface, TImplementation>()
        {
            CheckIsInterfaceType(typeof(TInterface));
            CheckIsConcreteClassType(typeof(TImplementation));
            _serviceProviders.Add(new MyServiceProvider(typeof(TInterface), typeof(TImplementation), ServiceLifeTime.Transient));
        }
        public void AddWithName<TInterface>(object obj, string name)
        {
            CheckIsInterfaceType(typeof(TInterface));
            _serviceProviders.Add(new MyServiceProvider(obj, typeof(TInterface), name));
        }
        public void AddWithName(object obj, string name)
        {
            _serviceProviders.Add(new MyServiceProvider(obj, name));
        }

        public void AddSingletonWithName<TImplementation>(string name)
        {
            CheckIsConcreteClassType(typeof(TImplementation));
            _serviceProviders.Add(new MyServiceProvider(typeof(TImplementation), ServiceLifeTime.Singleton, name));
        }
        public void AddSingletonWithName<TInterface, TImplementation>(string name)
        {
            CheckIsInterfaceType(typeof(TInterface));
            CheckIsConcreteClassType(typeof(TImplementation));
            _serviceProviders.Add(new MyServiceProvider(typeof(TInterface), typeof(TImplementation), ServiceLifeTime.Singleton, name));
        }

        public void AddTransientWithName<TImplementation>(string name)
        {
            CheckIsConcreteClassType(typeof(TImplementation));
            _serviceProviders.Add(new MyServiceProvider(typeof(TImplementation), ServiceLifeTime.Transient, name));
        }

        public void AddTransientWithName<TInterface, TImplementation>(string name)
        {
            CheckIsInterfaceType(typeof(TInterface));
            CheckIsConcreteClassType(typeof(TImplementation));
            _serviceProviders.Add(new MyServiceProvider(typeof(TInterface), typeof(TImplementation), ServiceLifeTime.Transient, name));
        }

        private static void CheckIsInterfaceType(Type type)
        {
            if (!type.IsInterface)
            {
                throw new ArgumentException("Should be Interface type", nameof(type));
            }
        }
        private static void CheckIsConcreteClassType(Type type)
        {
            if (!type.IsClass || type.IsAbstract || type.IsEnum)
            {
                throw new ArgumentException("Should be concrete class type", nameof(type));
            }
        }
        public MyContainer BuildContainer()
        {
            return new MyContainer(_serviceProviders);
        }
    }
}
