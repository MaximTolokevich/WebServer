using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Text;
using WebServer.Clients;
using WebServer.Models;
using Xunit;

namespace WebServerTests.ModelsTests
{
    public class MyHttpResponseTests
    {
        [Fact]
        public void Build_Should_Correctly_Build_Response()
        {
            // Arrange
            var options = new Mock<ServerOptions>
            {
                Object =
                {
                    ServerName = "Server"
                }
            };
            var httpResponse = new MyHttpResponse(options.Object)
            {
                Version = "HTTP/1.1",
                StatusCode = "200 OK"
            };
            httpResponse.Headers.Add("header", "some Header");
            httpResponse.CookieList = new List<string>()
            {
                "cookie=someCookie"
            };
            httpResponse.Body = Encoding.UTF8.GetBytes("Some Body");
            httpResponse.ContentType = "text/html; charset=UTF-8";

            // Act
            var dataAsByteArray = httpResponse.Build();

            // Assert
            var actualResponse = "HTTP/1.1 200 OK" +
                                 "\r\nServer: Server" +
                                 "\r\nContent-Type: text/html; charset=UTF-8" +
                                 "\r\nContent-Length: 9" +
                                 "\r\nSet-Cookie: cookie=someCookie" +
                                 "\r\nheader: some Header" +
                                 "\r\n\r\nSome Body";
            var actualResponseAsByteArray = Encoding.UTF8.GetBytes(actualResponse);
            dataAsByteArray.Should().BeEquivalentTo(actualResponseAsByteArray);

        }
    }
}
