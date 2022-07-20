using System.IO;
using System.Linq;
using System.Text;
using WebServer.Middlewares.Interfaces;
using WebServer.Models;
using WebServer.Service.Interfaces;
using WebServer.Storage;

namespace WebServer.Middlewares
{
    public class CookieMiddleware : IMiddleware
    {
        private readonly ICookieGenerator _cookieGenerator;
        private readonly CookieStorage _cookieStorage;

        private readonly string _template =
            File.ReadAllText("C:\\Users\\Trolo\\source\\repos\\WebServer\\ResponseBody.html");

        public CookieMiddleware(ICookieGenerator cookieGenerator, CookieStorage storage)
        {
            _cookieGenerator = cookieGenerator;
            _cookieStorage = storage;
        }
        public MyHttpContext Invoke(MyHttpContext context)
        {
            var cookies = Extensions.CookieExtensions.GetCookies(context.HttpRequest);
            var myCookieOrDefault =
                _cookieStorage.CookieDictionary.Keys.FirstOrDefault(x => cookies.Any(y => y.Equals(x)));
            if (myCookieOrDefault is not null)
            {
                context.HttpResponse.Body = Encoding.UTF8.GetBytes(string.Format(_template,
                    ++_cookieStorage.CookieDictionary[myCookieOrDefault]));
            }
            else
            {
                context.HttpResponse.Body = Encoding.UTF8.GetBytes(string.Format(_template, string.Empty));
                const long visit = 1;
                context.HttpResponse.CookieList.Add($"{_cookieGenerator.GenerateCookie()}");
                foreach (var item in context.HttpResponse.CookieList)
                {
                    _cookieStorage.CookieDictionary.TryAdd(item, visit);
                }
            }

            context.HttpResponse.ContentLength = context.HttpResponse.Body.Length;
            context.HttpResponse.ContentType = "text/html; charset=UTF-8";
            return context;
        }
    }
}
