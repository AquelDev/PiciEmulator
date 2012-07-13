using System.Collections.Generic;
using Butterfly.HabboHotel.Items;
using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers;
using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Effects;
using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;
using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Triggers;
using Butterfly.Messages;
using Database_Manager.Database.Session_Details.Interfaces;
using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Conditions;
using System;

namespace Butterfly.HabboHotel.Rooms.Wired
{
    class WiredSaver
    {
        internal static void HandleDefaultSave(uint itemID, Room room)
        {
            RoomItem item = room.GetRoomItemHandler().GetItem(itemID);
            if (item == null)
                return;

            InteractionType type = item.GetBaseItem().InteractionType;
            switch (type)
            {
                case InteractionType.actiongivescore:
                    {
                        int points = 0;
                        int games = 0;

                        IWiredTrigger action = new GiveScore(games, points, room.GetGameManager(), itemID);
                        HandleTriggerSave(action, room.GetWiredHandler(), room, itemID);
                        break;
                    }

                case InteractionType.actionmoverotate:
                    {
                        MovementState movement = MovementState.none;
                        RotationState rotation = RotationState.none;

                        List<RoomItem> items = new List<RoomItem>();
                        int delay = 0;

                        IWiredTrigger action = new MoveRotate(movement, rotation, items, delay, room, room.GetWiredHandler(), itemID);
                        HandleTriggerSave(action, room.GetWiredHandler(), room, itemID);
                        break;
                    }

                case InteractionType.actionposreset:
                    {
                        List<RoomItem> items = new List<RoomItem>();
                        int delay = 0;

                        IWiredTrigger action = new PositionReset(items, delay, room.GetRoomItemHandler(), room.GetWiredHandler(), itemID);
                        HandleTriggerSave(action, room.GetWiredHandler(), room, itemID);
                        break;
                    }

                case InteractionType.actionresettimer:
                    {
                        List<RoomItem> items = new List<RoomItem>();
                        int delay = 0;

                        IWiredTrigger action = new TimerReset(room, room.GetWiredHandler(), items, delay, itemID);
                        HandleTriggerSave(action, room.GetWiredHandler(), room, itemID);
                        break;
                    }

                case InteractionType.actionshowmessage:
                    {
                        string message = string.Empty;

                        IWiredTrigger action = new ShowMessage(message, room.GetWiredHandler(), itemID);
                        HandleTriggerSave(action, room.GetWiredHandler(), room, itemID);
                        break;
                    }

                case InteractionType.actionteleportto:
                    {
                        List<RoomItem> items = new List<RoomItem>();
                        int delay = 0;

                        IWiredTrigger action = new TeleportToItem(room.GetGameMap(), room.GetWiredHandler(), items, delay, itemID);
                        HandleTriggerSave(action, room.GetWiredHandler(), room, itemID);
                        break;
                    }

                case InteractionType.actiontogglestate:
                    {
                        List<RoomItem> items = new List<RoomItem>();
                        int delay = 0;

                        IWiredTrigger action = new ToggleItemState(room.GetGameMap(), room.GetWiredHandler(), items, delay, itemID);
                        HandleTriggerSave(action, room.GetWiredHandler(), room, itemID);
                        break;
                    }

                case InteractionType.conditionfurnishaveusers:
                    {

                        break;
                    }

                case InteractionType.conditionstatepos:
                    {

                        break;
                    }

                case InteractionType.conditiontimelessthan:
                    {

                        break;
                    }

                case InteractionType.conditiontimemorethan:
                    {

                        break;
                    }

                case InteractionType.conditiontriggeronfurni:
                    {

                        break;
                    }

                case InteractionType.specialrandom:
                    {

                        break;
                    }

                case InteractionType.specialunseen:
                    {

                        break;
                    }

                case InteractionType.triggergameend:
                    {
                        IWiredTrigger handler = new GameEnds(item, room.GetWiredHandler(), room.GetGameManager());
                        HandleTriggerSave(handler, room.GetWiredHandler(), room, itemID);
                        break;
                    }

                case InteractionType.triggergamestart:
                    {
                        IWiredTrigger handler = new GameStarts(item, room.GetWiredHandler(), room.GetGameManager());
                        HandleTriggerSave(handler, room.GetWiredHandler(), room, itemID);
                        break;
                    }

                case InteractionType.triggeronusersay:
                    {
                        bool isOnlyOwner = false;
                        string message = string.Empty;

                        IWiredTrigger handler = new UserSays(item, room.GetWiredHandler(), !isOnlyOwner, message, room);
                        HandleTriggerSave(handler, room.GetWiredHandler(), room, itemID);
                        break;
                    }

                case InteractionType.triggerrepeater:
                    {
                        int cycleTimes = 0;

                        IWiredTrigger handler = new Repeater(room.GetWiredHandler(), item, cycleTimes);
                        HandleTriggerSave(handler, room.GetWiredHandler(), room, itemID);
                        break;
                    }

                case InteractionType.triggerroomenter:
                    {
                        string users = string.Empty;

                        IWiredTrigger handler = new EntersRoom(item, room.GetWiredHandler(), room.GetRoomUserManager(), !string.IsNullOrEmpty(users), users);
                        HandleTriggerSave(handler, room.GetWiredHandler(), room, itemID);
                        break;
                    }

                case InteractionType.triggerscoreachieved:
                    {
                        int score = 0;

                        IWiredTrigger handler = new ScoreAchieved(item, room.GetWiredHandler(), score, room.GetGameManager());
                        HandleTriggerSave(handler, room.GetWiredHandler(), room, itemID);
                        break;
                    }

                case InteractionType.triggertimer:
                    {
                        int cycles = 0;

                        IWiredTrigger handler = new Timer(item, room.GetWiredHandler(), cycles, room.GetGameManager());
                        HandleTriggerSave(handler, room.GetWiredHandler(), room, itemID);
                        break;
                    }

                case InteractionType.triggerstatechanged:
                    {
                        List<RoomItem> items = new List<RoomItem>();
                        int delay = 0;

                        IWiredTrigger handler = new SateChanged(room.GetWiredHandler(), item, items, delay);
                        HandleTriggerSave(handler, room.GetWiredHandler(), room, itemID);

                        break;
                    }

                case InteractionType.triggerwalkofffurni:
                    {
                        List<RoomItem> items = new List<RoomItem>();

                        int delay = 0;

                        IWiredTrigger handler = new WalksOffFurni(item, room.GetWiredHandler(), items, delay);
                        HandleTriggerSave(handler, room.GetWiredHandler(), room, itemID);

                        break;
                    }

                case InteractionType.triggerwalkonfurni:
                    {
                        List<RoomItem> items = new List<RoomItem>();

                        int delay = 0;

                        IWiredTrigger handler = new WalksOnFurni(item, room.GetWiredHandler(), items, delay);
                        HandleTriggerSave(handler, room.GetWiredHandler(), room, itemID);

                        break;
                    }
            }
        }

