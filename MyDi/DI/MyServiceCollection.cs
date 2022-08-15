using System;
using System.Collections.Generic;
using System.Linq;
using MyDi.DI.Interfaces;

namespace MyDi.DI
{
    public class MyServiceCollection : IServiceCollection
    {
        private readonly IList<MyServiceProvider> _serviceProviders;

        public MyServiceCollection(IList<MyServiceProvider> providers)
        {
            _serviceProviders = providers;
        }

        public void Add<TInterface>(object obj, string name = null)
        {
            CheckIsInterfaceType(typeof(TInterface), obj.GetType());
            _serviceProviders.Add(new MyServiceProvider(obj, typeof(TInterface),name));
        }
        public void Add(object obj, string name = null)
        {
            _serviceProviders.Add(new MyServiceProvider(obj, name));
        }

        public void AddSingleton<TImplementation>(string name = null)
        {
            CheckIsConcreteClassType(typeof(TImplementation));
            _serviceProviders.Add(new MyServiceProvider(typeof(TImplementation), ServiceLifeTime.Singleton,name));
        }
        public void AddSingleton<TInterface, TImplementation>(string name = null)
        {
            CheckIsConcreteClassType(typeof(TImplementation));
            CheckIsInterfaceType(typeof(TInterface), typeof(TImplementation));
            _serviceProviders.Add(new MyServiceProvider(typeof(TInterface), typeof(TImplementation), ServiceLifeTime.Singleton,name));
        }

        public void AddTransient<TImplementation>(string name = null)
        {
            CheckIsConcreteClassType(typeof(TImplementation));
            _serviceProviders.Add(new MyServiceProvider(typeof(TImplementation), ServiceLifeTime.Transient, name));
        }
        public void AddTransient<TInterface>(object obj, string name = null)
        {
            CheckIsInterfaceType(typeof(TInterface),obj.GetType());
            _serviceProviders.Add(new MyServiceProvider(typeof(TInterface),obj, ServiceLifeTime.Transient, name));
        }

        public void AddTransient<TInterface, TImplementation>(string name = null)
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
        public IDIContainer BuildContainer()
        {
            return new MyContainer(_serviceProviders);
        }
    }
}
