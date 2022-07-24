using System.Collections.Generic;
using MyDi.DI.Interfaces;
using Server.Middlewares.Interfaces;

namespace Server.Middlewares
{
    public static class MiddlewareBuilder
    {
        public static ICollection<IMiddleware> Build(IServiceCollection collection, IDIContainer container, string name)
        {
            var middlewareCollection = new List<IMiddleware>();
            collection.AddTransient<IMiddleware, CookieMiddleware>(name);
            middlewareCollection.Add(container.GetService<CookieMiddleware>(name));
            return middlewareCollection;
        }
    }
}
