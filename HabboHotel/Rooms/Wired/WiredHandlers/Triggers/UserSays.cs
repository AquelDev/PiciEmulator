using Pici.HabboHotel.Items;
using Pici.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;
using Pici.Storage.Database.Session_Details.Interfaces;
using System.Data;

namespace Pici.HabboHotel.Rooms.Wired.WiredHandlers.Triggers
{
    class UserSays : IWiredTrigger
    {
        private RoomItem item;
        private WiredHandler handler;
        private bool isOwnerOnly;
        private string triggerMessage;
        private RoomUserSaysDelegate delegateFunction;

        public UserSays(RoomItem item, WiredHandler handler, bool isOwnerOnly, string triggerMessage, Room room)
        {
            this.item = item;
            this.handler = handler;
            this.isOwnerOnly = isOwnerOnly;
            this.triggerMessage = triggerMessage;
            this.delegateFunction = new RoomUserSaysDelegate(roomUserManager_OnUserSays);

            room.OnUserSays += delegateFunction;
        }

        private void roomUserManager_OnUserSays(object sender, UserSaysArgs e, out bool messageHandled)
        {
            RoomUser userSaying = e.user;
            string message = e.message;

            if ((!isOwnerOnly && canBeTriggered(message)) || (isOwnerOnly && userSaying.IsOwner() && canBeTriggered(message)))
            {
                handler.RequestStackHandle(item.Coordinate, null, userSaying, Games.Team.none);
                handler.OnEvent(item.Id);
                messageHandled = true;
            }
            else
                messageHandled = false;
        }

        private bool canBeTriggered(string message)
        {
            return message.ToLower() == triggerMessage.ToLower();
        }

        public void Dispose()
        {
            handler.GetRoom().OnUserSays -= delegateFunction;
            item = null;
            handler = null;
            triggerMessage = null;
        }


        public void SaveToDatabase(IQueryAdapter dbClient)
        {
            WiredUtillity.SaveTriggerItem(dbClient, (int)item.Id, "string", string.Empty, triggerMessage, isOwnerOnly);
        }

        public void LoadFromDatabase(IQueryAdapter dbClient, Room insideRoom)
        {
            dbClient.setQuery("SELECT trigger_data,  all_user_triggerable FROM trigger_item WHERE trigger_id = @id ");
            dbClient.addParameter("id", (int)this.item.Id);
            DataRow dRow = dbClient.getRow();
            if (dRow != null)
            {
                this.triggerMessage = dRow[0].ToString();
                this.isOwnerOnly = dRow[1].ToString() == "1";
            }
            else
            {
                this.triggerMessage = string.Empty;
                this.isOwnerOnly = false;
            }
        }


        public void DeleteFromDatabase(IQueryAdapter dbClient)
        {
            dbClient.runFastQuery("DELETE FROM trigger_item WHERE trigger_id = '" + this.item.Id + "'");
        }
    }
}
