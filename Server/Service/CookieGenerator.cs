using System;
using Server.Service.Interfaces;

namespace Server.Service
{
    public class CookieGenerator : ICookieGenerator
    {
        public string GenerateCookie() 
        {
            return  $"{Guid.NewGuid()}={Guid.NewGuid()}";
        }
    }
}
