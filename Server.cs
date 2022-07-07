using System.Threading;
using WebServer.Clients;
using WebServer.Listeners;
using WebServer.Service;

namespace WebServer
{
    public class Server:IServer
    {
        private readonly IListener _listener;
        private readonly IManager _manager;
        private bool _isRunning = true;
        public Server(IListener listener, IManager manager)
        {
            _listener = listener;
            _manager = manager;
        }

        public void Start()
        {
            _listener.Start();

            var thread = new Thread(WaitClients);

            thread.Start();
        }

        public void Stop() => _isRunning = false;

        private void WaitClients()
        {
            IClient client;

            SpinWait.SpinUntil(() =>
            {
                client = _listener.AcceptClient();
                if (client is not null)
                {
                    using (client)
                    {
                        _manager.Manage(client);
                    }
                    //ThreadPool.QueueUserWorkItem(HandleClient, client);
                }

                return !_isRunning;
            });

            _listener.Stop();
        }

        private void HandleClient(object obj)
        {
            _manager.Manage(obj as IClient);
        }
    }
}
