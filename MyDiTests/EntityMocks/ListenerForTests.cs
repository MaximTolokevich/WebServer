using System;
using WebServer.Clients;
using WebServer.Listeners;

namespace MyDiTests.EntityMocks
{
    internal class ListenerForTests : IListener
    {
        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public IClient AcceptClient()
        {
            throw new NotImplementedException();
        }
    }
}
