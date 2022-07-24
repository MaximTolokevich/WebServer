using System;
using MyDi.DI;

namespace DI
{
    public class MyServiceProvider
    {
        public MyServiceProvider(Type implType, ServiceLifeTime lifeTime)
        {
            ImplType = implType;
            LifeTime = lifeTime;
        }
        public MyServiceProvider(Type interfaceType, Type implType, ServiceLifeTime lifeTime)
        {
            ImplType = implType;
            LifeTime = lifeTime;
            InterfaceType = interfaceType;
        }

        public MyServiceProvider(object implementation, Type interfaceType)
        {
            Implementation = implementation;
            ImplType = implementation.GetType();
            InterfaceType = interfaceType;
        }
        public MyServiceProvider(object implementation)
        {
            Implementation = implementation;
            ImplType = implementation.GetType();
        }
        public MyServiceProvider(Type implType, ServiceLifeTime lifeTime, string name)
        {
            ImplType = implType;
            LifeTime = lifeTime;
            DependencyName = name;
        }
        public MyServiceProvider(Type interfaceType, Type implType, ServiceLifeTime lifeTime, string name)
        {
            ImplType = implType;
            LifeTime = lifeTime;
            InterfaceType = interfaceType;
            DependencyName = name;
        }
        public MyServiceProvider(Type interfaceType,object obj, ServiceLifeTime lifeTime, string name)
        {
            Implementation = obj;
            LifeTime = lifeTime;
            InterfaceType = interfaceType;
            DependencyName = name;
        }

        public MyServiceProvider(object implementation, Type interfaceType, string name)
        {
            Implementation = implementation;
            ImplType = implementation.GetType();
            DependencyName = name;
            InterfaceType = interfaceType;
        }
        public MyServiceProvider(object implementation, string name)
        {
            Implementation = implementation;
            ImplType = implementation.GetType();
            DependencyName = name;
        }

        public Type ImplType { get; }
        public Type InterfaceType { get; }
        public ServiceLifeTime LifeTime { get; }
        public object Implementation { get; set; }
        public string DependencyName { get; set; }
    }
}