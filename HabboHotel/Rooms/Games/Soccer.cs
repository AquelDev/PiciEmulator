﻿using System;
using System.Collections.Generic;
using Pici.Collections;
using Pici.HabboHotel.GameClients;
using Pici.HabboHotel.Items;
using Pici.HabboHotel.Pathfinding;
using Pici.Messages;
using System.Drawing;

namespace Pici.HabboHotel.Rooms.Games
{
    class Soccer
    {
        private RoomItem[] gates;
        private Room room;
        private QueuedDictionary<uint, RoomItem> balls;

        public Soccer(Room room)
        {
            this.room = room;
            this.gates = new RoomItem[4];
            this.balls = new QueuedDictionary<uint, RoomItem>();
        }

        internal void OnCycle()
        {
            balls.OnCycle();
        }

        internal void AddBall(RoomItem item)
        {
            balls.Add(item.Id, item);
        }

        internal void RemoveBall(uint itemID)
        {
            balls.Remove(itemID);
        }

        internal void OnUserWalk(RoomUser User)
        {
            if (User == null)
                return;
            foreach (RoomItem item in balls.Values)
            {
                int differenceX = User.X - item.GetX;
                int differenceY = User.Y - item.GetY;

                if (differenceX <= 1 && differenceX >= -1 && differenceY <= 1 && differenceY >= -1)
                {
                    int NewX = differenceX * -1;
                    int NewY = differenceY * -1;

                    NewX = NewX + item.GetX;
                    NewY = NewY + item.GetY;

                    if (item.interactingBallUser == User.userID && item.GetRoom().GetGameMap().ValidTile(NewX, NewY))
                    {
                        item.interactingBallUser = 0;
                        MoveBall(item, User.GetClient(), User.Coordinate, item.Coordinate, 6);
                    }
                    else if (item.GetRoom().GetGameMap().ValidTile(NewX, NewY))
                    {
                        MoveBall(item, User.GetClient(), NewX, NewY);
                    }
                }
            }
        }

        internal void RegisterGate(RoomItem item)
        {
            if (gates[0] == null)
            {
                item.team = Team.blue;
                gates[0] = item;
            }
            else if (gates[1] == null)
            {
                item.team = Team.red;
                gates[1] = item;
            }
            else if (gates[2] == null)
            {
                item.team = Team.green;
                gates[2] = item;
            }
            else if (gates[3] == null)
            {
                item.team = Team.yellow;
                gates[3] = item;
            }
        }

        internal void UnRegisterGate(RoomItem item)
        {
            switch (item.team)
            {
                case Team.blue:
                    {
                        gates[0] = null;
                        break;
                    }
                case Team.red:
                    {
                        gates[1] = null;
                        break;
                    }
                case Team.green:
                    {
                        gates[2] = null;
                        break;
                    }
                case Team.yellow:
                    {
                        gates[3] = null;
                        break;
                    }
            }
        }

        internal void onGateRemove(RoomItem item)
        {
            switch (item.GetBaseItem().InteractionType)
            {
                case InteractionType.footballgoalred:
                case InteractionType.footballcounterred:
                    {

                        room.GetGameManager().RemoveFurnitureFromTeam(item, Team.red);
                        break;
                    }
                case InteractionType.footballgoalgreen:
                case InteractionType.footballcountergreen:
                    {
                        room.GetGameManager().RemoveFurnitureFromTeam(item, Team.green);
                        break;
                    }
                case InteractionType.footballgoalblue:
                case InteractionType.footballcounterblue:
                    {
                        room.GetGameManager().RemoveFurnitureFromTeam(item, Team.blue);
                        break;
                    }
                case InteractionType.footballgoalyellow:
                case InteractionType.footballcounteryellow:
                    {
                        room.GetGameManager().RemoveFurnitureFromTeam(item, Team.yellow);
                        break;
                    }
            }
        }


        internal void MoveBall(RoomItem item, GameClient mover, int newX, int newY)
        {
            if (item == null || mover == null)
                return;

            if (!room.GetGameMap().itemCanBePlacedHere(newX, newY))
                return;

            Point oldRoomCoord = item.Coordinate;
            bool itemIsOnGameItem = GameItemOverlaps(item);

            Double NewZ = room.GetGameMap().Model.SqFloorHeight[newX, newY];

            ServerMessage Message = new ServerMessage(95);
            Message.AppendUInt(item.Id);
            Message.AppendInt32(3508);
            Message.AppendInt32(item.Coordinate.X);
            Message.AppendInt32(item.Coordinate.Y);
            Message.AppendInt32(newX);
            Message.AppendInt32(newY);
            Message.AppendInt32(4);
            Message.AppendStringWithBreak(NewZ.ToString());
            Message.AppendStringWithBreak("H11");
            Message.AppendInt32(-1);
            Message.AppendInt32(0);
            room.SendMessage(Message);

            ServerMessage mMessage = new ServerMessage();
            mMessage.Init(230); // Cf
            mMessage.AppendInt32(item.Coordinate.X);
            mMessage.AppendInt32(item.Coordinate.Y);
            mMessage.AppendInt32(newX);
            mMessage.AppendInt32(newY);
            mMessage.AppendInt32(1);
            mMessage.AppendUInt(item.Id);
            mMessage.AppendStringWithBreak(item.GetZ.ToString().Replace(',', '.'));
            mMessage.AppendStringWithBreak(NewZ.ToString().Replace(',', '.'));
            mMessage.AppendInt32(0);
            room.SendMessage(mMessage);

            if (oldRoomCoord.X == newX && oldRoomCoord.Y == newY)
                return;

            room.GetRoomItemHandler().SetFloorItem(mover, item, newX, newY, item.Rot, false, false, false, false);

            if (!itemIsOnGameItem)
                HandleFootballGameItems(new Point(newX, newY), room.GetRoomUserManager().GetRoomUserByHabbo(mover.GetHabbo().Id));
        }


