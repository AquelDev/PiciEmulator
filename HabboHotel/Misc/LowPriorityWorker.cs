using System;
using System.Threading;
using Pici.Core;
using Pici.Net;
using System.Text;
using Pici.Storage.Database.Session_Details.Interfaces;

namespace Pici.HabboHotel.Misc
{
    internal class LowPriorityWorker
    {
        private static int UserPeak;

        private static DateTime consoleLastExecution;
        private static string mColdTitle;
        internal static void ConsoleTitleWorker()
        {
            TimeSpan sinceLastTime = DateTime.Now - consoleLastExecution;

            if (sinceLastTime.TotalMilliseconds >= 1000)
            {
                consoleLastExecution = DateTime.Now;
                try
                {
                    //int TotalBytesSent = TcpConnection.GetNumberOfSentBytes();
                    //int TotalBytesReceived = TcpConnection.GetNumberOfReceivedBytes();

                    Console.Title = mColdTitle;//+ "In: " + FormatToBytes(TotalBytesReceived) + " Out: " + FormatToBytes(TotalBytesSent);
                }
                catch (Exception Exception)
                {
                    Logging.LogThreadException(Exception.ToString(), "ConsoleTitleWorker");
                }

            }
        }

        internal static void Init(IQueryAdapter dbClient)
        {
            dbClient.setQuery("SELECT userpeak FROM server_status");
            UserPeak = dbClient.getInteger();
            mColdTitle = string.Empty;
        }

        private static DateTime processLastExecution;
        internal static void Process()
        {
            TimeSpan sinceLastTime = DateTime.Now - processLastExecution;

            if (sinceLastTime.TotalMilliseconds >= 30000)
            {
                processLastExecution = DateTime.Now;
                try
                {
                    TimeSpan Uptime = DateTime.Now - PiciEnvironment.ServerStarted;
                    mColdTitle = PiciEnvironment.Title + " "+PiciEnvironment.Version+" | Uptime: " + Uptime.Minutes + " minutes, " + Uptime.Hours + " hours and " + Uptime.Days + " day | " +
                        "Online users: " + PiciEnvironment.GetGame().GetClientManager().ClientCount + " | Loaded rooms: " + PiciEnvironment.GetGame().GetRoomManager().LoadedRoomsCount;

                    #region Garbage Collection
                    //GC.Collect();
                    //GC.WaitForPendingFinalizers();
                    #endregion

                    #region Statistics
                    int Status = 1;
                    int UsersOnline = PiciEnvironment.GetGame().GetClientManager().ClientCount;

                    if (UsersOnline > UserPeak)
                        UserPeak = UsersOnline;

                    int RoomsLoaded = PiciEnvironment.GetGame().GetRoomManager().LoadedRoomsCount;

                    using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        dbClient.runFastQuery("UPDATE server_status SET stamp = '" + PiciEnvironment.GetUnixTimestamp() + "', status = " + Status + ", users_online = " + UsersOnline + ", rooms_loaded = " + RoomsLoaded + ", server_ver = '" + PiciEnvironment.Build + "', userpeak = " + UserPeak + "");
                    }
                    #endregion
                }
                catch (Exception e) { Logging.LogThreadException(e.ToString(), "Server status update task"); }
            }
        }
    }
}