using FluentAssertions;
using Moq;
using Server.Clients;
using Server.Exceptions;
using Server.Models;
using System;
using System.Text;
using Xunit;

namespace WebServerTests.ModelsTests
{
    public class MyHttpRequestTests
    {
        [Fact]
        public void Build_Should_Throw_InvalidRequestExceptions_When_Byte_Array_Empty()
        {
            // Arrange
            var data = Array.Empty<byte>();
            var optionsMock = new Mock<ServerOptions>();
            // Act
            Func<MyHttpRequest> action = () => MyHttpRequest.Build(data, optionsMock.Object);

            // Assert
            action.Should().Throw<InvalidRequestException>();
        }

        [Fact]
        public void Build_Should_Correctly_Parse_Request_Without_Body_To_Object_Model()
        {
            // Act
            var optionsMock = new Mock<ServerOptions>
            {
                Object =
                {
                    Port = 8080
                }
            };

            var request = MyHttpRequest.Build(GetRequestStringAsByteArrayWithoutBody(), optionsMock.Object);

            // Assert
            request.Body.Length.Should().Be(0);
            request.Headers.Count.Should().Be(16);
            request.Method.Should().Be("GET");
            request.Uri.AbsoluteUri.Should().Be("http://localhost:8080/");
            request.Uri.Port.Should().Be(8080);
            request.Uri.AbsolutePath.Should().Be("/");
        }
        [Fact]
        public void Build_Should_Correctly_Parse_Request_With_Body_To_Object_Model()
        {
            // Act
            var optionsMock = new Mock<ServerOptions>
            {
                Object =
                {
                    Port = 8080
                }
            };

            var request = MyHttpRequest.Build(GetRequestStringAsByteArrayWithBody(), optionsMock.Object);

            // Assert
            request.Headers.Count.Should().Be(16);
            request.Method.Should().Be("GET");
            request.Uri.AbsoluteUri.Should().Be("http://localhost:8080/");
            request.Uri.Port.Should().Be(8080);
            request.Uri.AbsolutePath.Should().Be("/");
            request.Body.Length.Should().Be(4);
        }


        public static byte[] GetRequestStringAsByteArrayWithoutBody()
        {
            return Encoding.UTF8.GetBytes("GET / HTTP/1.1" +
                                          "\r\nHost: localhost:8080" +
                                          "\r\nConnection: keep-alive" +
                                          "\r\nCache-Control: max-age=0" +
                                          "\r\nsec-ch-ua: \".Not/A)Brand\";v=\"99\", \"Google Chrome\";v=\"103\", \"Chromium\";v=\"103\"" +
                                          "\r\nsec-ch-ua-mobile: ?0" +
                                          "\r\nsec-ch-ua-platform: \"Windows\"" +
                                          "\r\nUpgrade-Insecure-Requests: 1" +
                                          "\r\nUser-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.0.0 Safari/537.36" +
                                          "\r\nAccept: text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9" +
                                          "\r\nSec-Fetch-Site: none" +
                                          "\r\nSec-Fetch-Mode: navigate" +
                                          "\r\nSec-Fetch-User: ?1" +
                                          "\r\nSec-Fetch-Dest: document" +
                                          "\r\nAccept-Encoding: gzip, deflate, br" +
                                          "\r\nAccept-Language: en-US,en;q=0.9,ru;q=0.8" +
                                          "\r\nCookie: fd49d19b-f181-4935-8b48-4a67187c97f4=e87dd849-0ffa-44a0-95bb-6af79c02eac7");
        }
        public static byte[] GetRequestStringAsByteArrayWithBody()
        {
            return Encoding.UTF8.GetBytes("GET / HTTP/1.1" +
                                          "\r\nHost: localhost:8080" +
                                          "\r\nConnection: keep-alive" +
                                          "\r\nCache-Control: max-age=0" +
                                          "\r\nsec-ch-ua: \".Not/A)Brand\";v=\"99\", \"Google Chrome\";v=\"103\", \"Chromium\";v=\"103\"" +
                                          "\r\nsec-ch-ua-mobile: ?0" +
                                          "\r\nsec-ch-ua-platform: \"Windows\"" +
                                          "\r\nUpgrade-Insecure-Requests: 1" +
                                          "\r\nUser-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.0.0 Safari/537.36" +
                                          "\r\nAccept: text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9" +
                                          "\r\nSec-Fetch-Site: none" +
                                          "\r\nSec-Fetch-Mode: navigate" +
                                          "\r\nSec-Fetch-User: ?1" +
                                          "\r\nSec-Fetch-Dest: document" +
                                          "\r\nAccept-Encoding: gzip, deflate, br" +
                                          "\r\nAccept-Language: en-US,en;q=0.9,ru;q=0.8" +
                                          "\r\nCookie: fd49d19b-f181-4935-8b48-4a67187c97f4=e87dd849-0ffa-44a0-95bb-6af79c02eac7" +
                                          "\r\n\r\nBody");
        }
    }
}
