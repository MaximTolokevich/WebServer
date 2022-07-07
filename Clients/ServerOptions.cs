namespace WebServer.Clients
{
    public class ServerOptions
    {
        public string ServerName { get; set; }
        public int ReadTimeOut { get; set; }
        public int Port { get; set; }
        public string IpAddress { get; set; }
        public string DependencyGroupName { get; set; }
        public (int,int) SetMaxThreads { get; set; }
        public (int,int) SetMinThreads { get; set; }

        public ServerOptions()
        {

        }
    }
}
