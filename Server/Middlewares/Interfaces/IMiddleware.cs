using Server.Models;

namespace Server.Middlewares.Interfaces
{
    public interface IMiddleware
    {
        MyHttpContext Invoke(MyHttpContext context);
    }
}
