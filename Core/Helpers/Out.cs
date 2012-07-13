using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using Pici.Core.Loggings;

namespace Pici.Core.Helpers
{
    /// <summary>
    /// Provides interface output related functions, such as logging activities.
    /// </summary>
    public static class Out
    {
        private class PrintItem {
            public delegate void LoggerMethod(PrintItem item);
            private LoggerMethod loggerMethod;
            public string message;
            public string methodName;
            public string dateTimenow;
            public string milliSecond;
            public string className;
            public ConsoleColor colorOne;
            public ConsoleColor colorTwo;
            public string headerText;
            public PrintItem(string message, string methodName, string className, string dateTimenow, string milliSecond, LoggerMethod method)
            {
                this.message = message;
                this.methodName = methodName;
                this.dateTimenow = dateTimenow;
                this.loggerMethod = method;
                this.milliSecond = milliSecond;
                this.className = className;
            }
            public PrintItem(string message, string methodName, string className, string dateTimenow, string milliSecond, LoggerMethod method, ConsoleColor colorOne, ConsoleColor colorTwo)
                : this(message, methodName, className, dateTimenow, milliSecond, method)
            {
                this.colorOne = colorOne;
                this.colorTwo = colorTwo;
            }

            public PrintItem(string message, string methodName, string className, string dateTimenow, string milliSecond, LoggerMethod method, ConsoleColor colorOne, ConsoleColor colorTwo, string headerText)
                : this(message, methodName, className, dateTimenow, milliSecond, method, colorOne, colorTwo)
            {
                this.headerText = headerText;
            }

            public void doWrite()
            {
                loggerMethod.Invoke(this);
            }
        }
        private static Queue queItem = new Queue();

        /// <summary>
        /// Enum with flags for log importancies. If 'minimumImportance' flag is higher than the action to be logged, then the action won't be logged.
        /// </summary>
        public enum logFlags { ImportantLogLevel = 3, StandardLogLevel = 2, BelowStandardlogLevel = 1, lowLogLevel = 0 }
        /// <summary>
        /// Flag for minimum importance in logs. Adjust this to don't print less important logs.
        /// </summary>
        public static logFlags minimumImportance = logFlags.lowLogLevel;

        private static bool loggin = false;

        public static void stopLogger() { loggin = false; }
        public static void startLogger()
        { 
            //new Thread(new ThreadStart(delegate(){

            //PrintItem currentHandeling;
            //loggin = false;
            //while (loggin)
            //{
            //    if (queItem.Count > 0)
            //    {
            //        lock (queItem.SyncRoot)
            //        {
            //            while (queItem.Count > 0)
            //            {
            //                currentHandeling = (PrintItem)queItem.Dequeue();
            //                currentHandeling.doWrite();
            //            }
            //        }
            //    }
            //    else
            //        Thread.Sleep(5);
            //}
            
            //})).Start();
            
        }

        /// <summary>
        /// Enqueues an item
        /// </summary>
        /// <param name="item"></param>
        private static void enQueueItem(PrintItem item)
        {
            if (loggin)
            {
                lock (queItem.SyncRoot)
                {
                    queItem.Enqueue(item);
                }
            }
        }

        

        /// <summary>
        /// Prints a green line of log, together with timestamp and method name.
        /// </summary>
        /// <param name="logText">The log line to be printed.</param>
        public static void writeNotification(string logText)
        {
            Writer.WriteLine(logText);
            return;

            if (minimumImportance > logFlags.lowLogLevel)
                return;

            DateTime _DTN = DateTime.Now;
            StackFrame _SF = new StackTrace().GetFrame(1);

            enQueueItem( new PrintItem(logText, _SF.GetMethod().Name, _SF.GetMethod().ReflectedType.Name, _DTN.ToLongTimeString(), _DTN.Millisecond.ToString(), writeNormal));

        }
        /// <summary>
        /// Prints a green line of log, together with timestamp and method name.
        /// </summary>
        /// <param name="logText">The log line to be printed.</param>
        /// <param name="logFlag">The importance flag of this log line.</param>
        public static void writeLine(string logText, logFlags flag)
        {
            Writer.WriteLine(logText);
            return;

            if (flag < minimumImportance)
                return;
            DateTime _DTN = DateTime.Now;
            StackFrame _SF = new StackTrace().GetFrame(1);
            enQueueItem(new PrintItem(logText, _SF.GetMethod().Name, _SF.GetMethod().ReflectedType.Name, _DTN.ToLongTimeString(), _DTN.Millisecond.ToString(), writeNormal));
        }

