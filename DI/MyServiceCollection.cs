using System;
using System.Collections.Generic;
using System.Linq;
using WebServer.DI.Interfaces;

namespace WebServer.DI
{
    public class MyServiceCollection : IServiceCollection
    {
        private readonly List<MyServiceProvider> _serviceProviders = new();

        public void Add<TInterface>(object obj)
        {
            CheckIsInterfaceType(typeof(TInterface), obj.GetType());
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
            CheckIsConcreteClassType(typeof(TImplementation));
            CheckIsInterfaceType(typeof(TInterface), typeof(TImplementation));
            _serviceProviders.Add(new MyServiceProvider(typeof(TInterface), typeof(TImplementation), ServiceLifeTime.Singleton));
        }

        public void AddTransient<TImplementation>()
        {
            CheckIsConcreteClassType(typeof(TImplementation));
            _serviceProviders.Add(new MyServiceProvider(typeof(TImplementation), ServiceLifeTime.Transient));
        }

        public void AddTransient<TInterface, TImplementation>()
        {
            CheckIsConcreteClassType(typeof(TImplementation));
            CheckIsInterfaceType(typeof(TInterface), typeof(TImplementation));
            _serviceProviders.Add(new MyServiceProvider(typeof(TInterface), typeof(TImplementation), ServiceLifeTime.Transient));
        }
        public void AddWithName<TInterface>(object obj, string name)
        {
            CheckIsInterfaceType(typeof(TInterface), obj.GetType());
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
            CheckIsConcreteClassType(typeof(TImplementation));
            CheckIsInterfaceType(typeof(TInterface), typeof(TImplementation));
            _serviceProviders.Add(new MyServiceProvider(typeof(TInterface), typeof(TImplementation), ServiceLifeTime.Singleton, name));
        }

        public void AddTransientWithName<TImplementation>(string name)
        {
            CheckIsConcreteClassType(typeof(TImplementation));
            _serviceProviders.Add(new MyServiceProvider(typeof(TImplementation), ServiceLifeTime.Transient, name));
        }

        public void AddTransientWithName<TInterface, TImplementation>(string name)
        {
            CheckIsConcreteClassType(typeof(TImplementation));
            CheckIsInterfaceType(typeof(TInterface), typeof(TImplementation));
            _serviceProviders.Add(new MyServiceProvider(typeof(TInterface), typeof(TImplementation), ServiceLifeTime.Transient, name));
        }

        private static void CheckIsInterfaceType(Type type, Type objType)
        {
            if (!type.IsInterface)
            {
                throw new ArgumentException("Should be Interface type", nameof(type));
            }

            var interfaces = objType.GetInterfaces();
            if (interfaces.All(x => x != type))
            {
                throw new ArgumentException($"{objType} not implement {type}");
            }
        }
        private static void CheckIsConcreteClassType(Type type)
        {
            if (!type.IsClass || type.IsAbstract)
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