        internal static void HandleSave(uint itemID, Room room, ClientMessage clientMessage)
        {
            RoomItem item = room.GetRoomItemHandler().GetItem(itemID);
            if (item == null)
                return;

            if (item.wiredHandler != null)
            {
                item.wiredHandler.Dispose();
                item.wiredHandler = null;
            }

            InteractionType type = item.GetBaseItem().InteractionType;
            switch (type)
            {
                case InteractionType.actiongivescore:
                    {
                        clientMessage.AdvancePointer(1);
                        int points = clientMessage.PopWiredInt32();
                        int games = clientMessage.PopWiredInt32();

                        IWiredTrigger action = new GiveScore(games, points, room.GetGameManager(), itemID);
                        HandleTriggerSave(action, room.GetWiredHandler(), room, itemID);

                        break;
                    }

                case InteractionType.actionmoverotate:
                    {
                        clientMessage.AdvancePointer(1);
                        MovementState movement = (MovementState)clientMessage.PopWiredInt32();
                        RotationState rotation = (RotationState)clientMessage.PopWiredInt32();

                        clientMessage.AdvancePointer(2);
                        int furniCount;
                        List<RoomItem> items = GetItems(clientMessage, room, out furniCount);
                        int delay = clientMessage.PopWiredInt32();

                        IWiredTrigger action = new MoveRotate(movement, rotation, items, delay, room, room.GetWiredHandler(), itemID);
                        HandleTriggerSave(action, room.GetWiredHandler(), room, itemID);

                        break;
                    }

                case InteractionType.actionposreset:
                    {

                        clientMessage.AdvancePointer(3);
                        int furniCount;
                        List<RoomItem> items = GetItems(clientMessage, room, out furniCount);
                        int delay = clientMessage.PopWiredInt32();

                        IWiredTrigger action = new PositionReset(items, delay, room.GetRoomItemHandler(), room.GetWiredHandler(), itemID);
                        HandleTriggerSave(action, room.GetWiredHandler(), room, itemID);

                        break;
                    }

                case InteractionType.actionresettimer:
                    {

                        clientMessage.AdvancePointer(3);
                        int furniCount;
                        List<RoomItem> items = GetItems(clientMessage, room, out furniCount);
                        int delay = clientMessage.PopWiredInt32();

                        IWiredTrigger action = new TimerReset(room, room.GetWiredHandler(), items, delay, itemID);
                        HandleTriggerSave(action, room.GetWiredHandler(), room, itemID);

                        break;
                    }

                case InteractionType.actionshowmessage:
                    {
                        clientMessage.AdvancePointer(1);
                        string message = clientMessage.PopFixedString();

                        IWiredTrigger action = new ShowMessage(message, room.GetWiredHandler(), itemID);
                        HandleTriggerSave(action, room.GetWiredHandler(), room, itemID);
                        break;
                    }

                case InteractionType.actionteleportto:
                    {
                        clientMessage.AdvancePointer(3);
                        int furniCount;
                        List<RoomItem> items = GetItems(clientMessage, room, out furniCount);
                        int delay = clientMessage.PopWiredInt32();

                        IWiredTrigger action = new TeleportToItem(room.GetGameMap(), room.GetWiredHandler(), items, delay, itemID);
                        HandleTriggerSave(action, room.GetWiredHandler(), room, itemID);

                        break;
                    }

                case InteractionType.actiontogglestate:
                    {
                        clientMessage.AdvancePointer(3);
                        int furniCount;
                        List<RoomItem> items = GetItems(clientMessage, room, out furniCount);
                        int delay = clientMessage.PopWiredInt32();

                        IWiredTrigger action = new ToggleItemState(room.GetGameMap(), room.GetWiredHandler(), items, delay, itemID);
                        HandleTriggerSave(action, room.GetWiredHandler(), room, itemID);

                        break;
                    }


                case InteractionType.conditionfurnishaveusers:
                    {
                        clientMessage.AdvancePointer(1);
                        bool a = clientMessage.PopWiredBoolean();
                        bool b = clientMessage.PopWiredBoolean();
                        bool c = clientMessage.PopWiredBoolean();
                        clientMessage.AdvancePointer(2);

                        int furniCount;
                        List<RoomItem> items = GetItems(clientMessage, room, out furniCount);


                        if (a)
                        {
                            int a1 = 2;
                        }

                        break;
                    }

                case InteractionType.conditionstatepos:
                    {

                        break;
                    }

                case InteractionType.conditiontimelessthan:
                    {

                        break;
                    }

                case InteractionType.conditiontimemorethan:
                    {

                        break;
                    }

                case InteractionType.conditiontriggeronfurni:
                    {

                        break;
                    }

                case InteractionType.specialrandom:
                    {

                        break;
                    }

                case InteractionType.specialunseen:
                    {

                        break;
                    }

                case InteractionType.triggergameend:
                    {
                        IWiredTrigger handler = new GameEnds(item, room.GetWiredHandler(), room.GetGameManager());
                        HandleTriggerSave(handler, room.GetWiredHandler(), room, itemID);
                        break;
                    }

                case InteractionType.triggergamestart:
                    {
                        IWiredTrigger handler = new GameStarts(item, room.GetWiredHandler(), room.GetGameManager());
                        HandleTriggerSave(handler, room.GetWiredHandler(), room, itemID);
                        break;
                    }

                case InteractionType.triggeronusersay:
                    {
                        clientMessage.AdvancePointer(1);
                        bool isOnlyOwner = clientMessage.PopWiredBoolean();
                        clientMessage.AdvancePointer(0);
                        string message = clientMessage.PopFixedString();
                        string stuff = clientMessage.ToString();

                        IWiredTrigger handler = new UserSays(item, room.GetWiredHandler(), isOnlyOwner, message, room);
                        HandleTriggerSave(handler, room.GetWiredHandler(), room, itemID);

                        break;
                    }

                case InteractionType.triggerrepeater:
                    {
                        clientMessage.AdvancePointer(1);
                        int cycleTimes = clientMessage.PopWiredInt32();
                       
                        IWiredTrigger handler = new Repeater(room.GetWiredHandler(), item, cycleTimes);
                        HandleTriggerSave(handler, room.GetWiredHandler(), room, itemID);
                       
                        break;
                    }

                case InteractionType.triggerroomenter:
                    {
                        clientMessage.AdvancePointer(1);
                        string users = clientMessage.PopFixedString();

                        IWiredTrigger handler = new EntersRoom(item, room.GetWiredHandler(), room.GetRoomUserManager(), !string.IsNullOrEmpty(users), users);
                        HandleTriggerSave(handler, room.GetWiredHandler(), room, itemID);
                        break;
                    }

                case InteractionType.triggerscoreachieved:
                    {
                        clientMessage.AdvancePointer(1);
                        int score = clientMessage.PopWiredInt32();

                        IWiredTrigger handler = new ScoreAchieved(item, room.GetWiredHandler(), score, room.GetGameManager());
                        HandleTriggerSave(handler, room.GetWiredHandler(), room, itemID);

                        break;
                    }

                case InteractionType.triggertimer:
                    {
                        clientMessage.AdvancePointer(1);
                        int cycles = clientMessage.PopWiredInt32();

                        IWiredTrigger handler = new Timer(item, room.GetWiredHandler(), cycles, room.GetGameManager());
                        HandleTriggerSave(handler, room.GetWiredHandler(), room, itemID);

                        break;
                    }

                case InteractionType.triggerstatechanged:
                    {
                        clientMessage.AdvancePointer(3);

                        int furniAmount;
                        List<RoomItem> items = GetItems(clientMessage, room, out furniAmount);
                        int delay = clientMessage.PopWiredInt32();

                        IWiredTrigger handler = new SateChanged(room.GetWiredHandler(), item, items, delay);
                        HandleTriggerSave(handler, room.GetWiredHandler(), room, itemID);

                        break;
                    }

                case InteractionType.triggerwalkofffurni:
                    {
                        clientMessage.AdvancePointer(3);

                        int furniAmount;
                        List<RoomItem> items = GetItems(clientMessage, room, out furniAmount);

                        int delay = clientMessage.PopWiredInt32();

                        IWiredTrigger handler = new WalksOffFurni(item, room.GetWiredHandler(), items, delay);
                        HandleTriggerSave(handler, room.GetWiredHandler(), room, itemID);
                        break;
                    }

                case InteractionType.triggerwalkonfurni:
                    {
                        clientMessage.AdvancePointer(3);
                        int furniAmount;
                        List<RoomItem> items = GetItems(clientMessage, room, out furniAmount);

                        int delay = clientMessage.PopWiredInt32();

                        IWiredTrigger handler = new WalksOnFurni(item, room.GetWiredHandler(), items, delay);
                        HandleTriggerSave(handler, room.GetWiredHandler(), room, itemID);

                        break;
                    }
            }
        }

