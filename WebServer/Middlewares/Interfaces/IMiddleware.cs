using WebServer.Models;

namespace WebServer.Middlewares.Interfaces
{
    public interface IMiddleware
    {
        MyHttpContext Invoke(MyHttpContext context);
    }
}
