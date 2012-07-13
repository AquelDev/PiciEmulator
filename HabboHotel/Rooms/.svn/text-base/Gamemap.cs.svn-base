using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Butterfly.Collections;
using Butterfly.Core;
using Butterfly.HabboHotel.Items;
using Butterfly.HabboHotel.Pathfinding;
using Butterfly.HabboHotel.Rooms.Games;
using Butterfly.HabboHotel.Rooms.Wired;

namespace Butterfly.HabboHotel.Rooms
{
    class Gamemap
    {
        private Room room;

        private RoomModel mStaticModel;
        private DynamicRoomModel mDynamicModel;
        //private Hashtable mCoordinatedItems;
        private Hashtable mCoordinatedItems;
        //private Dictionary<Point, List<RoomItem>> mCoordinatedItems;

        private List<RoomLinkInformation> roomLinkInformation;

        private byte[,] mGameMap;
        private byte[,] mUserItemEffect;
        //0 = none
        //1 = poo(l)
        //2 = normal skates
        //3 = ice skates
        private double[,] mItemHeightMap;
        internal bool DiagonalEnabled;
        internal bool gotPublicPool;
        private Hashtable userMap;

        internal DynamicRoomModel Model
        {
            get
            {
                return mDynamicModel;
            }
        }

        internal RoomModel StaticModel
        {
            get
            {
                return mStaticModel;
            }
        }

        internal byte[,] EffectMap
        {
            get
            {
                return mUserItemEffect;
            }
        }

        internal Hashtable CoordinatedItems
        {
            get
            {

                return mCoordinatedItems;
            }
        }

        internal byte[,] GameMap
        {
            get
            {
                return mGameMap;
            }
        }

        internal double[,] ItemHeightMap
        {
            get
            {
                return mItemHeightMap;
            }
        }

        public Gamemap(Room room)
        {
            this.room = room;
            this.DiagonalEnabled = true;
            this.mStaticModel = ButterflyEnvironment.GetGame().GetRoomManager().GetModel(room.ModelName, room.RoomId);
            if (mStaticModel == null)
                throw new Exception("No modeldata found for roomID " + room.RoomId);
            this.mDynamicModel = new DynamicRoomModel(this.mStaticModel);

            this.mCoordinatedItems = new Hashtable();


            this.gotPublicPool = room.RoomData.Model.gotPublicPool;
            this.mGameMap = new byte[Model.MapSizeX, Model.MapSizeY];
            this.mItemHeightMap = new double[Model.MapSizeX, Model.MapSizeY];
            userMap = new Hashtable();

            if (room.IsPublic)
                this.roomLinkInformation = ButterflyEnvironment.GetGame().GetRoomManager().getLinkedRoomData(room.RoomId);
        }

        internal void AddUserToMap(RoomUser user, Point coord)
        {
            if (userMap.ContainsKey(coord))
            {
                ((List<RoomUser>)userMap[coord]).Add(user);
            }
            else
            {
                List<RoomUser> users = new List<RoomUser>();
                users.Add(user);
                userMap.Add(coord, users);
            }
        }

        internal void TeleportToItem(RoomUser user, RoomItem item)
        {
            GameMap[user.X, user.Y] = user.SqState;
            UpdateUserMovement(new Point(user.Coordinate.X, user.Coordinate.Y), new Point(item.Coordinate.X, item.Coordinate.Y), user);
            user.X = item.GetX;
            user.Y = item.GetY;
            user.Z = item.GetZ;
            if (user.isFlying)
                user.Z += 4 + 0.5 * Math.Sin(0.7 * user.flyk);

            user.SqState = GameMap[item.GetX, item.GetY];
            GameMap[user.X, user.Y] = 1;
            user.RotBody = item.Rot;
            user.RotHead = item.Rot;

            user.GoalX = user.X;
            user.GoalY = user.Y;
            user.SetStep = false;
            user.IsWalking = false;
            user.UpdateNeeded = true;
        }

        internal void UpdateUserMovement(Point oldCoord, Point newCoord, RoomUser user)
        {
            RemoveUserFromMap(user, oldCoord);
            AddUserToMap(user, newCoord);
        }

        internal void RemoveUserFromMap(RoomUser user, Point coord)
        {
            if (userMap.ContainsKey(coord))
                ((List<RoomUser>)userMap[coord]).Remove(user);
        }

        internal bool MapGotUser(Point coord)
        {
            return (GetRoomUsers(coord).Count > 0);
        }

        internal List<RoomUser> GetRoomUsers(Point coord)
        {
            if (userMap.ContainsKey(coord))
                return (List<RoomUser>)userMap[coord];
            else
                return new List<RoomUser>();
        }

