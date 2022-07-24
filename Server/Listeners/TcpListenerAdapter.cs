using System;
using System.Net;
using System.Net.Sockets;
using Server.Clients;

namespace Server.Listeners
{
    public class TcpListenerAdapter : IListener
    {
        private readonly TcpListener _tcpListener;
        private readonly ServerOptions _options;
        public TcpListenerAdapter(ServerOptions options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options), "can't be null");
            }

            if (!IPAddress.TryParse(options.IpAddress, out var address))
            {
                throw new ArgumentException("is not valid", nameof(options.IpAddress));
            }

            if (options.Port is > IPEndPoint.MaxPort or < IPEndPoint.MinPort)
            {
                throw new ArgumentOutOfRangeException(nameof(options.Port),
                    $"can't be less then {IPEndPoint.MinPort} and more then {IPEndPoint.MaxPort}");
            }

            _options = options;
            _tcpListener = new TcpListener(address, options.Port);
        }

        public IClient AcceptClient()
        {
            if (_tcpListener.Pending())
            {
                try
                {
                    return new TcpClientAdapter(_tcpListener.AcceptTcpClient(), _options);
                }
                catch (SocketException e)
                {
                    Console.WriteLine(e +
                                      $" Error code {e.ErrorCode}, check https://docs.microsoft.com/en-us/windows/win32/winsock/windows-sockets-error-codes-2");
                    throw;
                }
            }

            return null;
        }

        public void Start()
        {
            _tcpListener.Start();
        }

        public void Stop()
        {
            _tcpListener.Stop();
        }
    }
}
