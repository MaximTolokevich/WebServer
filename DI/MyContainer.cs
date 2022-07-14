using System;
using System.Collections.Generic;
using System.Linq;
using WebServer.DI.Exceptions;
using WebServer.DI.Interfaces;

namespace WebServer.DI
{
    public class MyContainer : IDIContainer
    {
        public MyContainer(IList<MyServiceProvider> serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        private readonly IList<MyServiceProvider> _serviceProviders;

        private object GetService(Type serviceType)
        {
            if (serviceType.IsInterface)
            {
                var provider = _serviceProviders.LastOrDefault(x => x.InterfaceType == serviceType);

                CheckProviderExist(provider,serviceType);
                if (provider.Implementation is not null)
                {
                    return provider.Implementation;
                }
                var impl = CreateInstance(provider.ImplType, GetConstructorParameters(provider));
                if (provider.LifeTime is ServiceLifeTime.Singleton)
                {
                    provider.Implementation = impl;
                }

                return impl;
            }
            else
            {
                var provider = _serviceProviders.LastOrDefault(x => x.ImplType == serviceType);

                CheckProviderExist(provider, serviceType);

                if (provider.Implementation is not null)
                {
                    return provider.Implementation;
                }

                var parameters = GetConstructorParameters(provider);
                var implementation = CreateInstance(serviceType, parameters);

                if (provider.LifeTime is ServiceLifeTime.Singleton)
                {
                    provider.Implementation = implementation;
                }

                return implementation;
            }
            

        }
        public T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }
        private object GetService(Type serviceType, string name)
        {
            if (serviceType.IsInterface)
            {
                var provider = _serviceProviders.LastOrDefault(x => x.InterfaceType == serviceType && x.DependencyName.Equals(name));
                
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
            else
            {
                var provider = _serviceProviders.LastOrDefault(x => x.ImplType == serviceType && x.DependencyName.Equals(name));
                
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
        }
        public T GetService<T>(string name)
        {
            return (T)GetService(typeof(T), name);
        }

        private IEnumerable<object> GetConstructorParameters(MyServiceProvider provider)
        {
            var constructInfo = provider.ImplType.GetConstructors().First();
            return constructInfo.GetParameters().Select(x => GetService(x.ParameterType)).ToArray();
        }
        private IEnumerable<object> GetConstructorParameters(MyServiceProvider provider, string name)
        {
            var constructInfo = provider.ImplType.GetConstructors().First();
            return constructInfo.GetParameters().Select(x => GetService(x.ParameterType, name)).ToArray();
        }

        private static object CreateInstance(Type type, IEnumerable<object> param)
        {
            return Activator.CreateInstance(type, param.ToArray());
        }

        private void CheckProviderExist(MyServiceProvider provider, Type type)
        {
            if (provider is null)
            {
                throw new DependencyNotRegisteredException($"Dependency of type{type} not registered");
            }
        }
        private void CheckProviderWithNameExist(MyServiceProvider provider, Type type, string name)
        {
            if (provider is null)
            {
                throw new DependencyNotRegisteredException($"Dependency of type{type} with name{name} not registered");
            }
        }
    }
}