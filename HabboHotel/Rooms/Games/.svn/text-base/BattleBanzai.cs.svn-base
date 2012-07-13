using System;
using System.Collections;
using System.Collections.Generic;
using Butterfly.Collections;
using Butterfly.HabboHotel.GameClients;
using Butterfly.HabboHotel.Items;
using Butterfly.HabboHotel.Pathfinding;
using Butterfly.Messages;
using enclosuretest;
using System.Drawing;

namespace Butterfly.HabboHotel.Rooms.Games
{
    class BattleBanzai
    {
        private Room room;
        internal Hashtable banzaiTiles;
        private bool banzaiStarted;
        private QueuedDictionary<uint, RoomItem> pucks;
        private byte[,] floorMap;
        private GameField field;
        public BattleBanzai(Room room)
        {
            this.room = room;
            this.banzaiTiles = new Hashtable();
            this.banzaiStarted = false;
            this.pucks = new QueuedDictionary<uint, RoomItem>();
        }

        internal void AddTile(RoomItem item, uint itemID)
        {
            if (!banzaiTiles.ContainsKey(itemID))
                banzaiTiles.Add(itemID, item);
        }

        internal void RemoveTile(uint itemID)
        {
            banzaiTiles.Remove(itemID);
        }

        internal void OnCycle()
        {
            pucks.OnCycle();
        }

        internal void AddPuck(RoomItem item)
        {
            if (!pucks.ContainsKey(item.Id))
            pucks.Add(item.Id, item);
        }

        internal void RemovePuck(uint itemID)
        {
            pucks.Remove(itemID);
        }

        internal void OnUserWalk(RoomUser User)
        {
            if (User == null)
                return;
            foreach (RoomItem item in pucks.Values)
            {
                int differenceX = User.X - item.GetX;
                int differenceY = User.Y - item.GetY;

                if (differenceX <= 1 && differenceX >= -1 && differenceY <= 1 && differenceY >= -1)
                {
                    int NewX = differenceX * -1;
                    int NewY = differenceY * -1;

                    NewX = NewX + item.GetX;
                    NewY = NewY + item.GetY;

                    if (item.interactingBallUser == User.userID && room.GetGameMap().ValidTile(NewX, NewY))
                    {
                        item.interactingBallUser = 0;

                        MovePuck(item, User.GetClient(), User.Coordinate, item.Coordinate, 6, User.team);
                    }
                    else if (room.GetGameMap().ValidTile(NewX, NewY))
                    {
                        MovePuck(item, User.GetClient(), NewX, NewY, User.team);
                    }
                }
            }

            if (banzaiStarted)
            {
                HandleBanzaiTiles(User.Coordinate, User.team, User);
            }
        }

        internal void BanzaiStart()
        {
            if (banzaiStarted)
                return;
            room.GetGameManager().StartGame();
            floorMap = new byte[room.GetGameMap().Model.MapSizeY, room.GetGameMap().Model.MapSizeX];
            field = new GameField(floorMap, true);
            for (int i = 1; i < 5; i++)
            {
                room.GetGameManager().Points[i] = 0;
            }

            foreach (RoomItem tile in banzaiTiles.Values)
            {
                tile.ExtraData = "1";
                tile.value = 0;
                tile.team = Team.none;
                tile.UpdateState();
            }

            room.GetRoomItemHandler().mFloorItems.QueueDelegate(new onCycleDoneDelegate(ResetTiles));
            banzaiStarted = true;
        }

        internal void ResetTiles()
        {
            foreach (RoomItem item in room.GetRoomItemHandler().mFloorItems.Values)
            {
                InteractionType type = item.GetBaseItem().InteractionType;

                switch (type)
                {
                    case InteractionType.banzaiscoreblue:
                    case InteractionType.banzaiscoregreen:
                    case InteractionType.banzaiscorered:
                    case InteractionType.banzaiscoreyellow:
                        {
                            item.ExtraData = "0";
                            item.UpdateState();
                            break;
                        }
                }
            }
        }

        internal void BanzaiEnd()
        {
            room.GetGameManager().StopGame();
            banzaiStarted = false;
            field.destroy();
            this.floorMap = null;
            Team winners = room.GetGameManager().getWinningTeam();

            if (winners != Team.none)
            {
                foreach (RoomItem tile in banzaiTiles.Values)
                {
                    if (tile.team == winners)
                    {
                        tile.interactionCountHelper = 0;
                        tile.UpdateNeeded = true;
                    }
                    else if (tile.team == Team.none)
                    {
                        tile.ExtraData = "0";
                        tile.UpdateState();
                    }
                }
            }
        }