        /// <summary>
        /// Prints a customizeable line of log, together with timestamp and method name.
        /// </summary>
        /// <param name="logText">The log line to be printed.</param>
        /// <param name="logFlag">The importance flag of this log line.</param>
        /// <param name="colorOne">The color to use on the left.</param>
        /// <param name="colorTwo">The color to use on the right.</param>
        public static void writeLine(string logText, logFlags logFlag, ConsoleColor colorOne, ConsoleColor colorTwo)
        {

            Writer.WriteLine(logText);
            return;

            if (logFlag < minimumImportance)
                return;
            DateTime _DTN = DateTime.Now;
            StackFrame _SF = new StackTrace().GetFrame(1);
            enQueueItem(new PrintItem(logText, _SF.GetMethod().Name, _SF.GetMethod().ReflectedType.Name, _DTN.ToLongTimeString(), _DTN.Millisecond.ToString(), writeSpecialColorLine, colorOne, colorTwo));
        }

       

        /// <summary>
        /// Prints a red, error line of log, together with timestamp and method name.
        /// </summary>
        /// <param name="logText">The log line to be printed.</param>
        public static void writeError(string logText)
        {
            Writer.WriteLine(logText);
            return;

            DateTime _DTN = DateTime.Now;
            StackFrame _SF = new StackTrace().GetFrame(1);

            enQueueItem(new PrintItem(logText, _SF.GetMethod().Name, _SF.GetMethod().ReflectedType.Name, _DTN.ToLongTimeString(), _DTN.Millisecond.ToString(), writeSpecialColorLine, ConsoleColor.Red, ConsoleColor.DarkRed));
        }

        /// <summary>
        /// Writes a plain text line.
        /// </summary>
        /// <param name="logText">The log line to be printed.</param>
        public static void writePlain(string logText, logFlags flag)
        {
            Writer.WriteLine(logText);
            return;

            if (flag < minimumImportance)
                return;

            enQueueItem(new PrintItem(logText.Replace(Convert.ToChar(13).ToString(), "{13}"), null, null, null, null, writePlain));

        }
        /// <summary>
        /// Writes a blank line.
        /// </summary>
        public static void writeBlank()
        {
            enQueueItem(new PrintItem(null, null, null, null, null, printEmpty));
        }


        /// <summary>
        /// Writes a special line of log, with customizeable colors and header coloring of logText.
        /// </summary>
        /// <param name="logText">The log line to be printed.</param>
        /// <param name="flag">The importance flag of this log line.</param>
        /// <param name="colorOne">The color to use on the left.</param>
        /// <param name="colorTwo">The color to use on the right.</param>
        /// <param name="headerHead">The string to use infront of logText.</param>
        /// <param name="headerLength">The length of the header to color.</param>
        /// <param name="headerColor">The color for the header in the logText.</param>
        public static void writeSpecialLine(string logText, logFlags flag, ConsoleColor headerColor, ConsoleColor colorTwo, string headerHead)
        {
            Writer.WriteLine(logText);
            return;

            if (flag < minimumImportance)
                return;
            DateTime _DTN = DateTime.Now;
            StackFrame _SF = new StackTrace().GetFrame(1);
            enQueueItem(new PrintItem(logText, _SF.GetMethod().Name, _SF.GetMethod().ReflectedType.Name, _DTN.ToLongTimeString(), _DTN.Millisecond.ToString(), writeSpecialLineWithHeaderHead, headerColor, colorTwo, headerHead));

        }
        


        /// <summary>
        /// Writes a red line of text
        /// </summary>
        /// <param name="logText">The log line to be printed.</param>
        /// <param name="flag">The importance flag of this log line.</param>
        public static void writeRedText(string logText, logFlags flag)
        {
            if (flag < minimumImportance)
                return;


            DateTime _DTN = DateTime.Now;
            StackFrame _SF = new StackTrace().GetFrame(1);

            enQueueItem(new PrintItem(logText, _SF.GetMethod().Name, _SF.GetMethod().ReflectedType.Name, _DTN.ToLongTimeString(), _DTN.Millisecond.ToString(), writeRed));
        }

        /// <summary>
        /// Writes a serious error to the  "dc.err" file
        /// </summary>
        /// <param name="logText">The text that should be written in the file</param>
        public static void writeDCError(string logText)
        {
            enQueueItem(new PrintItem(logText, null, null, null, null, fileWriter, ConsoleColor.Black, ConsoleColor.Black, "dc.err"));
        }