        internal void HandleRoomLinks(RoomUser user)
        {
            foreach (RoomLinkInformation square in roomLinkInformation.Where(p => p.fromX == user.SetX && p.fromY == user.SetY))
            {
                RoomUser userOnSquare = room.GetRoomUserManager().GetUserForSquare(square.fromX, square.fromY);
                if (userOnSquare != null)
                {
                    //Session.GetMessageHandler().PrepareRoomForUser(roomID, "");
                    if (userOnSquare.GetClient() != null && userOnSquare.GetClient().GetMessageHandler() != null)
                    {
                        userOnSquare.GetClient().SetDoorPos = true;
                        userOnSquare.GetClient().newDoorPos = new Point(square.toX, square.toY);
                        userOnSquare.GetClient().GetMessageHandler().PrepareRoomForUser(square.toRoomID, "");
                    }
                }
            }
        }

        internal Point getRandomWalkableSquare()
        {
            List<Point> walkableSquares = new List<Point>();
            for (int y = 0; y < mGameMap.GetUpperBound(1) - 1; y++)
            {
                for (int x = 0; x < mGameMap.GetUpperBound(0) - 1; x++)
                {
                    if (mStaticModel.DoorX != x && mStaticModel.DoorY != y && mGameMap[x, y] == 1)
                        walkableSquares.Add(new Point(x, y));
                }
            }

            int RandomNumber = ButterflyEnvironment.GetRandomNumber(0, walkableSquares.Count);
            int i = 0;

            foreach (Point coord in walkableSquares)
            {
                if (i == RandomNumber)
                    return coord;
                i++;
            }

            return new Point(0, 0);
        }
        internal string GenerateMapDump()
        {
            StringBuilder Dump = new StringBuilder();
            Dump.AppendLine("Game map:");

            for (int y = 0; y < Model.MapSizeY; y++)
            {
                StringBuilder OneLine = new StringBuilder();
                for (int x = 0; x < Model.MapSizeX; x++)
                {
                    OneLine.Append(mGameMap[x, y].ToString());
                }
                Dump.AppendLine(OneLine.ToString());
            }
            Dump.AppendLine();

            Dump.AppendLine("Item height map:");

            for (int y = 0; y < Model.MapSizeY; y++)
            {
                StringBuilder OneLine = new StringBuilder();
                for (int x = 0; x < Model.MapSizeX; x++)
                {
                    OneLine.Append("[" + mItemHeightMap[x, y].ToString() + "]");
                }
                Dump.AppendLine(OneLine.ToString());
            }

            Dump.AppendLine();

            Dump.AppendLine("Static data:");

            for (int y = 0; y < Model.MapSizeY; y++)
            {
                StringBuilder OneLine = new StringBuilder();
                for (int x = 0; x < Model.MapSizeX; x++)
                {
                    OneLine.Append("[" + Model.SqState[x, y].ToString() + "]");
                }
                Dump.AppendLine(OneLine.ToString());
            }

            Dump.AppendLine();

            Dump.AppendLine("Static data height:");

            for (int y = 0; y < Model.MapSizeY; y++)
            {
                StringBuilder OneLine = new StringBuilder();
                for (int x = 0; x < Model.MapSizeX; x++)
                {
                    OneLine.Append("[" + Model.SqFloorHeight[x, y].ToString() + "]");
                }
                Dump.AppendLine(OneLine.ToString());
            }

            Dump.AppendLine();

            Dump.AppendLine("Pool map:");

            for (int y = 0; y < Model.MapSizeY; y++)
            {
                StringBuilder OneLine = new StringBuilder();
                for (int x = 0; x < Model.MapSizeX; x++)
                {
                    OneLine.Append("[" + mUserItemEffect[x, y].ToString() + "]");
                }
                Dump.AppendLine(OneLine.ToString());
            }
            Dump.AppendLine();
            return Dump.ToString();
        }

        internal void AddToMap(RoomItem item)
        {
            AddItemToMap(item);
        }

        private void SetDefaultValue(int x, int y)
        {
            mGameMap[x, y] = 0;
            mUserItemEffect[x, y] = 0;
            mItemHeightMap[x, y] = 0.0;

            if (x == Model.DoorX && y == Model.DoorY)
            {
                mGameMap[x, y] = 3;
            }
            else if (Model.SqState[x, y] == SquareState.OPEN)
            {
                mGameMap[x, y] = 1;
            }
            else if (Model.SqState[x, y] == SquareState.SEAT)
            {
                mGameMap[x, y] = 2;
            }
        }

        internal void updateMapForItem(RoomItem item)
        {
            RemoveFromMap(item);
            AddToMap(item);
        }

