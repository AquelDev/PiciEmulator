using System;

namespace Pici.Core.ConnectionManager.Socket_Exceptions
{
    class SocketInitializationException : Exception
    {
        public SocketInitializationException(string message)
            : base(message)
        {

        }
    }
}
