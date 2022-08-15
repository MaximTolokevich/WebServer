using Newtonsoft.Json;
using System.IO;


namespace MyDi.DI.ConfigMapper
{
    public class JSONConfigToObjectMapper
    {
        public static T MapConfig<T>(string path)
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
        }
    }
}
