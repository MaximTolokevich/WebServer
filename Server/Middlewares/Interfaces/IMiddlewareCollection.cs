using System.Collections.Generic;

namespace Server.Middlewares.Interfaces
{
    public interface IMiddlewareCollection
    {
        ICollection<IMiddleware> GetMiddlewares();
    }
}
