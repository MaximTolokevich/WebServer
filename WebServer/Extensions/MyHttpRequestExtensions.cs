using System;
using System.Collections.Generic;
using WebServer.Models;

namespace WebServer.Extensions
{
    public static class MyHttpRequestExtensions
    {
        public static IEnumerable<string> GetCookies(this MyHttpRequest httpRequest)
        {
            return httpRequest.Headers.ContainsKey("Cookie")
                 ? httpRequest.Headers["Cookie"].Split("; ")
                 : Array.Empty<string>();
        }
    }
}
