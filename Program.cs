using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Pici.Core;
using System.Net;
using System.Collections.Specialized;
using System.Net.Cache;
using Pici.Core.Loggings;
using System.ComponentModel;
using System.Text;

namespace Pici
{
    internal class Program
    {
        [DllImport("Kernel32")]

        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);
        private delegate bool EventHandler(CtrlType sig);

        enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        [STAThreadAttribute]
        internal static void Main()
        {
            Writer.Init();

            //string lcu = PiciEnvironment.GetConfig().data["Pici-Studios.username"];
            //string lcp = PiciEnvironment.GetConfig().data["Pici-Studios.password"];
            Program.InitEnvironment();
        }

        [MTAThread]
        internal static void InitEnvironment()
        {
            if (!PiciEnvironment.isLive)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.CursorVisible = false;
                AppDomain currentDomain = AppDomain.CurrentDomain;
                currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);

                PiciEnvironment.Initialize();
            }
        }

        static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Logging.DisablePrimaryWriting(true);
            Exception e = (Exception)args.ExceptionObject;
            Logging.LogCriticalException("SYSTEM CRITICAL EXCEPTION: " + e.ToString());
            PiciEnvironment.SendMassMessage("A fatal error crashed the server, server shutting down.");
            PiciEnvironment.PreformShutDown(true);
        }
    }
}
