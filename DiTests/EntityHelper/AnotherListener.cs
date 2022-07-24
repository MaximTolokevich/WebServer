using System;
using Server.Clients;
using Server.Listeners;

namespace DiTests.EntityHelper
{
    internal class AnotherListener : IListener
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
