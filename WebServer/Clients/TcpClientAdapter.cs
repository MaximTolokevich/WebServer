using System;
using System.Net;
using System.Net.Sockets;

namespace WebServer.Clients
{
    public class TcpClientAdapter : IClient
    {
        private readonly TcpClient _tcpClient;
        private readonly ServerOptions _options;
        private readonly int _bufferSize;
        private bool _disposedValue;

        public TcpClientAdapter(TcpClient client, ServerOptions options)
        {
            _tcpClient = client ?? throw new ArgumentNullException(nameof(client), "can't be null");
            _options = options ?? throw new ArgumentNullException(nameof(options), "can't be null");
            _bufferSize = _tcpClient.ReceiveBufferSize;
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

        public int GetBufferSize()
        {
            return _bufferSize;
        }

        public IPAddress GetClientInfo()
        {
            var info = _tcpClient.Client.RemoteEndPoint as IPEndPoint;
            return info?.Address;
        }


        public NetworkStream GetStream()
        {
            return _tcpClient.GetStream();
        }
    }
}
