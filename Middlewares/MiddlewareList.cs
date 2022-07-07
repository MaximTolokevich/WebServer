using System.Collections.Generic;
using WebServer.Middlewares.Interfaces;

namespace WebServer.Middlewares
{
    public class MiddlewareList : IMiddlewareCollection
    {
        public ICollection<IMiddleware> GetMiddlewares()
        {
            return new List<IMiddleware>();
        }
    }
}
