using Newtonsoft.Json;

namespace WebServer.Clients
{

    public class ServerOptions
    {
        [JsonProperty("ServerName")]
        public string ServerName { get; set; }

        [JsonProperty("ReadTimeOut")]
        public int ReadTimeOut { get; set; }

        [JsonProperty("Port")]
        public int Port { get; set; }

        [JsonProperty("IpAddress")]
        public string IpAddress { get; set; }

        [JsonProperty("DependencyGroupName")]
        public string DependencyGroupName { get; set; }

        [JsonProperty("WorkerThreadsMax")]
        public int WorkerThreadsMax { get; set; }

        [JsonProperty("CompletionPortThreadsMax")]
        public int CompletionPortThreadsMax { get; set; }

        [JsonProperty("WorkerThreadsMin")]
        public int WorkerThreadsMin { get; set; }

        [JsonProperty("CompletionPortThreadsMin")]
        public int CompletionPortThreadsMin { get; set; }
    }
}

