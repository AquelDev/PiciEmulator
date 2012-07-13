using System;

using Butterfly.Messages;
using Butterfly.HabboHotel.Rooms;
using Butterfly;

namespace Butterfly.HabboHotel.Navigators
{
    internal enum PublicImageType
    {
        INTERNAL = 0,
        EXTERNAL = 1
    }

    internal class PublicItem
    {
        private readonly Int32 BannerId;

        internal int Type;

        internal string Caption;
        internal string Image;
        internal PublicImageType ImageType;

        internal UInt32 RoomId;

        internal Int32 ParentId;

        internal Int32 Id
        {
            get { return BannerId; }
        }

        internal RoomData RoomData
        {
            get
            {
                if (RoomId == 0)
                {
                    return new RoomData();
                }

                if (ButterflyEnvironment.GetGame() == null)
                    throw new NullReferenceException();

                if (ButterflyEnvironment.GetGame().GetRoomManager() == null)
                    throw new NullReferenceException();

                if (ButterflyEnvironment.GetGame().GetRoomManager().GenerateRoomData(RoomId) == null)
                    throw new NullReferenceException();

                return ButterflyEnvironment.GetGame().GetRoomManager().GenerateRoomData(RoomId);
            }
        }

        internal Boolean Category;
        internal Boolean Recommended;

        internal PublicItem(int mId, int mType, string mCaption, string mImage, PublicImageType mImageType, uint mRoomId, int mParentId, Boolean mCategory, Boolean mRecommand)
        {
            BannerId = mId;
            Type = mType;
            Caption = mCaption;
            Image = mImage;
            ImageType = mImageType;
            RoomId = mRoomId;
            ParentId = mParentId;
            Category = mCategory;
            Recommended = mRecommand;
        }

        internal void Serialize(ServerMessage Message)
        {
            if (!Category)
            {
                Message.AppendInt32(Id);

                Message.AppendStringWithBreak((Type == 1) ? Caption : RoomData.Name);

                Message.AppendStringWithBreak(RoomData.Description);
                Message.AppendInt32(Type);
                Message.AppendStringWithBreak(Caption);
                Message.AppendStringWithBreak((ImageType == PublicImageType.EXTERNAL) ? Image : string.Empty);
                Message.AppendInt32(ParentId);
                Message.AppendInt32(RoomData.UsersNow);
                Message.AppendInt32(3);
                Message.AppendStringWithBreak((ImageType == PublicImageType.INTERNAL) ? Image : string.Empty);
                Message.AppendUInt(1337);
                Message.AppendBoolean(true);
                Message.AppendStringWithBreak(RoomData.CCTs);
                Message.AppendInt32(RoomData.UsersMax);
                Message.AppendUInt(RoomId);
            }
            else if (Category)
            {
                Message.AppendInt32(Id);
                Message.AppendStringWithBreak(Caption);
                Message.AppendStringWithBreak(string.Empty);
                Message.AppendBoolean(true);
                Message.AppendStringWithBreak(string.Empty);
                Message.AppendStringWithBreak(string.Empty);
                Message.AppendBoolean(false);
                Message.AppendBoolean(false);
                Message.AppendInt32(4);
                Message.AppendBoolean(false);
            }
        }
    }
}