        internal void GenerateMaps(bool checkLines = true)
        {

            int MaxX = 0;
            int MaxY = 0;
            mCoordinatedItems = new Hashtable();

            if (checkLines)
            {
                RoomItem[] items = room.GetRoomItemHandler().mFloorItems.Values.ToArray();
                foreach (RoomItem item in items)
                {
                    if (item.GetX > Model.MapSizeX && item.GetX > MaxX)
                        MaxX = item.GetX;
                    if (item.GetY > Model.MapSizeY && item.GetY > MaxY)
                        MaxY = item.GetY;
                }

                Array.Clear(items, 0, items.Length);
                items = null;
            }

            #region Dynamic game map handling
            if (MaxY > (Model.MapSizeY - 1) || MaxX > (Model.MapSizeX - 1))
            {
                if (MaxX < Model.MapSizeX)
                    MaxX = Model.MapSizeX;
                if (MaxY < Model.MapSizeY)
                    MaxY = Model.MapSizeY;

                Model.SetMapsize(MaxX + 7, MaxY + 7);
                GenerateMaps(false);
                return;
            }

            if (MaxX != StaticModel.MapSizeX || MaxY != StaticModel.MapSizeY)
            {
                this.mUserItemEffect = new byte[Model.MapSizeX, Model.MapSizeY];
                this.mGameMap = new byte[Model.MapSizeX, Model.MapSizeY];


                this.mItemHeightMap = new double[Model.MapSizeX, Model.MapSizeY];
                //if (modelRemap)
                //    Model.Generate(); //Clears model

                for (int line = 0; line < Model.MapSizeY; line++)
                {
                    for (int chr = 0; chr < Model.MapSizeX; chr++)
                    {
                        mGameMap[chr, line] = 0;
                        mUserItemEffect[chr, line] = 0;

                        if (chr == Model.DoorX && line == Model.DoorY)
                        {
                            mGameMap[chr, line] = 3;
                        }
                        else if (Model.SqState[chr, line] == SquareState.OPEN)
                        {
                            mGameMap[chr, line] = 1;
                        }
                        else if (Model.SqState[chr, line] == SquareState.SEAT)
                        {
                            mGameMap[chr, line] = 2;
                        }
                        else if (Model.SqState[chr, line] == SquareState.POOL)
                        {
                            mUserItemEffect[chr, line] = 6;
                        }
                    }
                }

                if (gotPublicPool)
                {
                    for (int y = 0; y < StaticModel.MapSizeY; y++)
                    {
                        for (int x = 0; x < StaticModel.MapSizeX; x++)
                        {
                            if (StaticModel.mRoomModelfx[x, y] != 0)
                            {
                                mUserItemEffect[x, y] = StaticModel.mRoomModelfx[x, y];
                            }
                        }
                    }
                }
            }
            #endregion

            #region Static game map handling
            else
            {
                //mGameMap
                //mUserItemEffect
                this.mUserItemEffect = new byte[Model.MapSizeX, Model.MapSizeY];
                this.mGameMap = new byte[Model.MapSizeX, Model.MapSizeY];


                this.mItemHeightMap = new double[Model.MapSizeX, Model.MapSizeY];
                //if (modelRemap)
                //    Model.Generate(); //Clears model

                for (int line = 0; line < Model.MapSizeY; line++)
                {
                    for (int chr = 0; chr < Model.MapSizeX; chr++)
                    {
                        mGameMap[chr, line] = 0;
                        mUserItemEffect[chr, line] = 0;

                        if (chr == Model.DoorX && line == Model.DoorY)
                        {
                            mGameMap[chr, line] = 3;
                        }
                        else if (Model.SqState[chr, line] == SquareState.OPEN)
                        {
                            mGameMap[chr, line] = 1;
                        }
                        else if (Model.SqState[chr, line] == SquareState.SEAT)
                        {
                            mGameMap[chr, line] = 2;
                        }
                        else if (Model.SqState[chr, line] == SquareState.POOL)
                        {
                            mUserItemEffect[chr, line] = 6;
                        }
                    }
                }

                if (gotPublicPool)
                {
                    for (int y = 0; y < StaticModel.MapSizeY; y++)
                    {
                        for (int x = 0; x < StaticModel.MapSizeX; x++)
                        {
                            if (StaticModel.mRoomModelfx[x, y] != 0)
                            {
                                mUserItemEffect[x, y] = StaticModel.mRoomModelfx[x, y];
                            }
                        }
                    }
                }
            }
            #endregion

            RoomItem[] tmpItems = room.GetRoomItemHandler().mFloorItems.Values.ToArray();
            foreach (RoomItem Item in tmpItems)
            {
                if (!AddItemToMap(Item))
                    break;
            }
            Array.Clear(tmpItems, 0, tmpItems.Length);
            tmpItems = null;

            if (!room.AllowWalkthrough)
            {
                foreach (RoomUser user in room.GetRoomUserManager().UserList.Values)
                {
                    user.SqState = mGameMap[user.X, user.Y];
                    mGameMap[user.X, user.Y] = 0;
                }
            }
            mGameMap[Model.DoorX, Model.DoorY] = 3;

        }

