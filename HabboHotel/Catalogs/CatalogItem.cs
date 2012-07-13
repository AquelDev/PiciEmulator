using System;
using System.Data;
using Pici;
using Pici.HabboHotel.Items;
using Pici.Messages;

namespace Pici.HabboHotel.Catalogs
{
    class CatalogItem
    {
        internal readonly uint Id;
        internal readonly uint ItemIds;
        internal readonly string Name;
        internal readonly int CreditsCost;
        internal readonly int PixelsCost;
        internal readonly int Amount;
        internal readonly int PageID;
        internal readonly int CrystalCost;
        internal readonly int OudeCredits;
        internal readonly uint songID;

        internal CatalogItem(DataRow Row) 
        {
            this.Id = Convert.ToUInt32(Row["id"]);
            this.Name = (string)Row["catalog_name"];
            this.ItemIds = Convert.ToUInt32(Row["item_ids"]);
            this.PageID = (int)Row["page_id"];
            this.CreditsCost = (int)Row["cost_credits"];
            this.PixelsCost = (int)Row["cost_pixels"];
            this.Amount = (int)Row["amount"];
            this.CrystalCost = (int)Row["cost_crystal"];
            this.OudeCredits = (int)Row["cost_oude_belcredits"];
            this.songID = Convert.ToUInt32(Row["song_id"]);
            //this.songID = 0;
        }

        internal Item GetBaseItem()
        {
            Item Return = PiciEnvironment.GetGame().GetItemManager().GetItem(ItemIds);
            if (Return == null)
            {
                Console.WriteLine("UNKNOWN ItemIds: " + ItemIds);
            }

            return Return;
        }

        internal void Serialize(ServerMessage Message)
        {
            try
            {
                Message.AppendUInt(Id);
                Message.AppendStringWithBreak(Name);
                Message.AppendInt32(CreditsCost);

                if (CrystalCost > 0)
                {
                    Message.AppendInt32(CrystalCost);
                    Message.AppendInt32(4); // Hearts
                }
                else
                {
                    Message.AppendInt32(PixelsCost);
                    Message.AppendInt32(0); // Page Type; 0 = nothing, 1 = Snowflakes, 2 = hearts, 3 = ?
                }
                Message.AppendInt32(1);
                Message.AppendStringWithBreak(GetBaseItem().Type.ToString());
                Message.AppendInt32(GetBaseItem().SpriteId);

                if (Name.Contains("wallpaper_single") || Name.Contains("floor_single") || Name.Contains("landscape_single"))
                {
                    string[] Analyze = Name.Split('_');
                    Message.AppendStringWithBreak(Analyze[2]);
                }
                else if (this.songID > 0 && GetBaseItem().InteractionType == InteractionType.musicdisc)
                {
                    Message.AppendStringWithBreak(songID.ToString());
                }
                else
                {
                    Message.AppendStringWithBreak(string.Empty);
                }
                Message.AppendInt32(Amount);
                Message.AppendInt32(-1);
                Message.AppendInt32(0);
            }
            catch
            {
                Console.WriteLine("Unable to load furniture item " + Id + ": " + Name);
            }
                
        }
    }
}
