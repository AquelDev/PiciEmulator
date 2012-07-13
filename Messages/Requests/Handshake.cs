
using Pici.Core;
using System;
using Pici.Messages.Interfaces;
using Pici.HabboHotel.GameClients;
using Pici.Messages.Headers;
namespace Pici.Messages
{
    class InitCryptoMessageEvent : Event
    {
        public void Invoke(GameClient Session, ClientMessage Request)
        {
            ServerMessage InitCrypto = new ServerMessage(MessageComposerIds.InitCryptoMessageComposer);
            InitCrypto.Append(default(int));
        }
    }

    class SSOTicketMessageEvent : Event
    {
        public void Invoke(GameClient Session, ClientMessage Request)
        {
            if (Session.GetHabbo() == null)
            {
                Session.tryLogin(Request.PopFixedString());
            }
            else
                Session.SendNotif(LanguageLocale.GetValue("user.allreadylogedon"));
        }
    }

    partial class GameClientMessageHandler
    {
        internal void SendSessionParams()
        {
            Response.Init(257);
            Response.Append(9);
            Response.Append(0);
            Response.Append(0);
            Response.Append(1);
            Response.Append(1);
            Response.Append(3);
            Response.Append(0);
            Response.Append(2);
            Response.Append(1);
            Response.Append(4);
            Response.Append(1);
            Response.Append(5);
            Response.Append("dd-MM-yyyy");
            Response.Append(7);
            Response.AppendBoolean(false);
            Response.Append(8);
            Response.Append("/client");
            Response.Append(9);
            Response.AppendBoolean(false);

            SendResponse();
        }

        internal void SSOLogin()
        {
            if (Session.GetHabbo() == null)
            {
                Session.tryLogin(Request.PopFixedString());
                //if (Session.tryLogin(Request.PopFixedString()))
                //{
                //    //RegisterCatalog();
                //    //RegisterHelp();
                //    //RegisterNavigator();
                //    //RegisterMessenger();
                //    //RegisterUsers();
                //    //RegisterRooms();
                //    //RegisterGroups();
                //    //RegisterQuests();
                //}
            }
            else
                Session.SendNotif(LanguageLocale.GetValue("user.allreadylogedon"));
        }

        //internal void RegisterHandshake()
        //{
        //    RequestHandlers.Add(206, new RequestHandler(SendSessionParams));
        //    RequestHandlers.Add(415, new RequestHandler(SSOLogin));
        //}

        //internal void UnRegisterHandshake()
        //{
        //    RequestHandlers.Remove(206);
        //    RequestHandlers.Remove(415);
        //}
    }
}
