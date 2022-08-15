using System;

namespace MyDi.DI.Exceptions
{
    public class DependencyNotRegisteredException : Exception
    {
        public DependencyNotRegisteredException(string message)
            : base(message)
        {
        }
    }
}
