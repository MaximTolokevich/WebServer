using System.Threading;
using WebServer.Clients;
using WebServer.DI.Interfaces;
using WebServer.Listeners;
using WebServer.Service.Interfaces;

namespace WebServer
{
    public class Server : IServer
    {
        private readonly IListener _listener;
        private readonly IDIContainer _container;
        private readonly ServerOptions _options;
        private bool _isRunning = true;
        public Server(IListener listener, IDIContainer container, ServerOptions options)
        {
            _listener = listener;
            _container = container;
            _options = options;
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
                    if (_options is not null)
                    {
                        ThreadPool.SetMinThreads(_options.SetMinThreads.Item1, _options.SetMinThreads.Item2);
                        ThreadPool.SetMaxThreads(_options.SetMaxThreads.Item1, _options.SetMaxThreads.Item2);
                    }
                    ThreadPool.QueueUserWorkItem(HandleClient, client);
                }

                return !_isRunning;
            });

            _listener.Stop();
        }

        private void HandleClient(object obj)
        {
            using var client = obj as IClient;

            if (_options.DependencyGroupName is null)
            {
                _container.GetService<IManager>(null).Manage(client);
            }
            else
            {
                _container.GetService<IManager>(_options.DependencyGroupName).Manage(client);
            }
        }
    }
}
