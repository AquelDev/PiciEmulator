using System.Collections.Generic;
using Butterfly.HabboHotel.Items;
using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers;
using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Effects;
using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;
using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Triggers;
using Database_Manager.Database.Session_Details.Interfaces;
using System;
using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Conditions;

namespace Butterfly.HabboHotel.Rooms.Wired
{
    class WiredLoader
    {
        internal static void LoadWiredItem(RoomItem item, Room room, IQueryAdapter dbClient)
        {
            InteractionType type = item.GetBaseItem().InteractionType;
            switch (type)
            {
                case InteractionType.actiongivescore:
                    {
                        IWiredTrigger action = new GiveScore(0, 0, room.GetGameManager(), item.Id);
                        action.LoadFromDatabase(dbClient, room);
                        HandleItemLoad(action, room.GetWiredHandler(), item);
                        break;
                    }

                case InteractionType.actionmoverotate:
                    {
                        IWiredTrigger action = new MoveRotate(MovementState.none, RotationState.none,new List<RoomItem>(), 0, room, room.GetWiredHandler(), item.Id);
                        action.LoadFromDatabase(dbClient, room);
                        HandleItemLoad(action, room.GetWiredHandler(), item);
                        break;
                    }

                case InteractionType.actionposreset:
                    {
                        IWiredTrigger action = new PositionReset(new List<RoomItem>(), 0, room.GetRoomItemHandler(), room.GetWiredHandler(), item.Id);
                        action.LoadFromDatabase(dbClient, room);
                        HandleItemLoad(action, room.GetWiredHandler(), item);
                        break;
                    }

                case InteractionType.actionresettimer:
                    {
                        IWiredTrigger action = new TimerReset(room, room.GetWiredHandler(), new List<RoomItem>(), 0, item.Id);
                        action.LoadFromDatabase(dbClient, room);
                        HandleItemLoad(action, room.GetWiredHandler(), item);
                        break;
                    }

                case InteractionType.actionshowmessage:
                    {
                        IWiredTrigger action = new ShowMessage(string.Empty, room.GetWiredHandler(), item.Id);
                        action.LoadFromDatabase(dbClient, room);
                        HandleItemLoad(action, room.GetWiredHandler(), item);
                        break;
                    }

                case InteractionType.actionteleportto:
                    {
                        IWiredTrigger action = new TeleportToItem(room.GetGameMap(), room.GetWiredHandler(), new List<RoomItem>(), 0, item.Id);
                        action.LoadFromDatabase(dbClient, room);
                        HandleItemLoad(action, room.GetWiredHandler(), item);
                        break;
                    }

                case InteractionType.actiontogglestate:
                    {
                        IWiredTrigger action = new ToggleItemState(room.GetGameMap(), room.GetWiredHandler(), new List<RoomItem>(), 0, item.Id);
                        action.LoadFromDatabase(dbClient, room);
                        HandleItemLoad(action, room.GetWiredHandler(), item);
                        break;
                    }

                case InteractionType.conditionfurnishaveusers:
                    {
                        IWiredCondition furniHasUsers = new FurniHasUser(item, new List<RoomItem>());
                        furniHasUsers.LoadFromDatabase(dbClient, room);
                        HandleConditionLoad(furniHasUsers, item);
                        break;
                    }

                case InteractionType.conditionstatepos:
                    {
                        IWiredCondition furnistatepos = new FurniStatePosMatch(item, new List<RoomItem>());
                        furnistatepos.LoadFromDatabase(dbClient, room);
                        HandleConditionLoad(furnistatepos, item);
                        break;
                    }

                case InteractionType.conditiontimelessthan:
                    {
                        IWiredCondition timeLessThan = new LessThanTimer(0, room, item);
                        timeLessThan.LoadFromDatabase(dbClient, room);
                        HandleConditionLoad(timeLessThan, item);
                        break;
                    }

                case InteractionType.conditiontimemorethan:
                    {
                        IWiredCondition timeMoreThan = new MoreThanTimer(0, room, item);
                        timeMoreThan.LoadFromDatabase(dbClient, room);
                        HandleConditionLoad(timeMoreThan, item);
                        break;
                    }

                case InteractionType.conditiontriggeronfurni:
                    {
                        IWiredCondition triggerOnFurni = new TriggerUserIsOnFurni(item, new List<RoomItem>());
                        triggerOnFurni.LoadFromDatabase(dbClient, room);
                        HandleConditionLoad(triggerOnFurni, item);
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
                        HandleItemLoad(handler, room.GetWiredHandler(), item);
                        break;
                    }

                case InteractionType.triggergamestart:
                    {
                        IWiredTrigger handler = new GameStarts(item, room.GetWiredHandler(), room.GetGameManager());
                        HandleItemLoad(handler, room.GetWiredHandler(), item);
                        break;
                    }

                case InteractionType.triggeronusersay:
                    {
                        IWiredTrigger handler = new UserSays(item, room.GetWiredHandler(), false, string.Empty, room);
                        handler.LoadFromDatabase(dbClient, room);
                        HandleItemLoad(handler, room.GetWiredHandler(), item);
                        break;
                    }

                case InteractionType.triggerrepeater:
                    {
                        IWiredTrigger handler = new Repeater(room.GetWiredHandler(), item, 0);
                        handler.LoadFromDatabase(dbClient, room);
                        HandleItemLoad(handler, room.GetWiredHandler(), item);
                        break;
                    }

                case InteractionType.triggerroomenter:
                    {
                        IWiredTrigger handler = new EntersRoom(item, room.GetWiredHandler(), room.GetRoomUserManager(), false, string.Empty);
                        handler.LoadFromDatabase(dbClient, room);
                        HandleItemLoad(handler, room.GetWiredHandler(), item);
                        break;
                    }

                case InteractionType.triggerscoreachieved:
                    {
                        IWiredTrigger handler = new ScoreAchieved(item, room.GetWiredHandler(), 0, room.GetGameManager());
                        handler.LoadFromDatabase(dbClient, room);
                        HandleItemLoad(handler, room.GetWiredHandler(), item);
                        break;
                    }

                case InteractionType.triggertimer:
                    {
                        IWiredTrigger handler = new Timer(item, room.GetWiredHandler(), 0, room.GetGameManager());
                        handler.LoadFromDatabase(dbClient, room);
                        HandleItemLoad(handler, room.GetWiredHandler(), item);
                        break;
                    }

                case InteractionType.triggerstatechanged:
                    {
                        IWiredTrigger handler = new SateChanged(room.GetWiredHandler(), item, new List<RoomItem>(), 0);
                        handler.LoadFromDatabase(dbClient, room);
                        HandleItemLoad(handler, room.GetWiredHandler(), item);
                        break;
                    }

                case InteractionType.triggerwalkofffurni:
                    {
                        IWiredTrigger handler = new WalksOffFurni(item, room.GetWiredHandler(), new List<RoomItem>(), 0);
                        handler.LoadFromDatabase(dbClient, room);
                        HandleItemLoad(handler, room.GetWiredHandler(), item);
                        break;
                    }

                case InteractionType.triggerwalkonfurni:
                    {
                        IWiredTrigger handler = new WalksOnFurni(item, room.GetWiredHandler(), new List<RoomItem>(), 0);
                        handler.LoadFromDatabase(dbClient, room);
                        HandleItemLoad(handler, room.GetWiredHandler(), item);
                        break;
                    }
            }

        }


        private static void HandleItemLoad(IWiredTrigger handler, WiredHandler wiredHandler, RoomItem item)
        {
            if (item.wiredHandler != null)
                item.wiredHandler.Dispose();

            item.wiredHandler = handler;
        }

        private static void HandleConditionLoad(IWiredCondition handler, RoomItem item)
        {
            if (item.wiredCondition != null)
                item.wiredCondition.Dispose();

            item.wiredCondition = handler;
        }
    }

}
