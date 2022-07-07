using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Service;

namespace WebServer.Middlewares
{
    public interface IMiddleware
    {
        MyHttpContext Invoke(MyHttpContext context);
    }
}
