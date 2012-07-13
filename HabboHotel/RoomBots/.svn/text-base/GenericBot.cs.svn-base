using System;
using Butterfly.Core;
using Butterfly.HabboHotel.Pathfinding;
using Butterfly.HabboHotel.Rooms;
using Butterfly.Messages;
using System.Drawing;

namespace Butterfly.HabboHotel.RoomBots
{
    class GenericBot : BotAI
    {
        private int SpeechTimer;
        private int ActionTimer;

        internal GenericBot(int VirtualId)
        {
            this.SpeechTimer = new Random((VirtualId ^ 2) + DateTime.Now.Millisecond).Next(10, 250);
            this.ActionTimer = new Random((VirtualId ^ 2) + DateTime.Now.Millisecond).Next(10, 30);
        }

        internal override void OnSelfEnterRoom()
        {
            
        }

        internal override void OnSelfLeaveRoom(bool Kicked)
        {
            
        }

        internal override void OnUserEnterRoom(Rooms.RoomUser User)
        {
            
        }

        internal override void OnUserLeaveRoom(GameClients.GameClient Client)
        {
            
        }

        internal override void OnUserSay(Rooms.RoomUser User, string Message)
        {
            if (Gamemap.TileDistance(GetRoomUser().X, GetRoomUser().Y, User.X, User.Y) > 8)
            {
                return;
            }

            BotResponse Response = GetBotData().GetResponse(Message);

            if (Response == null)
            {
                return;
            }

            switch (Response.ResponseType.ToLower())
            {
                case "say":

                    GetRoomUser().Chat(null, Response.ResponseText, false);
                    break;

                case "shout":

                    GetRoomUser().Chat(null, Response.ResponseText, true);
                    break;

                case "whisper":

                    ServerMessage TellMsg = new ServerMessage(25);
                    TellMsg.AppendInt32(GetRoomUser().VirtualId);
                    TellMsg.AppendStringWithBreak(Response.ResponseText);
                    TellMsg.AppendBoolean(false);

                    User.GetClient().SendMessage(TellMsg);
                    break;
            }

            if (Response.ServeId >= 1)
            {
                User.CarryItem(Response.ServeId);
            }
        }

        internal override void OnUserShout(Rooms.RoomUser User, string Message)
        {
            if (ButterflyEnvironment.GetRandomNumber(0, 10) >= 5)
            {
                GetRoomUser().Chat(null, LanguageLocale.GetValue("onusershout"), true); // shout nag
            }
        }

        internal override void OnTimerTick()
        {
            if (GetBotData() == null)
                return;
            if (SpeechTimer <= 0)
            {
                if (GetBotData().RandomSpeech.Count > 0)
                {
                    RandomSpeech Speech = GetBotData().GetRandomSpeech();
                    GetRoomUser().Chat(null, Speech.Message, Speech.Shout);
                }

                SpeechTimer = ButterflyEnvironment.GetRandomNumber(10, 300);
            }
            else
            {
                SpeechTimer--;
            }

            if (ActionTimer <= 0)
            {
                switch (GetBotData().WalkingMode.ToLower())
                {
                    default:
                    case "stand":

                        // (8) Why is my life so boring?

                        break;

                    case "freeroam":
                        Point nextCoord = GetRoom().GetGameMap().getRandomWalkableSquare();
                        GetRoomUser().MoveTo(nextCoord.X, nextCoord.Y);
                        
                        break;

                    case "specified_range":
                        Point nextCoord2 = GetRoom().GetGameMap().getRandomWalkableSquare();
                        GetRoomUser().MoveTo(nextCoord2.X, nextCoord2.Y);
                        
                        break;
                }

                ActionTimer = ButterflyEnvironment.GetRandomNumber(1, 30);
            }
            else
            {
                ActionTimer--;
            }
        }
    }
}