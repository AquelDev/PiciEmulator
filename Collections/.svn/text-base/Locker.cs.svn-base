using System.Threading;

namespace Butterfly.Collections
{
    public class Locker
    {
        internal int handle;

        public Locker()
        {
            handle = 0;
        }

        internal bool TryEnterLock()
        {
            if (handle == 1)
                return false;

            return (Interlocked.Exchange(ref handle, 1) == 0); // 0 = open
        }

        internal bool ExitLock()
        {
            return (Interlocked.Exchange(ref handle, 0) == 1); // 1 = closed
        }
    }
}