        private void HandleFootballGameItems(Point ballItemCoord, RoomUser user)
        {
            if (user.team == Team.none)
                return;

            foreach (RoomItem item in room.GetGameManager().GetItems(Team.red).Values)
            {
                foreach (ThreeDCoord tile in item.GetAffectedTiles.Values)
                {
                    if (tile.X == ballItemCoord.X && tile.Y == ballItemCoord.Y)
                    {
                        room.GetGameManager().AddPointToTeam(user.team, user);
                        return;
                    }
                }
            }

            foreach (RoomItem item in room.GetGameManager().GetItems(Team.green).Values)
            {
                foreach (ThreeDCoord tile in item.GetAffectedTiles.Values)
                {
                    if (tile.X == ballItemCoord.X && tile.Y == ballItemCoord.Y)
                    {
                        room.GetGameManager().AddPointToTeam(user.team, user);
                        return;
                    }
                }
            }

            foreach (RoomItem item in room.GetGameManager().GetItems(Team.blue).Values)
            {
                foreach (ThreeDCoord tile in item.GetAffectedTiles.Values)
                {
                    if (tile.X == ballItemCoord.X && tile.Y == ballItemCoord.Y)
                    {
                        room.GetGameManager().AddPointToTeam(user.team, user);
                        return;
                    }
                }
            }

            foreach (RoomItem item in room.GetGameManager().GetItems(Team.yellow).Values)
            {
                foreach (ThreeDCoord tile in item.GetAffectedTiles.Values)
                {
                    if (tile.X == ballItemCoord.X && tile.Y == ballItemCoord.Y)
                    {
                        room.GetGameManager().AddPointToTeam(user.team, user);
                        return;
                    }
                }
            }
        }

        private bool GameItemOverlaps(RoomItem gameItem)
        {
            Point gameItemCoord = gameItem.Coordinate;
            foreach (RoomItem item in GetFootballItemsForAllTeams())
            {
                foreach (ThreeDCoord tile in item.GetAffectedTiles.Values)
                {
                    if (tile.X == gameItemCoord.X && tile.Y == gameItemCoord.Y)
                        return true;
                }
            }

            return false;
        }

        private List<RoomItem> GetFootballItemsForAllTeams()
        {
            List<RoomItem> items = new List<RoomItem>();

            foreach (RoomItem item in room.GetGameManager().GetItems(Team.red).Values)
            {
                items.Add(item);
            }

            foreach (RoomItem item in room.GetGameManager().GetItems(Team.green).Values)
            {
                items.Add(item);
            }

            foreach (RoomItem item in room.GetGameManager().GetItems(Team.blue).Values)
            {
                items.Add(item);
            }

            foreach (RoomItem item in room.GetGameManager().GetItems(Team.yellow).Values)
            {
                items.Add(item);
            }

            return items;
        }


        internal void MoveBall(RoomItem item, GameClient client, Point user, Point ball, int length)
        {
            int differenceX = user.X - ball.X;
            int differenceY = user.Y - ball.Y;

            if (differenceX <= 1 && differenceX >= -1 && differenceY <= 1 && differenceY >= -1)
            {
                int newX = ball.X;
                int newY = ball.Y;
                for (int i = 1; i < length; i++)
                {
                    newX = differenceX * -i;
                    newY = differenceY * -i;

                    newX = newX + item.GetX;
                    newY = newY + item.GetY;

                    if (!room.GetGameMap().itemCanBePlacedHere(newX, newY))
                    {
                        if (i == 1)
                            break;

                        i = i - 1;
                        newX = differenceX * -i;
                        newY = differenceY * -i;

                        newX = newX + item.GetX;
                        newY = newY + item.GetY;
                        break;
                    }
                }

                if (newX != ball.X || newY != ball.Y)
                    MoveBall(item, client, newX, newY);
            }
        }

        internal void Destroy()
        {
            Array.Clear(gates, 0, gates.Length);
            gates = null;
            room = null;
            balls.Destroy();
            balls = null;
        }
    }
}