        private bool ConstructMapForItem(RoomItem Item, Point Coord)
        {
            try
            {
                if (Coord.X > (Model.MapSizeX - 1))
                {
                    Model.AddX();
                    GenerateMaps();
                    return false;
                }

                if (Coord.Y > (Model.MapSizeY - 1))
                {
                    Model.AddY();
                    GenerateMaps();
                    return false;
                }

                if (Model.SqState[Coord.X, Coord.Y] == SquareState.BLOCKED)
                {
                    Model.OpenSquare(Coord.X, Coord.Y, Item.GetZ);
                    Model.SetUpdateState();
                }
                if (mItemHeightMap[Coord.X, Coord.Y] <= Item.TotalHeight)
                {
                    mItemHeightMap[Coord.X, Coord.Y] = Item.TotalHeight - mDynamicModel.SqFloorHeight[Item.GetX, Item.GetY];
                    mUserItemEffect[Coord.X, Coord.Y] = 0;


                    switch (Item.GetBaseItem().InteractionType)
                    {
                        case InteractionType.pool:
                            mUserItemEffect[Coord.X, Coord.Y] = 1;
                            break;
                        case InteractionType.normslaskates:
                            mUserItemEffect[Coord.X, Coord.Y] = 2;
                            break;
                        case InteractionType.iceskates:
                            mUserItemEffect[Coord.X, Coord.Y] = 3;
                            break;
                        case InteractionType.lowpool:
                            mUserItemEffect[Coord.X, Coord.Y] = 4;
                            break;
                        case InteractionType.haloweenpool:
                            mUserItemEffect[Coord.X, Coord.Y] = 5;
                            break;
                    }
                    //if (Item.GetBaseItem().InteractionType == Butterfly.HabboHotel.Items.InteractionType.pool)
                    //    mUserItemEffect[Coord.X, Coord.Y] = 1;
                    //else if (Item.GetBaseItem().InteractionType == Butterfly.HabboHotel.Items.InteractionType.normslaskates)
                    //    mUserItemEffect[Coord.X, Coord.Y] = 2;
                    //else if (Item.GetBaseItem().InteractionType == Butterfly.HabboHotel.Items.InteractionType.iceskates)
                    //    mUserItemEffect[Coord.X, Coord.Y] = 3;
                    //else if (Item.GetBaseItem().InteractionType == Butterfly.HabboHotel.Items.InteractionType.lowpool)
                    //    mUserItemEffect[Coord.X, Coord.Y] = 4;



                    //SwimHalloween
                    if (Item.GetBaseItem().Walkable)// If this item is walkable and on the floor, allow users to walk here.
                    {
                        if (mGameMap[Coord.X, Coord.Y] != 3)
                            mGameMap[Coord.X, Coord.Y] = 1;
                    }
                    else if (Item.GetZ <= (Model.SqFloorHeight[Item.GetX, Item.GetY] + 0.1) && Item.GetBaseItem().InteractionType == Butterfly.HabboHotel.Items.InteractionType.gate && Item.ExtraData == "1") // If this item is a gate, open, and on the floor, allow users to walk here.
                    {
                        if (mGameMap[Coord.X, Coord.Y] != 3)
                            mGameMap[Coord.X, Coord.Y] = 1;
                    }
                    else if (Item.GetBaseItem().IsSeat || Item.GetBaseItem().InteractionType == Butterfly.HabboHotel.Items.InteractionType.bed)
                    {
                        mGameMap[Coord.X, Coord.Y] = 3;
                    }
                    else // Finally, if it's none of those, block the square.
                    {
                        if (mGameMap[Coord.X, Coord.Y] != 3)
                            mGameMap[Coord.X, Coord.Y] = 0;
                    }
                }

                // Set bad maps
                if (Item.GetBaseItem().InteractionType == Butterfly.HabboHotel.Items.InteractionType.bed)
                    mGameMap[Coord.X, Coord.Y] = 3;
            }
            catch (Exception e)
            {
                Logging.LogException("Error during map generation for room " + room.RoomId + ". Exception: " + e.ToString());
                //Console.WriteLine("Mapsize X: " + mItemHeightMap.GetUpperBound(0) + "Mapsize Y: " + mItemHeightMap.GetUpperBound(1));
                //Console.WriteLine("Original mapsize: " + Model.MapSizeX + "Mapsize Y: " + Model.MapSizeY);
                //Console.WriteLine("Coord X: " + Coord.X + " Y: " + Coord.Y);
            }
            return true;
        }

