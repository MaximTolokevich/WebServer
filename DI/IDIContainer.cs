using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.DI
{
    public interface IDIContainer
    {
        T GetService<T>();
        T GetService<T>(string name);
    }
}
