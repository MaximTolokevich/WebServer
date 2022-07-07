using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Models;

namespace WebServer.Extensions
{
    public static class CookieExtensions
    {
        public static IEnumerable<string> GetCookies(MyHttpRequest httpRequest)
        {
           return httpRequest.Headers.ContainsKey("Cookie")
                ? httpRequest.Headers["Cookie"].Split(';')
                : Array.Empty<string>();
        }
    }
}
