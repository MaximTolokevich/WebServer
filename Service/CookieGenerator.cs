using System;
using WebServer.Service.Interfaces;

namespace WebServer.Service
{
    public class CookieGenerator : ICookieGenerator
    {
        public string GenerateCookie() 
        {
            return  Guid.NewGuid().ToString();
        }
    }
}
