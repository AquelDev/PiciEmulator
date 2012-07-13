using System;
using System.Collections.Generic;
using Butterfly.HabboHotel.Items;

namespace Butterfly.HabboHotel.Items
{
    class Item
    {
        private uint Id;

        internal int SpriteId;

        internal string PublicName;
        internal string Name;
        internal char Type;

        internal int Width;
        internal int Length;
        internal double Height;

        internal bool Stackable;
        internal bool Walkable;
        internal bool IsSeat;

        internal bool AllowRecycle;
        internal bool AllowTrade;
        internal bool AllowMarketplaceSell;
        internal bool AllowGift;
        internal bool AllowInventoryStack;

        internal InteractionType InteractionType;

        internal List<int> VendingIds;

        internal int Modes;

        

        internal uint ItemId
        {
            get
            {
                return Id;
            }
        }

        internal Item(UInt32 Id, int Sprite, string PublicName, string Name, string Type, int Width, int Length, double Height, bool Stackable, bool Walkable, bool IsSeat, bool AllowRecycle, bool AllowTrade, bool AllowMarketplaceSell, bool AllowGift, bool AllowInventoryStack, InteractionType InteractionType, int Modes, string VendingIds)
        {
            this.Id = Id;
            this.SpriteId = Sprite;
            this.PublicName = PublicName;
            this.Name = Name;
            this.Type = char.Parse(Type);
            this.Width = Width;
            this.Length = Length;
            this.Height = Height;
            this.Stackable = Stackable;
            this.Walkable = Walkable;
            this.IsSeat = IsSeat;
            this.AllowRecycle = AllowRecycle;
            this.AllowTrade = AllowTrade;
            this.AllowMarketplaceSell = AllowMarketplaceSell;
            this.AllowGift = AllowGift;
            this.AllowInventoryStack = AllowInventoryStack;
            this.InteractionType = InteractionType;
            this.Modes = Modes;
            this.VendingIds = new List<int>();

            foreach (string VendingId in VendingIds.Split(','))
            {
                this.VendingIds.Add(int.Parse(VendingId));
            }
        }
    }
}
