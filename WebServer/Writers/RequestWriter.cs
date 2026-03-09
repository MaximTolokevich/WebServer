using WebServer.Clients;

namespace WebServer.Writers
{
    public class RequestWriter : IClientWriter
    {
        public void Write(IClient client, byte[] data)
        {
            var stream = client.GetStream();
            stream.Write(data);
        }
    }
}
