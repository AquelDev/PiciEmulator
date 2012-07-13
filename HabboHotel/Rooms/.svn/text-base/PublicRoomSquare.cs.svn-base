using System.Data;

namespace Butterfly.HabboHotel.Rooms
{
    struct PublicRoomSquare
    {
        private string mRoomModelName;
        private int mX;
        private int mY;
        private byte mRotation;

        internal PublicRoomSquare(DataRow dRow)
        {
            mRoomModelName = (string)dRow["model"];
            mX = (int)dRow["x"];
            mY = (int)dRow["y"];
            mRotation = (byte)((int)dRow["rot"]);
        }

        internal string RoomModelName
        {
            get
            {
                return mRoomModelName;
            }
        }

        internal int X
        {
            get
            {
                return mX;
            }
        }

        internal int Y
        {
            get
            {
                return mY;
            }
        }

        //internal string Name
        //{
        //    get
        //    {
        //        return mName;
        //    }
        //}

        //internal bool IsSeat
        //{
        //    get
        //    {
        //        return mIsSitable;
        //    }
        //}

        internal byte Rotation
        {
            get
            {
                return mRotation;
            }
        }
    }
}
