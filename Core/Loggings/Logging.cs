using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace Pici.Core.Loggings
{
    public class Writer
    {
        private static Thread logWriter;
        private static Queue logQueue;

        public static void Init()
        {
            logQueue = new Queue();
            //logWriter = new Thread(new ThreadStart(WriteToLogFile));
            //logWriter.Start();
        }

        private static void WriteToLogFile()
        {
            while (true)
            {
                try
                {
                    Dictionary<string, StringBuilder> fileContent = new Dictionary<string, StringBuilder>();
                    while (logQueue.Count > 0)
                    {
                        LogMessage message = null;
                        lock (logQueue.SyncRoot)
                        {
                            if (logQueue.Count > 0)
                                message = (LogMessage)logQueue.Dequeue();
                        }

                        if (message != null)
                        {
                            if (fileContent.ContainsKey(message.location))
                                fileContent[message.location].Append(message.message);
                            else
                                fileContent.Add(message.location, new StringBuilder(message.message));
                        }
                    }

                    foreach (KeyValuePair<string, StringBuilder> fileEntry in fileContent)
                    {
                        WriteFileContent(fileEntry.Value.ToString(), fileEntry.Key);
                    }

                    fileContent.Clear();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error in console writer: " + e.ToString());
                }

                Thread.Sleep(3000);
            }
        }

        private static void WriteFileContent(string message, string location)
        {
            FileStream fStream = new FileStream(location, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 4096, true);
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(Environment.NewLine + message);

            IAsyncResult asyncResult = fStream.BeginWrite(
                bytes, 0, bytes.Length,
                new AsyncCallback(WriteCallback),
                fStream);
        }


        private static bool mDisabled = false;

        public static bool DisabledState
        {
            get
            {
                return mDisabled;
            }
            set
            {
                mDisabled = value;
            }
        }

        public static void WriteLine(string Line)
        {
            if (!mDisabled)
                Console.WriteLine(Line);
        }

        public static void LogException(string logText)
        {
            WriteToFile(@"Logs\exceptions.txt", logText + "\r\n\r\n");
            WriteLine("Exception has been saved");
        }

        public static void LogCriticalException(string logText)
        {
            WriteToFile(@"Logs\criticalexceptions.txt", logText + "\r\n\r\n");
            WriteLine("CRITICAL ERROR LOGGED");
        }

        public static void LogCacheError(string logText)
        {
            WriteToFile(@"Logs\cacheerror.txt", logText + "\r\n\r\n");
            WriteLine("Critical error saved");
        }

        public static void LogMessage(string logText)
        {
            WriteToFile(@"Logs\logg.txt", logText + "\r\n\r\n");
            WriteLine(logText);
        }

        public static void LogDDOS(string logText)
        {
            WriteToFile(@"Logs\ddos.txt", logText + "\r\n\r\n");
            WriteLine(logText);
        }

        public static void LogThreadException(string Exception, string Threadname)
        {
            WriteToFile(@"Logs\threaderror.txt", "Error in thread " + Threadname + ": \r\n" + Exception + "\r\n\r\n");
            WriteLine("Error in " + Threadname + " caught");
        }

        public static void LogQueryError(Exception Exception, string query)
        {
            WriteToFile(@"Logs\MySQLerrors.txt", "Error in query: \r\n" + query + "\r\n" + Exception + "\r\n\r\n");
            WriteLine("Error in query caught");
        }

        public static void LogPacketException(string Packet, string Exception)
        {
            WriteToFile(@"Logs\packeterror.txt", "Error in packet " + Packet + ": \r\n" + Exception + "\r\n\r\n");
            WriteLine("User disconnection logged");
        }

        public static void HandleException(Exception pException, string pLocation)
        {
            StringBuilder ExceptionData = new StringBuilder();
            ExceptionData.AppendLine("Exception logged " + DateTime.Now.ToString() + " in " + pLocation + ":");
            ExceptionData.AppendLine(pException.ToString());
            if (pException.InnerException != null)
            {
                ExceptionData.AppendLine("Inner exception:");
                ExceptionData.AppendLine(pException.InnerException.ToString());
            }
            if (pException.HelpLink != null)
            {
                ExceptionData.AppendLine("Help link:");
                ExceptionData.AppendLine(pException.HelpLink);
            }
            if (pException.Source != null)
            {
                ExceptionData.AppendLine("Source:");
                ExceptionData.AppendLine(pException.Source);
            }
            if (pException.Data != null)
            {
                ExceptionData.AppendLine("Data:");
                foreach (DictionaryEntry Entry in pException.Data)
                {
                    ExceptionData.AppendLine("  Key: " + Entry.Key + "Value: " + Entry.Value);
                }
            }
            if (pException.Message != null)
            {
                ExceptionData.AppendLine("Message:");
                ExceptionData.AppendLine(pException.Message);
            }
            if (pException.StackTrace != null)
            {
                ExceptionData.AppendLine("Stack trace:");
                ExceptionData.AppendLine(pException.StackTrace);
            }
            ExceptionData.AppendLine();
            ExceptionData.AppendLine();
            LogException(ExceptionData.ToString());
        }

        public static void DisablePrimaryWriting(bool ClearConsole)
        {
            mDisabled = true;
            if (ClearConsole)
                Console.Clear();
        }

        public static void LogShutdownError(string p)
        {
            ///thRow new NotImplementedException();
        }

        public static void LogShutdown(StringBuilder builder)
        {
            WriteToFile(@"Logs\shutdownlog.txt", builder.ToString());
        }

        private static void WriteToFile(string path, string content)
        {
            try
            {
                //LogMessage message = new LogMessage(content, path);
                //lock (logQueue.SyncRoot)
                //{
                //    logQueue.Enqueue(message);
                //}

                FileStream errWriter = new System.IO.FileStream(path, System.IO.FileMode.Append, System.IO.FileAccess.Write);
                byte[] Msg = ASCIIEncoding.ASCII.GetBytes(Environment.NewLine + content);
                errWriter.Write(Msg, 0, Msg.Length);
                errWriter.Dispose();

                
                //// Create random data to write to the file.
                //FileStream fStream =
                //    new FileStream("Test#@@#.dat", FileMode.Create,
                //    FileAccess.ReadWrite, FileShare.None, 4096, true);

                //// Check that the FileStream was opened asynchronously.
                //Console.WriteLine("fStream was {0}opened asynchronously.",
                //    fStream.IsAsync ? "" : "not ");

                //// Asynchronously write to the file.
                //IAsyncResult asyncResult = fStream.BeginWrite(
                //    Msg, 0, Msg.Length,
                //    new AsyncCallback(WriteCallback),
                //    fStream);
            }
            catch (Exception e)
            {
                WriteLine("Could not write to file: " + e.ToString() + ":" + content);
            }
        }

        private static void WriteCallback(IAsyncResult callback)
        {
            FileStream stream = (FileStream)callback.AsyncState;
            stream.EndWrite(callback);
            stream.Dispose();
        }

        public static void Main(string[] argg)
        {

        }
    }
}
