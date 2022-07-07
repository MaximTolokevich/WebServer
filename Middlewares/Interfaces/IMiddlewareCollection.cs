using System.Collections.Generic;

namespace WebServer.Middlewares.Interfaces
{
    public interface IMiddlewareCollection
    {
        ICollection<IMiddleware> GetMiddlewares();
    }
}
