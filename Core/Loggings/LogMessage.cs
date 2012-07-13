
namespace Pici.Core.Loggings
{
    class LogMessage
    {
        internal string message;
        internal string location;

        public LogMessage(string message, string location)
        {
            this.message = message;
            this.location = location;
        }

        internal void Dispose()
        {
            message = null;
            location = null;
        }
    }
}