        internal void AddCoordinatedItem(RoomItem item, Point coord)
        {
            //Coord CoordForItem = new Coord(Item.GetX, Item.GetY);
            List<RoomItem> Items = new List<RoomItem>(); //mCoordinatedItems[CoordForItem];

            if (!mCoordinatedItems.ContainsKey(coord))
            {
                Items = new List<RoomItem>();
                Items.Add(item);
                mCoordinatedItems.Add(coord, Items);
            }
            else
            {
                Items = ((List<RoomItem>)mCoordinatedItems[coord]);

                if (!Items.Contains(item))
                {
                    Items.Add(item);
                    mCoordinatedItems[coord] = Items;
                }
            }
        }

        internal List<RoomItem> GetCoordinatedItems(Point coord)
        {
            Point point = new Point(coord.X, coord.Y);
            if (mCoordinatedItems.ContainsKey(point))
            {
                return (List<RoomItem>)mCoordinatedItems[point];
            }

            return new List<RoomItem>();
        }

        internal bool RemoveCoordinatedItem(RoomItem item, Point coord)
        {
            Point point = new Point(coord.X, coord.Y);
            if (mCoordinatedItems.ContainsKey(point))
            {
                ((List<RoomItem>)mCoordinatedItems[point]).Remove(item);
                return true;
            }
            return false;
        }

        private void AddSpecialItems(RoomItem item)
        {

            switch (item.GetBaseItem().InteractionType)
            {
                case InteractionType.fbgate:
                    //IsTrans = true;
                    room.GetSoccer().RegisterGate(item);


                    string[] splittedExtraData = item.ExtraData.Split(':');

                    if (string.IsNullOrEmpty(item.ExtraData) || splittedExtraData.Length <= 1)
                    {
                        item.Gender = "M";
                        switch (item.team)
                        {
                            case Team.yellow:
                                item.Figure = "lg-275-93.hr-115-61.hd-207-14.ch-265-93.sh-305-62";
                                break;
                            case Team.red:
                                item.Figure = "lg-275-96.hr-115-61.hd-180-3.ch-265-96.sh-305-62";
                                break;
                            case Team.green:
                                item.Figure = "lg-275-102.hr-115-61.hd-180-3.ch-265-102.sh-305-62";
                                break;
                            case Team.blue:
                                item.Figure = "lg-275-108.hr-115-61.hd-180-3.ch-265-108.sh-305-62";
                                break;
                        }
                    }
                    else
                    {
                        item.Gender = splittedExtraData[0];
                        item.Figure = splittedExtraData[1];
                    }
                    break;

                case InteractionType.banzaifloor:
                    {
                        room.GetBanzai().AddTile(item, item.Id);
                        break;
                    }

                case InteractionType.banzaipyramid:
                    {
                        room.GetGameItemHandler().AddPyramid(item, item.Id);
                        break;
                    }

                case InteractionType.banzaitele:
                    {
                        room.GetGameItemHandler().AddTeleport(item, item.Id);
                        item.ExtraData = "";
                        break;
                    }
                case InteractionType.banzaipuck:
                    {
                        room.GetBanzai().AddPuck(item);
                        break;
                    }

                case InteractionType.football:
                    {
                        room.GetSoccer().AddBall(item);
                        break;
                    }
                case InteractionType.freezetileblock:
                    {
                        room.GetFreeze().AddFreezeBlock(item);
                        break;
                    }
                case InteractionType.freezetile:
                    {
                        room.GetFreeze().AddFreezeTile(item);
                        break;
                    }
                case InteractionType.freezeexit:
                    {
                        RoomItem _item = room.GetFreeze().ExitTeleport;
                        if (_item == null)
                            break;
                        if (item.Id == _item.Id)
                            room.GetFreeze().ExitTeleport = null;
                        break;
                    }
            }
        }

