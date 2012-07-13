﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Net;
using System.Net.Sockets;

using Pici.HabboHotel.GameClients;
using Pici.HabboHotel.Rooms;
using Pici.Messages;
using Pici.Core;

using Pici.Storage.Database.Session_Details.Interfaces;

namespace Pici.Net
{
    class MusSocket
    {
        internal Socket msSocket;

        internal String musIp;
        internal int musPort;

        internal HashSet<String> allowedIps;

        internal MusSocket(String _musIp, int _musPort, String[] _allowedIps, int backlog)
        {
            musIp = _musIp;
            musPort = _musPort;

            allowedIps = new HashSet<String>();

            foreach (String ip in _allowedIps)
            {
                allowedIps.Add(ip);
            }

            try
            {
                msSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                msSocket.Bind(new IPEndPoint(IPAddress.Any, musPort));
                msSocket.Listen(backlog);

                msSocket.BeginAccept(OnEvent_NewConnection, msSocket);

                Logging.WriteLine("MUS socket -> READY!");
            }

            catch (Exception e)
            {
                throw new ArgumentException("Could not set up MUS socket:\n" + e.ToString());
            }
        }

        internal void OnEvent_NewConnection(IAsyncResult iAr)
        {
            try
            {
                Socket socket = ((Socket)iAr.AsyncState).EndAccept(iAr);
                String ip = socket.RemoteEndPoint.ToString().Split(':')[0];

                if (allowedIps.Contains(ip))
                {
                    MusConnection nC = new MusConnection(socket);
                }
                else
                {
                    socket.Close();
                }
            }
            catch (Exception) { }

            msSocket.BeginAccept(OnEvent_NewConnection, msSocket);
        }
    }

    class MusConnection
    {
        private Socket socket;
        private byte[] buffer = new byte[1024];

        internal MusConnection(Socket _socket)
        {
            socket = _socket;

            try
            {
                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, OnEvent_RecieveData, socket);
            }
            catch
            {
                tryClose();
            }
        }

        internal void tryClose()
        {
            try
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                socket.Dispose();
            }
            catch { }

            socket = null;
            buffer = null;
        }

        internal void OnEvent_RecieveData(IAsyncResult iAr)
        {
            try
            {
                int bytes = 0;

                try
                {
                    bytes = socket.EndReceive(iAr);
                }
                catch { tryClose(); return; }
                    
                String data = Encoding.Default.GetString(buffer, 0, bytes);

                if (data.Length > 0)
                    processCommand(data);
            }
            catch { }

            tryClose();
        }

        internal void processCommand(String data)
        {
            String header = data.Split(Convert.ToChar(1))[0];
            String param = data.Split(Convert.ToChar(1))[1];

            Logging.WriteLine("[MUSConnection.ProcessCommand]: " + data);

            GameClient Client = null;
            switch (header.ToLower())
            {
                case "updatecredits":
                    {
                        if (param == "ALL")
                        {
                         //   PiciEnvironment.GetGame().GetClientManager().GetClient()
                        }
                        else
                        {
                            Client = PiciEnvironment.GetGame().GetClientManager().GetClientByUserID(uint.Parse(param));

                            if (Client == null)
                            {
                                return;
                            }

                            DataRow newCredits;

                            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
                            {
                                dbClient.setQuery("SELECT credits FROM users WHERE id = @userid");
                                dbClient.addParameter("userid", (int)Client.GetHabbo().Id);
                                newCredits = dbClient.getRow();
                            }
                            
                            Client.GetHabbo().Credits = (int)newCredits["credits"];
                            Client.GetHabbo().UpdateCreditsBalance();
                        }

                        break;
                    }
                case "signout":
                    {
                        PiciEnvironment.GetGame().GetClientManager().GetClientByUserID(uint.Parse(param)).Disconnect();
                        break;
                    }
                
                case "ha":
                    {
                        //String extradata = data.Split(Convert.ToChar(1))[2];

                        ServerMessage HotelAlert = new ServerMessage(810);
                        HotelAlert.Append(1);
                        HotelAlert.Append(LanguageLocale.GetValue("hotelallert.notice") + "\r\n" + 
                        param + "\r\n");

                        /*if (extradata.Contains("://"))
                        {
                            Console.WriteLine("TEST");
                            HotelAlert.Append(extradata);
                        }*/

                        PiciEnvironment.GetGame().GetClientManager().QueueBroadcaseMessage(HotelAlert);
                        break;
                    }
                case "useralert":
                    {
                        String extradata = data.Split(Convert.ToChar(1))[2];
                        String url = extradata.Split(Convert.ToChar(1))[0];
                        GameClient TargetClient = null;
                        TargetClient = PiciEnvironment.GetGame().GetClientManager().GetClientByUsername(param);

                        if (TargetClient == null)
                        {
                            return;
                        }
                        if (url.Contains("://"))
                        {
                            extradata = extradata + Convert.ToChar(2) + url;
                        }
                        TargetClient.SendNotif(extradata);
                        break;
                    }
                
                default:
                    {
                        Logging.WriteLine("Unrecognized MUS packet: " + data);
                        break;
                    }
            }
        }
    }
}
