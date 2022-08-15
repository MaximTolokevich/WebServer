using WebServer.Clients;

namespace WebServer.Models
{
     public class MyHttpContext
    {
        public MyHttpRequest HttpRequest { get; set; }
        public MyHttpResponse HttpResponse { get; set; }
        public MyHttpContext(ServerOptions options)
        {
            HttpResponse = new MyHttpResponse(options);
            HttpRequest = new MyHttpRequest();
        }
    }
}