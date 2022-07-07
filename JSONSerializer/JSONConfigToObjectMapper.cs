﻿using System.IO;
using System.Text.Json;

namespace WebServer.JSONSerializer
{
    public class JSONConfigToObjectMapper
    {
        public static T MapConfig<T>(string path)
        {
            return JsonSerializer.Deserialize<T>(File.ReadAllText(path));
        }
    }
}
