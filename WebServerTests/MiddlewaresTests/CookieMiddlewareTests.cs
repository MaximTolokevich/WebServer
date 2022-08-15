using FluentAssertions;
using Moq;
using WebServer.Clients;
using WebServer.Middlewares;
using WebServer.Models;
using WebServer.Service.Interfaces;
using WebServer.Storage;
using Xunit;

namespace WebServerTests.MiddlewaresTests
{
    public class CookieMiddlewareTests
    {
        [Fact]
        public void Invoke_Should_Set_Cookie_When_Context_Not_Contain_MyCookie_Cookie()
        {
            // Arrange
            const string cookieValue = "CookieKey=CookieValue";
            var cookieGeneratorMock = new Mock<ICookieGenerator>();
            cookieGeneratorMock.Setup(x => x.GenerateCookie()).Returns(cookieValue);
            var cookieStorageMock = new Mock<CookieStorage>();
            var middleware = new CookieMiddleware(cookieGeneratorMock.Object, cookieStorageMock.Object);
            var optionsMock = new Mock<ServerOptions>();
            var httpContext = new Mock<MyHttpContext>(optionsMock.Object);


            // Act
            middleware.Invoke(httpContext.Object);

            // Arrange
            httpContext.Object.HttpResponse.CookieList.Should().Contain(cookieValue);
        }
        [Fact]
        public void Invoke_Should_Set_Cookie_When_Context_Contain_MyCookie_Cookie()
        {
            // Arrange
            const string cookieValue = "CookieKey=CookieValue";
            var cookieGeneratorMock = new Mock<ICookieGenerator>();
            var cookieStorageMock = new Mock<CookieStorage>();

            cookieStorageMock.Object.CookieDictionary.TryAdd(cookieValue, 1);

            var middleware = new CookieMiddleware(cookieGeneratorMock.Object, cookieStorageMock.Object);
            var optionsMock = new Mock<ServerOptions>();
            var httpContext = new MyHttpContext(optionsMock.Object);
            httpContext.HttpRequest.Headers.Add("Cookie", cookieValue);

            // Act
            middleware.Invoke(httpContext);

            // Arrange
            httpContext.HttpResponse.CookieList.Should().NotContain(cookieValue);
        }
    }
}
