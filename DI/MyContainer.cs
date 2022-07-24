using System;
using System.Collections.Generic;
using System.Linq;
using DI.Exceptions;
using MyDi.DI;
using MyDi.DI.Interfaces;

namespace DI
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