using System;
using System.Collections;
using System.IO;
using System.Text;
using Butterfly.ServerManager;
using ConsoleWriter;

namespace Butterfly.Core
{
    public static class Logging
    {
        private static int tokenID = 0;

        internal static bool DisabledState
        {
            get
            {
                return Writer.DisabledState;
            }
            set
            {
                Writer.DisabledState = value;
            }
        }

        internal static void WriteLine(string Line)
        {
            Writer.WriteLine(Line);
        }

        internal static void LogException(string logText)
        {
            SessionManagement.BroadcastExceptionNotification(Butterfly.Core.ExceptionType.StandardException, tokenID++);
            Writer.LogException("TokenID: " + tokenID + Environment.NewLine + logText + Environment.NewLine);
        }

        internal static void LogCriticalException(string logText)
        {
            SessionManagement.BroadcastExceptionNotification(Butterfly.Core.ExceptionType.FatalException, tokenID++);
            Writer.LogCriticalException("TokenID: " + tokenID + logText);
        }

        internal static void LogCacheError(string logText)
        {
            SessionManagement.BroadcastExceptionNotification(Butterfly.Core.ExceptionType.StandardException, tokenID++);
            Writer.LogCacheError("TokenID: " + tokenID + logText);
        }

        internal static void LogMessage(string logText)
        {
            Writer.LogMessage(logText);
        }

        //internal static void LogDDOS(string logText)
        //{
        //    SessionManagement.BroadcastExceptionNotification(Butterfly.Core.ExceptionType.DDOSException, tokenID++);
        //    Writer.LogDDOS("TokenID: " + tokenID + logText);
        //}

        internal static void LogThreadException(string Exception, string Threadname)
        {
            SessionManagement.BroadcastExceptionNotification(Butterfly.Core.ExceptionType.ThreadedException, tokenID++);
            Writer.LogThreadException("TokenID: " + tokenID + Exception, Threadname);
        }

        public static void LogQueryError(Exception Exception, string query)
        {
            SessionManagement.BroadcastExceptionNotification(Butterfly.Core.ExceptionType.SQLException, tokenID++);
            Writer.LogQueryError(Exception, "TokenID: " + tokenID + query);
        }

        internal static void LogPacketException(string Packet, string Exception)
        {
            SessionManagement.BroadcastExceptionNotification(Butterfly.Core.ExceptionType.UserException, tokenID++);
            Writer.LogPacketException(Packet, "TokenID: " + tokenID + Exception);
            SessionManagement.IncreaseError();
        }

        internal static void HandleException(Exception pException, string pLocation)
        {
            Writer.HandleException(pException, pLocation);
        }

        internal static void DisablePrimaryWriting(bool ClearConsole)
        {
            Writer.DisablePrimaryWriting(ClearConsole);
        }

        internal static void LogShutdown(StringBuilder builder)
        {
            Writer.LogShutdown(builder);
        }
    }
}
