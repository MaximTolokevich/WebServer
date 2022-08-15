using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using FluentAssertions;
using MyDi.DI;
using MyDiTests.EntityMocks;
using WebServer.Clients;
using WebServer.Listeners;
using Xunit;

namespace MyDiTests
{
    public class MyContainerTests
    {
        [Fact]
        public void GetService_Should_Return_Service_IfRegistered()
        {
            // Arrange
            var options = new ServerOptions
            {
                IpAddress = "127.0.0.1",
                Port = 8080
            };
            var providers = new List<MyServiceProvider>
            {
                new(typeof(IListener), typeof(ListenerForTests), ServiceLifeTime.Singleton),
                new(options),
                new(typeof(TcpListener),ServiceLifeTime.Singleton)
            };

            var container = new MyContainer(providers);

            // Act
            var service = container.GetService<IListener>();

            // Assert
            service.Should().NotBeNull().And.BeOfType<ListenerForTests>();
        }
        [Fact]
        public void GetService_Should_Return_The_Same_Object_When_Service_LifeTime_Singleton()
        {
            // Arrange
            var options = new ServerOptions
            {
                IpAddress = "127.0.0.1",
                Port = 8080
            };
            var providers = new List<MyServiceProvider>()
            {
                new(typeof(IListener), typeof(ListenerForTests), ServiceLifeTime.Singleton),
                new(options),
                new(typeof(TcpListener),ServiceLifeTime.Singleton)
            };

            var container = new MyContainer(providers);

            // Act
            var service = container.GetService<IListener>();
            var secondService = container.GetService<IListener>();

            // Assert
            service.Should().NotBeNull().And.BeOfType<ListenerForTests>();

            service.Should().BeEquivalentTo(secondService);
        }
        [Fact]
        public void GetService_Should_Return_The_Various_Object_When_Service_LifeTime_Transient()
        {
            // Arrange
            var options = new ServerOptions
            {
                IpAddress = "127.0.0.1",
                Port = 8080
            };
            var providers = new List<MyServiceProvider>
            {
                new(typeof(IListener), typeof(ListenerForTests), ServiceLifeTime.Transient),
                new(options),
                new(typeof(TcpListener),ServiceLifeTime.Singleton)
            };

            var container = new MyContainer(providers);
            
            // Act
            var service = container.GetService<IListener>();
            var secondService = container.GetService<IListener>();

            // Assert
            service.Should().NotBeNull().And.BeOfType<ListenerForTests>();

            ReferenceEquals(service, secondService).Should().BeFalse();
        }
        [Fact]
        public void GetService_Should_Return_The_Various_Object_When_Service_LifeTime_Singleton_And_Dependency_Name_Are_Different()
        {
            // Arrange
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
                new(typeof(IListener), typeof(ListenerForTests), ServiceLifeTime.Singleton,firstDependencyName),
                new(options,firstDependencyName),
                new(typeof(TcpListener),ServiceLifeTime.Singleton,firstDependencyName),

                new(typeof(IListener), typeof(ListenerForTests), ServiceLifeTime.Singleton,secondDependencyName),
                new(secondOptions,secondDependencyName),
                new(typeof(TcpListener),ServiceLifeTime.Singleton,secondDependencyName)
            };

            var container = new MyContainer(providers);

            // Act
            var service = container.GetService<IListener>(firstDependencyName);
            var secondService = container.GetService<IListener>(secondDependencyName);

            // Assert
            service.Should().NotBeNull().And.BeOfType<ListenerForTests>();
            secondService.Should().NotBeNull().And.BeOfType<ListenerForTests>();

            ReferenceEquals(service, secondService).Should().BeFalse();
        }

        [Fact]
        public void GetService_Should_Return_Last_Service_IfRegistered()
        {
            // Arrange
            var providers = new List<MyServiceProvider>
            {
                new(typeof(IListener), typeof(ListenerForTests), ServiceLifeTime.Singleton),
                new(typeof(IListener), typeof(AnotherListenerForTests), ServiceLifeTime.Singleton),

            };

            var container = new MyContainer(providers);

            // Act
            var service = container.GetService<IListener>();

            // Assert
            service.Should().NotBeNull().And.BeOfType<AnotherListenerForTests>();
        }

        [Fact]
        public void GetAll_Should_Return_All_Registered_Services_By_Type()
        {
            // Arrange
            var providers = new List<MyServiceProvider>
            {
                new(typeof(IListener), typeof(ListenerForTests), ServiceLifeTime.Singleton),
                new(typeof(IListener), typeof(AnotherListenerForTests), ServiceLifeTime.Singleton),

            };

            var container = new MyContainer(providers);

            // Act
            var service = container.GetAll<IListener>().ToArray();

            // Assert
            service.Length.Should().Be(2);
            service[0].Should().BeOfType<ListenerForTests>();
            service[1].Should().BeOfType<AnotherListenerForTests>();
        }
    }
}
