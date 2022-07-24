using System;

namespace DI.Exceptions
{
    public class DependencyNotRegisteredException : Exception
    {
        public DependencyNotRegisteredException(string message)
            : base(message)
        {
        }
    }
}
