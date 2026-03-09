using Moq;
using System.Collections.Generic;
using System.Net;
using MyDi.DI.Interfaces;
using WebServer.Clients;
using WebServer.HttpRequestReaders;
using WebServer.Middlewares.Interfaces;
using WebServer.Models;
using WebServer.Service;
using Xunit;

namespace WebServerTests.ServiceTests
{
    public class HttpManagerTests
    {
        [Fact]
        public void Manage_Should_Call_All_Dependencies()
        {
            // Arrange
            var optionsMock = new Mock<ServerOptions>();
            var requestReaderMock = new Mock<IHttpRequestReader>();

            var httpContext = new MyHttpContext(optionsMock.Object)
            {
                HttpRequest = new MyHttpRequest()
                {
                    Headers = new Dictionary<string, string>(),
                }
            };

            requestReaderMock.Setup(x => x.Read(It.IsAny<IClient>())).Returns(httpContext.HttpRequest);

            var middlewareMock = new Mock<IMiddleware>();
            middlewareMock.Setup(x => x.Invoke(httpContext)).Returns(httpContext);

            var container = new Mock<IDIContainer>();
            container.Setup(x => x.GetAll<IMiddleware>(null)).Returns(new List<IMiddleware>()
            {
                middlewareMock.Object
            });

            var clientMock = new Mock<IClient>();
            clientMock.Setup(x => x.GetClientInfo()).Returns(new IPAddress(123213213));

            var httpManagerMock = new HttpManager(requestReaderMock.Object, optionsMock.Object, container.Object);

            // Act
            httpManagerMock.Manage(clientMock.Object);

            // Assert
            middlewareMock.Verify(x => x.Invoke(It.IsAny<MyHttpContext>()), Times.Once);
            clientMock.Verify(x => x.SendResponse(It.IsAny<byte[]>()), Times.Once);
            requestReaderMock.Verify(x => x.Read(It.IsAny<IClient>()), Times.Once);
        }
    }
}
