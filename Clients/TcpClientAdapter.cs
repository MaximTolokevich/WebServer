using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace WebServer.Clients
{
    public class TcpClientAdapter : IClient
    {
        private readonly TcpClient _tcpClient;
        private readonly ServerOptions _options;
        private bool _disposedValue;
        public TcpClientAdapter(TcpClient client, ServerOptions options)
        {
            _tcpClient = client ?? throw new ArgumentNullException(nameof(client), "can't be null");
            _options = options ?? throw new ArgumentNullException(nameof(options), "can't be null");
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            switch (_disposedValue)
            {
                case true:
                    return;
                case false:
                    _disposedValue = true;
                    _tcpClient.Close();
                    break;
            }
        }

        public IPAddress GetClientInfo()
        {
            var info = _tcpClient.Client.RemoteEndPoint as IPEndPoint;
            return info?.Address;
        }
        public byte[] ReadRequest()
        {
            var stream = _tcpClient.GetStream();
            var buffer = new byte[8192];
            stream.ReadTimeout = _options.ReadTimeOut;
            using var memoryStream = new MemoryStream();
            do
            {
                var dataLength = stream.Read(buffer, 0, buffer.Length);
                memoryStream.Write(buffer, 0, dataLength);
                Array.Clear(buffer, 0, buffer.Length);
            } while (stream.DataAvailable);


            return memoryStream.ToArray();
        }

        public void SendResponse(byte[] data)
        {
            var stream = _tcpClient.GetStream();
            stream.Write(data);
        }
    }
}
