using System;
using System.Collections.Generic;
using System.Linq;

namespace WebServer.DI
{
    public class MyContainer :IDIContainer
    {
        public MyContainer(List<MyServiceProvider> serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        private List<MyServiceProvider> _serviceProviders { get; }

        private object GetService(Type serviceType)
        {
            if (serviceType.IsInterface)
            {
                var provider1 = _serviceProviders.FirstOrDefault(x => x.InterfaceType == serviceType);
                return provider1.Implementation ?? CreateInstance(provider1.ImplType, GetConstructorParameters(provider1));
            }
            var provider = _serviceProviders.FirstOrDefault(x => x.ImplType == serviceType);
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
        public T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }
        private object GetService(Type serviceType, string name)
        {
            if (serviceType.IsInterface)
            {
                var provider1 = _serviceProviders.FirstOrDefault(x => x.InterfaceType == serviceType);
                return provider1.Implementation ?? CreateInstance(provider1.ImplType, GetConstructorParameters(provider1));
            }
            var provider = _serviceProviders.FirstOrDefault(x => x.ImplType == serviceType);
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
        public T GetService<T>(string name)
        {
            return (T)GetService(typeof(T), name);
        }

        private IEnumerable<object> GetConstructorParameters(MyServiceProvider provider)
        {
            var constructInfo = provider.ImplType.GetConstructors().First();
            return constructInfo.GetParameters().Select(x => GetService(x.ParameterType)).ToArray();
        }

        private object CreateInstance(Type type, IEnumerable<object> param)
        {
           return  Activator.CreateInstance(type, param.ToArray());
        }
    }
}