        /// <summary>
        /// Writes a serious error to the  "dc.err" file
        /// </summary>
        /// <param name="logText">The text that should be written in the file</param>
        public static void writeAreaLoopError(string logText)
        {
            enQueueItem(new PrintItem(logText, null, null, null, null, fileWriter, ConsoleColor.Black, ConsoleColor.Black, "areaLoop.err"));
        }


        private static void fileWriter(PrintItem item)
        {
            writeToFile(item.headerText, item.message);
        }
        /// <summary>
        /// Writes a serious error to the  "serious.err" file
        /// </summary>
        /// <param name="logText">The text that should be written in the file</param>
        public static void writeSeriousError(string logText)
        {
            enQueueItem(new PrintItem(logText, null, null, null, null, fileWriter, ConsoleColor.Black, ConsoleColor.Black, "serious.err"));
            DateTime _DTN = DateTime.Now;
            StackFrame _SF = new StackTrace().GetFrame(1);

            enQueueItem(new PrintItem("A serious error occured in the brainwave platform. It has been written to the file serious.err", _SF.GetMethod().Name, _SF.GetMethod().ReflectedType.Name, _DTN.ToLongTimeString(), _DTN.Millisecond.ToString(), writeSpecialColorLine, ConsoleColor.Red, ConsoleColor.DarkRed));
            
        }

        public static void writeSqlError(Exception ex, string command)
        {
            DateTime _DTN = DateTime.Now;
            StackFrame _SF = new StackTrace().GetFrame(1);
            enQueueItem(new PrintItem(ex.Message + "\r\n\r\n" + command, null, null, null, null, fileWriter, ConsoleColor.Black, ConsoleColor.Black, "sql.err"));
            enQueueItem(new PrintItem(ex.Message + "\r\n\r\n" + command, _SF.GetMethod().Name, _SF.GetMethod().ReflectedType.Name, _DTN.ToLongTimeString(), _DTN.Millisecond.ToString(), writeSpecialColorLine, ConsoleColor.Red, ConsoleColor.DarkRed));
        }

        /// <summary>
        /// Writes stuff to a file
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <param name="text">the information that needs to be appended to the file</param>
        private static void writeToFile(string fileName, string text)
        {
            try
            {
                System.IO.FileStream Writer = new System.IO.FileStream(fileName, System.IO.FileMode.Append, System.IO.FileAccess.Write);
                while (!Writer.CanWrite)
                    Thread.Sleep(1);
                byte[] Msg = System.Text.ASCIIEncoding.ASCII.GetBytes(text + "\r\n\r\n");
                Writer.Write(Msg, 0, Msg.Length);
                Writer.Close();
            }
            catch
            { }

        }

        public static void writeError(string logText, logFlags flag)
        {
            if (flag < minimumImportance)
                return;
            DateTime _DTN = DateTime.Now;
            StackFrame _SF = new StackTrace().GetFrame(1);
            enQueueItem(new PrintItem(logText, _SF.GetMethod().Name, _SF.GetMethod().ReflectedType.Name, _DTN.ToLongTimeString(), _DTN.Millisecond.ToString(), writeSpecialColorLine, ConsoleColor.Red, ConsoleColor.DarkRed));

        }

        private static void writeSpecialColorLine(PrintItem item)
        {
            Console.Write("[" + item.dateTimenow + ":" + item.milliSecond + "] [");
            Console.ForegroundColor = item.colorOne;
            Console.Write(item.className + "." + item.methodName);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("] » ");
            Console.ForegroundColor = item.colorTwo;
            Console.WriteLine(item.message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private static void writePlain(PrintItem item)
        {
            Console.WriteLine(item.message);
        }

        private static void printEmpty(PrintItem item)
        {
            Console.WriteLine();
        }

        private static void writeDefaultHeader(PrintItem item)
        {
            Console.Write("[" + item.dateTimenow + ":" + item.milliSecond + "] [");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(item.className + "." + item.methodName);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("] » ");
        }

        private static void writeRed(PrintItem item)
        {
            writeDefaultHeader(item);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(item.message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }


        private static void writeNormal(PrintItem item)
        {
            writeDefaultHeader(item);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(item.message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        private static void writeSpecialLineWithHeaderHead(PrintItem item)
        {
            writeDefaultHeader(item);
            Console.ForegroundColor = item.colorOne;
            Console.Write(item.headerText);
            Console.ForegroundColor = item.colorTwo;
            Console.WriteLine(item.message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

    }
}