        private void RemoveSpecialItem(RoomItem item)
        {
            switch (item.GetBaseItem().InteractionType)
            {
                case InteractionType.fbgate:
                    room.GetSoccer().UnRegisterGate(item);
                    break;
                case InteractionType.banzaifloor:
                    room.GetBanzai().RemoveTile(item.Id);
                    break;
                case InteractionType.banzaipuck:
                    room.GetBanzai().RemovePuck(item.Id);
                    break;
                case InteractionType.banzaipyramid:
                    room.GetGameItemHandler().RemovePyramid(item.Id);
                    break;
                case InteractionType.banzaitele:
                    room.GetGameItemHandler().RemoveTeleport(item.Id);
                    break;
                case InteractionType.football:
                    room.GetSoccer().RemoveBall(item.Id);
                    break;
                case InteractionType.freezetile:
                    room.GetFreeze().RemoveFreezeTile(item.Id);
                    break;
                case InteractionType.freezetileblock:
                    room.GetFreeze().RemoveFreezeBlock(item.Id);
                    break;
            }
        }
        internal bool RemoveFromMap(RoomItem item, bool handleGameItem)
        {
            if (handleGameItem)
                RemoveSpecialItem(item);

            if (room.GotSoccer())
                room.GetSoccer().onGateRemove(item);

            bool isRemoved = false;
            foreach (Point coord in item.GetCoords)
            {
                if (RemoveCoordinatedItem(item, coord))
                    isRemoved = true;
            }

            Hashtable items = new Hashtable();
            foreach (Point tile in item.GetCoords)
            {
                Point point = new Point(tile.X, tile.Y);
                if (mCoordinatedItems.ContainsKey(point))
                {
                    List<RoomItem> __items = (List<RoomItem>)mCoordinatedItems[point];
                    if (!items.ContainsKey(tile))
                        items.Add(tile, __items);
                }
                SetDefaultValue(tile.X, tile.Y);
            }

            foreach (Point coord in items.Keys)
            {
                if (!items.ContainsKey(coord))
                    continue;

                List<RoomItem> _items = (List<RoomItem>)items[coord];

                foreach (RoomItem _item in _items)
                {
                    ConstructMapForItem(_item, coord);
                }
            }

            items.Clear();
            items = null;
            return isRemoved;
        }

        internal bool RemoveFromMap(RoomItem item)
        {
            if (room.GotWired() && WiredUtillity.TypeIsWired(item.GetBaseItem().InteractionType))
                room.GetWiredHandler().RemoveFurniture(item);

            return RemoveFromMap(item, true);
        }

        internal bool AddItemToMap(RoomItem Item, bool handleGameItem)
        {
            if (handleGameItem)
            {
                if (room.GotWired() && WiredUtillity.TypeIsWired(Item.GetBaseItem().InteractionType))
                    room.GetWiredHandler().AddFurniture(Item);

                AddSpecialItems(Item);

                switch (Item.GetBaseItem().InteractionType)
                {
                    case InteractionType.footballgoalred:
                    case InteractionType.footballcounterred:
                    case InteractionType.banzaiscorered:
                    case InteractionType.freezeredcounter:
                    case InteractionType.freezeredgate:
                        {
                            room.GetGameManager().AddFurnitureToTeam(Item, Games.Team.red);
                            break;
                        }
                    case InteractionType.footballgoalgreen:
                    case InteractionType.footballcountergreen:
                    case InteractionType.banzaiscoregreen:
                    case InteractionType.freezegreencounter:
                    case InteractionType.freezegreengate:
                        {
                            room.GetGameManager().AddFurnitureToTeam(Item, Games.Team.green);
                            break;
                        }
                    case InteractionType.footballgoalblue:
                    case InteractionType.footballcounterblue:
                    case InteractionType.banzaiscoreblue:
                    case InteractionType.freezebluecounter:
                    case InteractionType.freezebluegate:
                        {
                            room.GetGameManager().AddFurnitureToTeam(Item, Games.Team.blue);
                            break;
                        }
                    case InteractionType.footballgoalyellow:
                    case InteractionType.footballcounteryellow:
                    case InteractionType.banzaiscoreyellow:
                    case InteractionType.freezeyellowcounter:
                    case InteractionType.freezeyellowgate:
                        {
                            room.GetGameManager().AddFurnitureToTeam(Item, Games.Team.yellow);
                            break;
                        }
                    case InteractionType.freezeexit:
                        {
                            room.GetFreeze().ExitTeleport = Item;
                            break;
                        }
                    case InteractionType.roller:
                        {
                            if (!room.GetRoomItemHandler().mRollers.ContainsKey(Item.Id))
                                room.GetRoomItemHandler().mRollers.Add(Item.Id, Item);
                            break;
                        }
                }
            }

            if (Item.GetBaseItem().Type != 's')
                return true;

            foreach (Point coord in Item.GetCoords)
            {
                Point point = new Point(coord.X, coord.Y);
                AddCoordinatedItem(Item, point);
            }

            if (Item.GetX > (Model.MapSizeX - 1))
            {
                Model.AddX();
                GenerateMaps();
                return false;
            }

            if (Item.GetY > (Model.MapSizeY - 1))
            {
                Model.AddY();
                GenerateMaps();
                return false;
            }


            foreach (Point coord in Item.GetCoords)
                if (!ConstructMapForItem(Item, coord))
                    return false;

            return true;
        }



