using System;
using WebServer.Clients;
using WebServer.Listeners;

namespace MyDiTests.EntityMocks
{
    internal class AnotherListenerForTests : IListener
    {
        public IClient AcceptClient()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
