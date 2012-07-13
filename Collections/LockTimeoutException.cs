using System;

namespace Pici.Collections
{
    [Serializable]
    class LockTimeoutException : Exception
    {
        public LockTimeoutException(string exception)
            : base(exception)
        {
            //Mini weini
        }
    }
}
