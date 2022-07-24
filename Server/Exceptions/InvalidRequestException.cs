using System;

namespace Server.Exceptions
{
    public class InvalidRequestException : Exception
    {
        public InvalidRequestException(string message)
            : base(message)
        {
        }
    }
}