        internal void MovePuck(RoomItem item, GameClient mover, int newX, int newY, Team team)
        {
            if (!room.GetGameMap().itemCanBePlacedHere(newX, newY))
                return;

            Point oldRoomCoord = item.Coordinate;

            Double NewZ = room.GetGameMap().Model.SqFloorHeight[newX, newY];

            //ServerMessage Message = new ServerMessage(95);
            //Message.AppendUInt(item.Id);
            //Message.AppendUInt(3508);
            //Message.AppendInt32(item.Coordinate.X);
            //Message.AppendInt32(item.Coordinate.Y);
            //Message.AppendInt32(newX);
            //Message.AppendInt32(newY);
            //Message.AppendUInt(4);
            //Message.AppendStringWithBreak(NewZ.ToString());
            //Message.AppendStringWithBreak("H11");
            //Message.AppendInt32(-1);
            //Message.AppendInt32(0);
            //SendMessage(Message);

            if (oldRoomCoord.X == newX && oldRoomCoord.Y == newY)
                return;

            item.ExtraData = ((int)team).ToString();
            item.UpdateNeeded = true;
            item.UpdateState();



            ServerMessage mMessage = new ServerMessage();
            mMessage.Init(230); // Cf
            mMessage.AppendInt32(item.Coordinate.X);
            mMessage.AppendInt32(item.Coordinate.Y);
            mMessage.AppendInt32(newX);
            mMessage.AppendInt32(newY);
            mMessage.AppendInt32(1);
            mMessage.AppendUInt(item.Id);
            mMessage.AppendStringWithBreak(TextHandling.GetString(item.GetZ));
            mMessage.AppendStringWithBreak(TextHandling.GetString(NewZ));
            mMessage.AppendUInt(1);
            room.SendMessage(mMessage);




            //HandleBanzaiTiles(new Coord(item.GetX, item.GetY), team);
            room.GetRoomItemHandler().SetFloorItem(mover, item, newX, newY, item.Rot, false, false, false, false);

            if (mover == null || mover.GetHabbo() == null)
                return;

            RoomUser user = mover.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(mover.GetHabbo().Id);
            if (banzaiStarted)
            {
                HandleBanzaiTiles(new Point(newX, newY), team, user);
            }
        }

        internal void MovePuck(RoomItem item, GameClient client, Point user, Point ball, int length, Team team)
        {
            int differenceX = user.X - ball.X;
            int differenceY = user.Y - ball.Y;

            if (differenceX <= 1 && differenceX >= -1 && differenceY <= 1 && differenceY >= -1)
            {
                List<Point> affectedTiles = new List<Point>();
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

                        if (i != length)
                            affectedTiles.Add(new Point(newX, newY));
                        i = i - 1;
                        newX = differenceX * -i;
                        newY = differenceY * -i;

                        newX = newX + item.GetX;
                        newY = newY + item.GetY;
                        break;
                    }
                    else
                    {
                        if (i != length)
                            affectedTiles.Add(new Point(newX, newY));
                    }
                }
                if (client == null || client.GetHabbo() == null)
                    return;

                RoomUser _user = client.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(client.GetHabbo().Id);

                foreach (Point coord in affectedTiles)
                {
                    HandleBanzaiTiles(coord, team, _user);
                }

                if (newX != ball.X || newY != ball.Y)
                    MovePuck(item, client, newX, newY, team);
            }
        }

        private void SetTile(RoomItem item, Team team, RoomUser user)
        {
            if (item.team == team)
            {
                if (item.value < 3)
                {
                    item.value++;
                    if (item.value == 3)
                    {
                        room.GetGameManager().AddPointToTeam(item.team, user);
                        field.updateLocation(item.GetX, item.GetY, (byte)team);
                        List<PointField> gfield = field.doUpdate();
                        Team t;
                        foreach (PointField gameField in gfield)
                        {
                            t = (Team)gameField.forValue;
                            foreach (Point p in gameField.getPoints())
                            {
                                HandleMaxBanzaiTiles(new Point(p.X, p.Y), t, user);
                                floorMap[p.Y, p.X] = gameField.forValue;
                            }
                        }

                    }
                }
            }
            else
            {
                if (item.value < 3)
                {
                    item.team = team;
                    item.value = 1;
                }
            }


            int newColor = item.value + ((int)item.team * 3) - 1;
            item.ExtraData = newColor.ToString();
        }

        private void HandleBanzaiTiles(Point coord, Team team, RoomUser user)
        {
            if (team == Team.none)
                return;

            List<RoomItem> items = room.GetGameMap().GetCoordinatedItems(coord);
            foreach (RoomItem _item in banzaiTiles.Values)
            {
                if (_item.GetBaseItem().InteractionType != InteractionType.banzaifloor)
                    continue;

                if (_item.GetX != coord.X || _item.GetY != coord.Y)
                    continue;

                SetTile(_item, team, user);
                _item.UpdateState(false, true);
            }
        }

        private void HandleMaxBanzaiTiles(Point coord, Team team, RoomUser user)
        {
            if (team == Team.none)
                return;

            List<RoomItem> items = room.GetGameMap().GetCoordinatedItems(coord);
            foreach (RoomItem _item in banzaiTiles.Values)
            {
                if (_item.GetBaseItem().InteractionType != InteractionType.banzaifloor)
                    continue;

                if (_item.GetX != coord.X || _item.GetY != coord.Y)
                    continue;
                if (_item.value == 3)
                    return;

                SetMaxForTile(_item, team);

                room.GetGameManager().AddPointToTeam(team, user);

                _item.UpdateState(false, true);
            }
        }

        private static void SetMaxForTile(RoomItem item, Team team)
        {
            if (item.value < 3)
            {
                item.value = 3;
                item.team = team;
            }

            int newColor = item.value + ((int)item.team * 3) - 1;
            item.ExtraData = newColor.ToString();
        }

        internal bool isBanzaiActive { get { return banzaiStarted; } }

        internal void Destroy()
        {
            banzaiTiles.Clear();
            pucks.Clear();
            Array.Clear(floorMap, 0, floorMap.Length);
            field.destroy();

            room = null;
            banzaiTiles = null;
            pucks = null;
            floorMap = null;
            field = null;
        }
    }
}
