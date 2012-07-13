using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using Pici.Core;
using Pici.HabboHotel.GameClients;
using Pici.Messages.StaticMessageHandlers;
using Pici.Messages.Interfaces;

namespace Pici.Messages
{
    partial class GameClientMessageHandler
    {
        //internal static int timeOut = 300;

        private GameClient Session;
        private ClientMessage Request;
        private ServerMessage Response;

        /// <summary>
        /// Storage of names from Info Events.
        /// </summary>
        public Dictionary<string, short> InfoEvents;

        /// <summary>
        /// Storage of Invokers from Message Events
        /// </summary>
        public Dictionary<short, Event> MessageEvents;

        //private delegate void RequestHandler();
        //private Dictionary<uint, RequestHandler> RequestHandlers;

        public GameClientMessageHandler(GameClient Session)
        {
            this.Session = Session;
            //this.RequestHandlers = new Dictionary<uint, RequestHandler>();
            this.Response = new ServerMessage(0);
        }

        internal ServerMessage GetResponse()
        {
            return Response;
        }

        internal void Destroy()
        {
           // RequestHandlers.Clear();
            Session = null;
        }

        internal void HandleRequest(ClientMessage request)
        {
            DateTime start = DateTime.Now;
            this.Request = request;
            StaticClientMessageHandler.HandlePacket(this, request);

            TimeSpan spent = DateTime.Now - start;
            if (spent.TotalMilliseconds > PiciEnvironment.timeout)
            {
                Console.WriteLine("Packet " + request.Id + " took " + spent.Milliseconds + "ms to run. Packetdata: " + request.ToString());
            }
            //RequestHandler handler;

            //if (RequestHandlers.TryGetValue(pRequest.Id, out handler))
            //{
            //    //Console.ForegroundColor = ConsoleColor.Green;
            //    //Console.WriteLine(string.Format("Processing packetID => [{0}]", pRequest.Id));
            //    //DateTime start = DateTime.Now;
            //    handler.Invoke();

            //    //TimeSpan spent = DateTime.Now - start;

            //    //int msSpent = (int)spent.TotalMilliseconds;

            //    //if (msSpent > timeOut)
            //    //{
            //    //    Logging.LogCriticalException(start.ToString() +  " PacketID: " + pRequest.Id + ", total time: " + msSpent);
            //    //}

            //    //Console.WriteLine(string.Format("[{0}] => Invoked [{1} ticks]", pRequest.Id, spent.Ticks));


            //}
            //else
            //{
            //    //Console.ForegroundColor = ConsoleColor.Red;
            //Console.WriteLine(string.Format("Unknown packetID => [{0}] data: [{1}]", pRequest.Id, pRequest.Header));
            //}

            //TimeSpan TimeUsed = DateTime.Now - start;
            //if (TimeUsed.Milliseconds > 0 || TimeUsed.Seconds > 0)
            //    Console.WriteLine("Total used time: " + TimeUsed.Seconds + "s, " + TimeUsed.Milliseconds + "ms");


        }

        internal void SendResponse()
        {
            if (Response != null)
                if (Response.Id > 0)
                    if (Session.GetConnection() != null)
                        Session.GetConnection().SendData(Response.GetBytes());
        }
    }
}
