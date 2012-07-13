using System;
using System.Threading;
using Butterfly.Core;
using Butterfly.Net;
using System.Text;
using Database_Manager.Database.Session_Details.Interfaces;

namespace Butterfly.HabboHotel.Misc
{
    internal class LowPriorityWorker
    {
        private static int UserPeak;

        private static string FormatToBytes(int Num)
        {
            if (Num < 1024)
                return Num + "B/s";
            else if (Num < 1048576) //KB/s
            {
                double Traffic = Num / 1024;
                return String.Format("{0:0.##}", Traffic) + "KB/s";
            }
            else //MB/s
            {
                double Traffic = Num / 1048576;
                return String.Format("{0:0.##}", Traffic) + "MB/s";
            }
        }

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
                    TimeSpan Uptime = DateTime.Now - ButterflyEnvironment.ServerStarted;
                    string addOn = string.Empty;
                    if (System.Diagnostics.Debugger.IsAttached)
                        addOn = "[DEBUG] ";
                    mColdTitle = addOn + "Butterfly | Uptime: " + Uptime.Minutes + " minutes, " + Uptime.Hours + " hours and " + Uptime.Days + " day | " +
                        "Online users: " + ButterflyEnvironment.GetGame().GetClientManager().ClientCount + " | Loaded rooms: " + ButterflyEnvironment.GetGame().GetRoomManager().LoadedRoomsCount;

                    #region Garbage Collection
                    //GC.Collect();
                    //GC.WaitForPendingFinalizers();
                    #endregion

                    #region Statistics
                    int Status = 1;
                    int UsersOnline = ButterflyEnvironment.GetGame().GetClientManager().ClientCount;

                    if (UsersOnline > UserPeak)
                        UserPeak = UsersOnline;

                    int RoomsLoaded = ButterflyEnvironment.GetGame().GetRoomManager().LoadedRoomsCount;

                    using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        dbClient.runFastQuery("UPDATE server_status SET stamp = '" + ButterflyEnvironment.GetUnixTimestamp() + "', status = " + Status + ", users_online = " + UsersOnline + ", rooms_loaded = " + RoomsLoaded + ", server_ver = '" + ButterflyEnvironment.PrettyVersion + "', userpeak = " + UserPeak + "");
                    }
                    #endregion
                }
                catch (Exception e) { Logging.LogThreadException(e.ToString(), "Server status update task"); }
            }
        }
    }
}