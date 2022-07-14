using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Clients;
using WebServer.DI.Interfaces;
using WebServer.Middlewares;
using WebServer.Middlewares.Interfaces;

namespace WebServer.DI
{
    public class MiddlewareBuilder:IMiddlewareBuilder
    {
        public ICollection<IMiddleware> BuildMiddleware(IDIContainer container, ServerOptions options)
        {
            var list = new MiddlewareList().GetMiddlewares();
            list.Add(container.GetService<IMiddleware>(options.DependencyGroupName));
            return list;
        }
    }
}