        internal bool CanWalk(int X, int Y, bool Override)
        {
            if (room.AllowWalkthrough)
                return true;
            if (Override)
                return true;
            if (room.GetRoomUserManager().GetUserForSquare(X, Y) != null)
                return false;

            return true;
        }
        internal bool AddItemToMap(RoomItem Item)
        {
            return AddItemToMap(Item, true);
        }

        internal byte GetFloorStatus(Point coord)
        {
            if (coord.X > mGameMap.GetUpperBound(0) || coord.Y > mGameMap.GetUpperBound(1))
                return 1;
            return mGameMap[coord.X, coord.Y];
        }

        internal double GetHeightForSquareFromData(Point coord)
        {
            if (coord.X > mDynamicModel.SqFloorHeight.GetUpperBound(0) || coord.Y > mDynamicModel.SqFloorHeight.GetUpperBound(1))
                return 1;
            return mDynamicModel.SqFloorHeight[coord.X, coord.Y];
        }

        internal bool CanRollItemHere(int x, int y)
        {
            if (!ValidTile(x, y))
                return false;
            if (Model.SqState[x, y] == SquareState.BLOCKED)
                return false;

            return true;
        }

        internal bool SquareIsOpen(int x, int y, bool pOverride)
        {
            if ((mDynamicModel.MapSizeX - 1) < x || (mDynamicModel.MapSizeY - 1) < y)
                return false;
            return CanWalk(mGameMap[x, y], pOverride);
        }

        internal static bool CanWalk(byte pState, bool pOverride)
        {

            if (!pOverride)
            {
                if (pState == 3)
                    return true;
                if (pState == 1)
                    return true;

                return false;
            }
            return true;
        }

        internal bool validTile(int x, int y)
        {
            return mDynamicModel.SqState[x, y] == SquareState.OPEN;
        }

        internal bool itemCanBePlacedHere(int x, int y)
        {
            if (mDynamicModel.MapSizeX - 1 < x || mDynamicModel.MapSizeY - 1 < y || (x == mDynamicModel.DoorX && y == mDynamicModel.DoorY))
                return false;

            return mGameMap[x, y] == 1;
        }

        internal double SqAbsoluteHeight(int X, int Y)
        {
            Point point = new Point(X, Y);
            if (mCoordinatedItems.ContainsKey(point))
            {
                List<RoomItem> items = (List<RoomItem>)mCoordinatedItems[point];
                return SqAbsoluteHeight(X, Y, items);
            }

            return mDynamicModel.SqFloorHeight[X, Y];
        }

        internal double SqAbsoluteHeight(int X, int Y, List<RoomItem> ItemsOnSquare)
        {
            try
            {
                //List<RoomItem> ItemsOnSquare = GetFurniObjects(X, Y);
                double HighestStack = 0;

                bool deduct = false;
                double deductable = 0.0;


                foreach (RoomItem Item in ItemsOnSquare)
                {
                    if (Item.TotalHeight > HighestStack)
                    {
                        if (Item.GetBaseItem().IsSeat || Item.GetBaseItem().InteractionType == InteractionType.bed)
                        {
                            deduct = true;
                            deductable = Item.GetBaseItem().Height;
                        }
                        else
                        {
                            deduct = false;
                        }

                        HighestStack = Item.TotalHeight;
                    }
                }

                double floorHeight = Model.SqFloorHeight[X, Y];
                double stackHeight = HighestStack - Model.SqFloorHeight[X, Y];

                if (deduct)
                    stackHeight -= deductable;

                if (stackHeight < 0)
                    stackHeight = 0;

                return (floorHeight + stackHeight);
            }
            catch (Exception e)
            {
                Logging.HandleException(e, "Room.SqAbsoluteHeight");
                return 0;
            }
        }


        internal bool ValidTile(int X, int Y)
        {
            if (X < 0 || Y < 0 || X >= Model.MapSizeX || Y >= Model.MapSizeY)
            {
                return false;
            }

            return true;
        }

