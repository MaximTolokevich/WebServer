using System.Collections.Generic;
using System.Net.Sockets;
using DI;
using DiTests.EntityHelper;
using FluentAssertions;
using Server.Clients;
using Server.Listeners;
using Xunit;

namespace DiTests
{
    public class MyContainerTests
    {
        [Fact]
        public void GetService_Should_Return_Service_IfRegistered()
        {
            var options = new ServerOptions
            {
                IpAddress = "127.0.0.1",
                Port = 8080
            };
            var providers = new List<MyServiceProvider>
            {
                new(typeof(IListener), typeof(TcpListenerAdapter), ServiceLifeTime.Singleton),
                new(options),
                new(typeof(TcpListener),ServiceLifeTime.Singleton)
            };


            var container = new MyContainer(providers);
            var service = container.GetService<IListener>();
            service.Should().NotBeNull().And.BeOfType<TcpListenerAdapter>();
        }
        [Fact]
        public void GetService_Should_Return_The_Same_Object_When_Dependency_LifeCycle_Singleton()
        {
            var options = new ServerOptions
            {
                IpAddress = "127.0.0.1",
                Port = 8080
            };
            var providers = new List<MyServiceProvider>()
            {
                new(typeof(IListener), typeof(TcpListenerAdapter), ServiceLifeTime.Singleton),
                new(options),
                new(typeof(TcpListener),ServiceLifeTime.Singleton)
            };



            var container = new MyContainer(providers);
            var service = container.GetService<IListener>();
            var secondService = container.GetService<IListener>();

            service.Should().NotBeNull().And.BeOfType<TcpListenerAdapter>();

            service.Should().BeEquivalentTo(secondService);
        }
        [Fact]
        public void GetService_Should_Return_The_Various_Object_When_Dependency_LifeCycle_Transient()
        {
            var options = new ServerOptions
            {
                IpAddress = "127.0.0.1",
                Port = 8080
            };
            var providers = new List<MyServiceProvider>
            {
                new(typeof(IListener), typeof(TcpListenerAdapter), ServiceLifeTime.Transient),
                new(options),
                new(typeof(TcpListener),ServiceLifeTime.Singleton)
            };



            var container = new MyContainer(providers);
            var service = container.GetService<IListener>();
            var secondService = container.GetService<IListener>();

            service.Should().NotBeNull().And.BeOfType<TcpListenerAdapter>();

            ReferenceEquals(service, secondService).Should().BeFalse();
        }
        [Fact]
        public void GetService_Should_Return_The_Various_Object_When_Dependency_LifeCycle_Singleton_And_Dependency_Name_Are_Different()
        {
            var firstDependencyName = "first dependency name";
            var secondDependencyName = "second dependency name";
            var options = new ServerOptions
            {
                IpAddress = "127.0.0.1",
                Port = 8080,
                DependencyGroupName = firstDependencyName
            };
            var secondOptions = new ServerOptions
            {
                IpAddress = "127.0.0.1",
                Port = 8080,
                DependencyGroupName = secondDependencyName
            };
            var providers = new List<MyServiceProvider>
            {
                new(typeof(IListener), typeof(TcpListenerAdapter), ServiceLifeTime.Singleton,firstDependencyName),
                new(options,firstDependencyName),
                new(typeof(TcpListener),ServiceLifeTime.Singleton,firstDependencyName),

                new(typeof(IListener), typeof(TcpListenerAdapter), ServiceLifeTime.Singleton,secondDependencyName),
                new(secondOptions,secondDependencyName),
                new(typeof(TcpListener),ServiceLifeTime.Singleton,secondDependencyName)
            };



            var container = new MyContainer(providers);
            var service = container.GetService<IListener>(firstDependencyName);
            var secondService = container.GetService<IListener>(secondDependencyName);

            service.Should().NotBeNull().And.BeOfType<TcpListenerAdapter>();

            ReferenceEquals(service, secondService).Should().BeFalse();
        }

        [Fact]
        public void GetService_Should_Return_Last_Service_IfRegistered()
        {

            var providers = new List<MyServiceProvider>
            {
                new(typeof(IListener), typeof(TcpListenerAdapter), ServiceLifeTime.Singleton),
                new(typeof(IListener), typeof(AnotherListener), ServiceLifeTime.Singleton),

            };


            var container = new MyContainer(providers);
            var service = container.GetService<IListener>();
            service.Should().NotBeNull().And.BeOfType<AnotherListener>();
        }
    }
}
