using MyDi.DI.Exceptions;
using MyDi.DI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyDi.DI
{
    public class MyContainer : IDIContainer
    {
        public MyContainer(IList<MyServiceProvider> serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        private readonly IList<MyServiceProvider> _serviceProviders;

        private object GetService(Type serviceType, string name = null)
        {
            return serviceType.IsInterface ?
                GetImplementationByInterface(_serviceProviders, serviceType, name)
                : GetImplementation(_serviceProviders, serviceType, name);
        }

        public T GetService<T>(string name = null)
        {
            return (T)GetService(typeof(T), name);
        }

        private IEnumerable<T> GetAll<T>(Type serviceType, string name = null)
        {
            return serviceType.IsInterface ?
                GetAllImplementationsByInterface<T>(_serviceProviders, serviceType, name)
                : GetAllImplementations<T>(_serviceProviders, serviceType, name);
        }
        public IEnumerable<T> GetAll<T>(string name = null)
        {
            return GetAll<T>(typeof(T), name);
        }

        private IEnumerable<object> GetConstructorParameters(MyServiceProvider provider, string name)
        {
            var constructInfo = provider.ImplType.GetConstructors().First();
            return constructInfo.GetParameters().Select(x => GetService(x.ParameterType, name)).ToArray();
        }

        private object GetImplementationByInterface(IEnumerable<MyServiceProvider> providers, Type serviceType, string name = null)
        {
            var provider = providers.LastOrDefault(x => x.InterfaceType == serviceType && x.DependencyName == name);

            CheckProviderWithNameExist(provider, serviceType, name);

            if (provider.Implementation is not null)
            {
                return provider.Implementation;
            }

            var impl = CreateInstance(provider.ImplType, GetConstructorParameters(provider, name));

            if (provider.LifeTime is ServiceLifeTime.Singleton)
            {
                provider.Implementation = impl;
            }

            return impl;
        }

        private object GetImplementation(IEnumerable<MyServiceProvider> providers, Type serviceType, string name = null)
        {
            var provider = providers.LastOrDefault(x => x.ImplType == serviceType && x.DependencyName == name);

            CheckProviderWithNameExist(provider, serviceType, name);

            if (provider.Implementation is not null)
            {
                return provider.Implementation;
            }

            var parameters = GetConstructorParameters(provider, name);
            var implementation = CreateInstance(serviceType, parameters);

            if (provider.LifeTime is ServiceLifeTime.Singleton)
            {
                provider.Implementation = implementation;
            }

            return implementation;
        }

        private IEnumerable<T> GetAllImplementationsByInterface<T>(IEnumerable<MyServiceProvider> providers, Type serviceType, string name = null)
        {
            var collection = new List<T>();
            var providersByType = providers.Where(x => x.InterfaceType == serviceType && x.DependencyName == name).ToArray();
            foreach (var item in providersByType)
            {
                if (item.Implementation is not null)
                {
                    collection.Add((T)item.Implementation);
                }
                else
                {
                    var impl = CreateInstance(item.ImplType, GetConstructorParameters(item, name));

                    if (item.LifeTime is ServiceLifeTime.Singleton)
                    {
                        item.Implementation = impl;
                    }

                    collection.Add((T)impl);
                }
            }

            return collection;
        }

        private IEnumerable<T> GetAllImplementations<T>(IEnumerable<MyServiceProvider> providers, Type serviceType,
            string name = null)
        {
            var collection = new List<T>();
            var providersByType = providers.Where(x => x.ImplType == serviceType && x.DependencyName == name).ToArray();
            foreach (var item in providersByType)
            {
                if (item.Implementation is not null)
                {
                    collection.Add((T)item.Implementation);
                }
                else
                {
                    var impl = CreateInstance(item.ImplType, GetConstructorParameters(item, name));

                    if (item.LifeTime is ServiceLifeTime.Singleton)
                    {
                        item.Implementation = impl;
                    }

                    collection.Add((T)impl);
                }
            }

            return collection;
        }

        private static object CreateInstance(Type type, IEnumerable<object> param)
        {
            return Activator.CreateInstance(type, param.ToArray());
        }

        private static void CheckProviderWithNameExist(MyServiceProvider provider, Type type, string name)
        {
            if (provider is null)
            {
                throw new DependencyNotRegisteredException($"Dependency of type{type} with name {name} not registered");
            }
        }
    }
}