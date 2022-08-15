using FluentAssertions;
using System;
using System.Net;
using WebServer.Clients;
using WebServer.Listeners;
using Xunit;

namespace WebServerTests.ListenersTests
{
    public class TcpListenerAdapterTests
    {
        [Fact]
        public void Ctor_Should_Not_Throw_When_ServerOptions_Valid()
        {
            // Arrange
            var options = new ServerOptions()
            {
                IpAddress = "127.0.0.1",
                Port = 8080
            };

            // Act
            Action act = () => new TcpListenerAdapter(options);

            // Assert
            act.Should().NotThrow();
        }
        [Fact]
        public void Ctor_Should_Throw_ArgumentException_When_ServerOptions_IpAddress_Invalid()
        {
            // Arrange
            var options = new ServerOptions()
            {
                IpAddress = "qwe",
                Port = 8080
            };

            // Act
            Action act = () => new TcpListenerAdapter(options);

            // Assert
            act.Should().Throw<ArgumentException>().WithMessage("is not valid (Parameter 'IpAddress')");
        }
        [Fact]
        public void Ctor_Should_Throw_ArgumentOutOfRangeException_When_ServerOptions_Port_Invalid()
        {
            // Arrange
            var options = new ServerOptions()
            {
                IpAddress = "127.0.0.1",
                Port = 245162315
            };

            // Act
            Action act = () => new TcpListenerAdapter(options);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>().WithMessage($"can't be less then {IPEndPoint.MinPort} and more then {IPEndPoint.MaxPort} (Parameter 'Port')");
        }
        [Fact]
        public void Ctor_Should_Throw_ArgumentNullException_When_ServerOptions_Null()
        {
            // Arrange & Act
            Action act = () => new TcpListenerAdapter(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("can't be null (Parameter 'options')");
        }
    }
}
