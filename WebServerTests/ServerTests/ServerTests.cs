using Moq;
using MyDi.DI.Interfaces;
using WebServer;
using WebServer.Clients;
using WebServer.Listeners;
using Xunit;

namespace WebServerTests.ServerTests
{
    public class ServerTests
    {
        [Fact]
        public void Should_Start_And_Stop()
        {
            // Arrange
            var listener = new Mock<IListener>();
            var di = new Mock<IDIContainer>();
            var options = new Mock<ServerOptions>();
            var server = new Server(listener.Object, di.Object, options.Object);

            // Act
            server.Start();
            server.Stop();

            // Assert
            listener.Verify(x => x.Start(), Times.Once);
            listener.Verify(x => x.Stop(), Times.Once);

        }
    }
}
