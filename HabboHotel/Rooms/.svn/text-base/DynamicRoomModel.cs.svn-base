using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Butterfly.HabboHotel.Rooms;
using Butterfly.Messages;
using Butterfly.Core;

namespace Butterfly.HabboHotel.Rooms
{
    class DynamicRoomModel
    {

        private RoomModel staticModel;
        internal int DoorX;
        internal int DoorY;
        internal double DoorZ;
        internal int DoorOrientation;

        internal string Heightmap;

        internal SquareState[,] SqState;
        internal short[,] SqFloorHeight;
        internal byte[,] SqSeatRot;

        internal int MapSizeX;
        internal int MapSizeY;

        internal bool ClubOnly;

        internal DynamicRoomModel(RoomModel pModel)
        {
            this.staticModel = pModel;
            this.DoorX = staticModel.DoorX;
            this.DoorY = staticModel.DoorY;
            this.DoorZ = staticModel.DoorZ;

            this.DoorOrientation = staticModel.DoorOrientation;
            this.Heightmap = staticModel.Heightmap;

            this.MapSizeX = staticModel.MapSizeX;
            this.MapSizeY = staticModel.MapSizeY;
            this.ClubOnly = staticModel.ClubOnly;

            Generate();

        }

        internal void Generate()
        {
            SqState = new SquareState[MapSizeX, MapSizeY];
            SqFloorHeight = new short[MapSizeX, MapSizeY];
            SqSeatRot = new byte[MapSizeX, MapSizeY];

            for (int y = 0; y < MapSizeY; y++)
            {
                for (int x = 0; x < MapSizeX; x++)
                {
                    if (x > (staticModel.MapSizeX - 1) || y > (staticModel.MapSizeY - 1))
                    {
                        SqState[x, y] = SquareState.BLOCKED;
                    }
                    else
                    {
                        SqState[x, y] = staticModel.SqState[x, y];
                        SqFloorHeight[x, y] = staticModel.SqFloorHeight[x, y];
                        SqSeatRot[x, y] = staticModel.SqSeatRot[x, y];

                    }
                }
            }

            RelativeSerialized = false;
            HeightmapSerialized = false;
        }

        internal void refreshArrays()
        {
            SquareState[,] newSqState = new SquareState[MapSizeX + 1, MapSizeY + 1];
            short[,] newSqFloorHeight = new short[MapSizeX + 1, MapSizeY + 1];
            byte[,] newSqSeatRot = new byte[MapSizeX + 1, MapSizeY + 1];

            for (int y = 0; y < MapSizeY; y++)
            {
                for (int x = 0; x < MapSizeX; x++)
                {
                    if (x > (staticModel.MapSizeX - 1) || y > (staticModel.MapSizeY - 1))
                    {
                        newSqState[x, y] = SquareState.BLOCKED;
                    }
                    else
                    {
                        newSqState[x, y] = SqState[x, y];
                        newSqFloorHeight[x, y] = SqFloorHeight[x, y];
                        newSqSeatRot[x, y] = SqSeatRot[x, y];

                    }
                }
            }

            SqState = newSqState;
            SqFloorHeight = newSqFloorHeight;
            SqSeatRot = newSqSeatRot;

            RelativeSerialized = false;
            HeightmapSerialized = false;
        }

        internal void SetUpdateState()
        {
            RelativeSerialized = false;
            HeightmapSerialized = false;
        }

        private ServerMessage SerializedRelativeHeightmap;
        private bool RelativeSerialized;

        private ServerMessage SerializedHeightmap;
        private bool HeightmapSerialized;

        internal ServerMessage SerializeRelativeHeightmap()
        {
            if (!RelativeSerialized)
            {
                SerializedRelativeHeightmap = GetRelativeHeightmap();
                RelativeSerialized = true;
            }

            return SerializedRelativeHeightmap;
        }

        internal ServerMessage GetHeightmap()
        {
            if (!HeightmapSerialized)
            {
                SerializedHeightmap = SerializeHeightmap();
                HeightmapSerialized = true;
            }

            return SerializedHeightmap;
        }

        private ServerMessage GetRelativeHeightmap()
        {
            ServerMessage Message = new ServerMessage(470);

            //Needs cache!
            for (int y = 0; y < MapSizeY; y++)
            {
                for (int x = 0; x < MapSizeX; x++)
                {
                    if (x == DoorX && y == DoorY)
                        Message.AppendString(DoorZ.ToString());
                    else if (SqState[x, y] == SquareState.BLOCKED)
                        Message.AppendString("x");
                    else
                        Message.AppendString(SqFloorHeight[x, y].ToString());
                }

                Message.AppendString(Convert.ToChar(13).ToString());
            }

            return Message;
        }

        internal ServerMessage SerializeHeightmap()
        {
            StringBuilder HeightMap = new StringBuilder();

            //Needs cache!
            for (int y = 0; y < MapSizeY; y++)
            {
                for (int x = 0; x < MapSizeX; x++)
                {
                    if (SqState[x, y] == SquareState.BLOCKED)
                        HeightMap.Append("x");
                    else if (x == DoorX && y == DoorY)
                        HeightMap.Append(DoorZ.ToString());
                    else
                        HeightMap.Append(SqFloorHeight[x, y]);
                }
                HeightMap.Append(Convert.ToChar(13));
            }

            ServerMessage Message = new ServerMessage(31);
            Message.AppendStringWithBreak(HeightMap.ToString());

            return Message;
        }


        internal void AddX()
        {
            MapSizeX++;
            refreshArrays();
        }

        internal void OpenSquare(int x, int y, double z)
        {
            if (z > 9)
                z = 9;
            if (z < 0)
                z = 0;
            SqFloorHeight[x, y] = (short)z;
            SqState[x, y] = SquareState.OPEN;
        }

        internal void AddY()
        {
            MapSizeY++;
            refreshArrays();
        }

        internal void SetMapsize(int x, int y)
        {
            MapSizeX = x;
            MapSizeY = y;
            refreshArrays();
        }

        internal void Destroy()
        {
            Array.Clear(SqState, 0, SqState.Length);
            Array.Clear(SqFloorHeight, 0, SqFloorHeight.Length);
            Array.Clear(SqSeatRot, 0, SqSeatRot.Length);

            staticModel = null;
            Heightmap = null;
            SqState = null;
            SqFloorHeight = null;
            SqSeatRot = null;
        }
    }
}
