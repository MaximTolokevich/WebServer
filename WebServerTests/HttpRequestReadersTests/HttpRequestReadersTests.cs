using FluentAssertions;
using Moq;
using System.Text;
using WebServer.Clients;
using WebServer.HttpRequestReaders;
using Xunit;

namespace WebServerTests.HttpRequestReadersTests
{
    public class HttpRequestReadersTests
    {
        [Fact]
        public void Read_Should_Read_Data_From_Client()
        {
            // Arrange
            var optionsMock = new Mock<ServerOptions>();
            var requestReaderMock = new Mock<HttpRequestReader>(optionsMock.Object);
            var clientMock = new Mock<IClient>();
            const string httpRequestAsString = "GET / HTTP/1.1" +
                                               "\r\nHost: localhost:8080" +
                                               "\r\nConnection: keep-alive" +
                                               "\r\nContent-Type: Content-Type: text/html; charset=UTF-8" +
                                               "\r\nContent-Length: 19" +
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
                                               "\r\n\r\nsome little message";
            var httpRequestAsByteArray = Encoding.UTF8.GetBytes(httpRequestAsString);
            clientMock.Setup(x => x.ReadRequest()).Returns(httpRequestAsByteArray);

            // Act
            var model = requestReaderMock.Object.Read(clientMock.Object);

            // Assert
            model.Body.Length.Should().Be(19);
        }
        [Fact]
        public void Read_Should_Read_Big_Data_From_Client()
        {
            // Arrange
            var optionsMock = new Mock<ServerOptions>();
            var requestReaderMock = new HttpRequestReader(optionsMock.Object);
            var clientMock = new Mock<IClient>();
            var httpRequestAsString = "GET / HTTP/1.1" +
                                               "\r\nHost: localhost:8080" +
                                               "\r\nConnection: keep-alive" +
                                               "\r\nContent-Type: Content-Type: text/html; charset=UTF-8" +
                                               "\r\nContent-Length: 22222222" +
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
                                               $"\r\n\r\n{new string('k', 11111111)}";

            var httpRequestAsByteArray = Encoding.UTF8.GetBytes(httpRequestAsString);

            var bodySecondPart = Encoding.UTF8.GetBytes(new string('k', 11111111));

            clientMock.SetupSequence(x => x.ReadRequest())
                                        .Returns(httpRequestAsByteArray)
                                        .Returns(bodySecondPart);
            // Act
            var model = requestReaderMock.Read(clientMock.Object);

            // Assert
            model.Body.Length.Should().Be(22222222);
        }
    }
}
