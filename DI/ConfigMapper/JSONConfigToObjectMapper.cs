using System.IO;
using Newtonsoft.Json;

namespace DI.ConfigMapper
{
    public class JSONConfigToObjectMapper
    {
        public static T MapConfig<T>(string path)
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
        }
    }
}
