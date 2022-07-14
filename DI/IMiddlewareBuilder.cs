using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Clients;
using WebServer.DI.Interfaces;
using WebServer.Middlewares.Interfaces;

namespace WebServer.DI
{
    public interface IMiddlewareBuilder
    {
        ICollection<IMiddleware> BuildMiddleware(IDIContainer container, ServerOptions options);
    }
}
