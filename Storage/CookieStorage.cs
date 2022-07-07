using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.Storage
{
    public class CookieStorage
    {
        public readonly ConcurrentDictionary<string,long> CookieDictionary= new ();
    }
}
