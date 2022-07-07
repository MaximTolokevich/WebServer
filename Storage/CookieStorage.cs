using System.Collections.Concurrent;

namespace WebServer.Storage
{
    public class CookieStorage
    {
        public readonly ConcurrentDictionary<string, long> CookieDictionary = new();
    }
}