        internal static void HandleConditionSave(uint itemID, Room room, ClientMessage clientMessage)
        {
            RoomItem item = room.GetRoomItemHandler().GetItem(itemID);
            if (item == null)
                return;

            if (item.wiredHandler != null)
            {
                item.wiredHandler.Dispose();
                item.wiredHandler = null;
            }

            InteractionType type = item.GetBaseItem().InteractionType;

            if (type != InteractionType.conditionfurnishaveusers && type != InteractionType.conditionstatepos &&
                type != InteractionType.conditiontimelessthan && type != InteractionType.conditiontimemorethan &&
                type != InteractionType.conditiontriggeronfurni)
                return;
            
            clientMessage.AdvancePointer(1);
            bool a = clientMessage.PopWiredBoolean();
            bool b = clientMessage.PopWiredBoolean();
            bool c = clientMessage.PopWiredBoolean();
            clientMessage.AdvancePointer(2);

            int furniCount;
            List<RoomItem> items = GetItems(clientMessage, room, out furniCount);

            IWiredCondition handler = null;

            switch (type)
            {
                case InteractionType.conditionfurnishaveusers:
                    {
                        handler = new FurniHasUser(item, items);
                        break;
                    }
                case InteractionType.conditionstatepos:
                    {
                        handler = new FurniStatePosMatch(item, items);
                        break;
                    }

                case InteractionType.conditiontimelessthan:
                    {
                        handler = new LessThanTimer(500, room, item);
                        break;
                    }

                case InteractionType.conditiontimemorethan:
                    {
                        handler = new MoreThanTimer(500, room, item);
                        break;
                    }

                case InteractionType.conditiontriggeronfurni:
                    {
                        handler = new TriggerUserIsOnFurni(item, items);
                        break;
                    }

                default:
                    return;
            }

            item.wiredCondition = handler;
            room.GetWiredHandler().conditionHandler.AddOrIgnoreRefferance(item);

            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                handler.SaveToDatabase(dbClient);
            }
        }

        private static List<RoomItem> GetItems(ClientMessage message, Room room, out int itemCount)
        {
            List<RoomItem> items = new List<RoomItem>();
            itemCount = message.PopWiredInt32();

            uint itemID;
            RoomItem item;
            for (int i = 0; i < itemCount; i++)
            {
                itemID = message.PopWiredUInt();
                item = room.GetRoomItemHandler().GetItem(itemID);

                if (item != null && !WiredUtillity.TypeIsWired(item.GetBaseItem().InteractionType))
                    items.Add(item);
            }

            return items;
        }

        private static void HandleTriggerSave(IWiredTrigger handler, WiredHandler manager, Room room, uint itemID)
        {
            RoomItem item = room.GetRoomItemHandler().GetItem(itemID);
            if (item == null)
                return;

            item.wiredHandler = handler;
            manager.RemoveFurniture(item); //Removes it from le manager just in case there is annything registered allready
            manager.AddFurniture(item);

            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                handler.SaveToDatabase(dbClient);
            }
        }
    }
}
