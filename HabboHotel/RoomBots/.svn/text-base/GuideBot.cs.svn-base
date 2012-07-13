
using Butterfly.Core;
using Butterfly.HabboHotel.Pathfinding;
using Butterfly.HabboHotel.Rooms;
using Butterfly.Messages;
using System.Drawing;

namespace Butterfly.HabboHotel.RoomBots
{
    class GuideBot : BotAI
    {
        private int SpeechTimer;
        private int ActionTimer;

        internal GuideBot()
        {
            this.SpeechTimer = 0;
            this.ActionTimer = 0;
        }

        internal override void OnSelfEnterRoom()
        {
            GetRoomUser().Chat(null, LanguageLocale.GetValue("guide.message"), true);
            //GetRoomUser().Chat(null, "This is your own room, you can always come back to room by clicking the nest icon on the left.", false);
            //GetRoomUser().Chat(null, "If you want to explore the Habbo by yourself, click on the orange hotel icon on the left (we call it navigator).", false);
            //GetRoomUser().Chat(null, "You will find cool rooms and fun events with other people in them, feel free to visit them.", false);
            GetRoomUser().Chat(null, LanguageLocale.GetValue("guide.question"), false);
        }

        internal override void OnSelfLeaveRoom(bool Kicked) { }
        internal override void OnUserEnterRoom(Rooms.RoomUser User) { }

        internal override void OnUserLeaveRoom(GameClients.GameClient Client)
        {
            if (GetRoom() != null && Client != null && Client.GetHabbo() != null)
            {
                if (GetRoom().Owner.ToLower() == Client.GetHabbo().Username.ToLower())
                {
                    GetRoom().GetRoomUserManager().RemoveBot(GetRoomUser().VirtualId, false);
                }
            }
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

        internal override void OnUserShout(Rooms.RoomUser User, string Message) { }

        internal override void OnTimerTick()
        {
            if (SpeechTimer <= 0)
            {
                if (GetBotData() != null)
                {
                    if (GetBotData().RandomSpeech.Count > 0)
                    {
                        RandomSpeech Speech = GetBotData().GetRandomSpeech();
                        GetRoomUser().Chat(null, Speech.Message, Speech.Shout);
                    }
                }

                SpeechTimer = ButterflyEnvironment.GetRandomNumber(0, 150);
            }
            else
            {
                SpeechTimer--;
            }

            if (ActionTimer <= 0)
            {
                Point nextCoord = GetRoom().GetGameMap().getRandomWalkableSquare();
                //int randomX = ButterflyEnvironment.GetRandomNumber(0, GetRoom().Model.MapSizeX);
                //int randomY = ButterflyEnvironment.GetRandomNumber(0, GetRoom().Model.MapSizeY);

                GetRoomUser().MoveTo(nextCoord.X, nextCoord.Y);

                ActionTimer = ButterflyEnvironment.GetRandomNumber(0, 30);
            }
            else
            {
                ActionTimer--;
            }
        }
    }
}
