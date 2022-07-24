using System.Collections.Concurrent;

namespace Server.Storage
{
    public class CookieStorage
    {
        public readonly ConcurrentDictionary<string, long> CookieDictionary = new();
    }
}