        internal static Dictionary<int, ThreeDCoord> GetAffectedTiles(int Length, int Width, int PosX, int PosY, int Rotation)
        {
            int x = 0;

            Dictionary<int, ThreeDCoord> PointList = new Dictionary<int, ThreeDCoord>();

            if (Length > 1)
            {
                if (Rotation == 0 || Rotation == 4)
                {
                    for (int i = 1; i < Length; i++)
                    {
                        PointList.Add(x++, new ThreeDCoord(PosX, PosY + i, i));

                        for (int j = 1; j < Width; j++)
                        {
                            PointList.Add(x++, new ThreeDCoord(PosX + j, PosY + i, (i < j) ? j : i));
                        }
                    }
                }
                else if (Rotation == 2 || Rotation == 6)
                {
                    for (int i = 1; i < Length; i++)
                    {
                        PointList.Add(x++, new ThreeDCoord(PosX + i, PosY, i));

                        for (int j = 1; j < Width; j++)
                        {
                            PointList.Add(x++, new ThreeDCoord(PosX + i, PosY + j, (i < j) ? j : i));
                        }
                    }
                }
            }

            if (Width > 1)
            {
                if (Rotation == 0 || Rotation == 4)
                {
                    for (int i = 1; i < Width; i++)
                    {
                        PointList.Add(x++, new ThreeDCoord(PosX + i, PosY, i));

                        for (int j = 1; j < Length; j++)
                        {
                            PointList.Add(x++, new ThreeDCoord(PosX + i, PosY + j, (i < j) ? j : i));
                        }
                    }
                }
                else if (Rotation == 2 || Rotation == 6)
                {
                    for (int i = 1; i < Width; i++)
                    {
                        PointList.Add(x++, new ThreeDCoord(PosX, PosY + i, i));

                        for (int j = 1; j < Length; j++)
                        {
                            PointList.Add(x++, new ThreeDCoord(PosX + j, PosY + i, (i < j) ? j : i));
                        }
                    }
                }
            }

            return PointList;
        }

        internal List<RoomItem> GetRoomItemForSquare(int pX, int pY, double minZ)
        {
            List<RoomItem> itemsToReturn = new List<RoomItem>();


            Point coord = new Point(pX, pY);
            if (mCoordinatedItems.ContainsKey(coord))
            {
                List<RoomItem> itemsFromSquare = (List<RoomItem>)mCoordinatedItems[coord];

                foreach (RoomItem item in itemsFromSquare)
                    if (item.GetZ > minZ)
                        if (item.GetX == pX && item.GetY == pY)
                            itemsToReturn.Add(item);
            }

            return itemsToReturn;
        }

        internal List<RoomItem> GetRoomItemForSquare(int pX, int pY)
        {
            Point coord = new Point(pX, pY);
            //List<RoomItem> itemsFromSquare = new List<RoomItem>();
            List<RoomItem> itemsToReturn = new List<RoomItem>();

            if (mCoordinatedItems.ContainsKey(coord))
            {
                List<RoomItem> itemsFromSquare = (List<RoomItem>)mCoordinatedItems[coord];

                foreach (RoomItem item in itemsFromSquare)
                {
                    if (item.Coordinate.X == coord.X && item.Coordinate.Y == coord.Y)
                        itemsToReturn.Add(item);
                }
            }

            return itemsToReturn;
        }

        internal List<RoomItem> GetAllRoomItemForSquare(int pX, int pY)
        {
            Point coord = new Point(pX, pY);
            List<RoomItem> itemsToReturn = new List<RoomItem>();

            if (mCoordinatedItems.ContainsKey(coord))
            {
                List<RoomItem> itemsFromSquare = (List<RoomItem>)mCoordinatedItems[coord];

                foreach (RoomItem item in itemsFromSquare)
                {
                    if (!itemsToReturn.Contains(item))
                        itemsToReturn.Add(item);
                }
            }

            return itemsToReturn;
        }

        internal bool SquareHasUsers(int X, int Y)
        {
            return MapGotUser(new Point(X, Y));
            //return (GetUserForSquare(X, Y) != null);
            //return !SquareIsOpen(X, Y, false);
        }


        internal static bool TilesTouching(int X1, int Y1, int X2, int Y2)
        {
            if (!(Math.Abs(X1 - X2) > 1 || Math.Abs(Y1 - Y2) > 1)) return true;
            if (X1 == X2 && Y1 == Y2) return true;
            return false;
        }

        internal static int TileDistance(int X1, int Y1, int X2, int Y2)
        {
            return Math.Abs(X1 - X2) + Math.Abs(Y1 - Y2);
        }

        internal void Destroy()
        {
            userMap.Clear();
            mDynamicModel.Destroy();
            mCoordinatedItems.Clear();
            if (roomLinkInformation != null)
                roomLinkInformation.Clear();

            Array.Clear(mGameMap, 0, mGameMap.Length);
            Array.Clear(mUserItemEffect, 0, mUserItemEffect.Length);
            Array.Clear(mItemHeightMap, 0, mItemHeightMap.Length);

            userMap = null;
            mGameMap = null;
            mUserItemEffect = null;
            mItemHeightMap = null;
            mCoordinatedItems = null;
            roomLinkInformation = null;
            mDynamicModel = null;
            room = null;
            mStaticModel = null;
        }
    }
}
