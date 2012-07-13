using System;
using System.Collections.Generic;
using System.Data;
using Butterfly.Core;
using Butterfly.HabboHotel.Items;
using Database_Manager.Database.Session_Details.Interfaces;

namespace Butterfly.HabboHotel.Items
{
    class ItemManager
    {
        private Dictionary<UInt32, Item> Items;

        internal ItemManager()
        {
            Items = new Dictionary<uint, Item>();
        }

        internal void LoadItems(IQueryAdapter dbClient)
        {
            Items = new Dictionary<uint, Item>();

            dbClient.setQuery("SELECT * FROM items_base");
            DataTable ItemData = dbClient.getTable();

            if (ItemData != null)
            {
                uint id;
                int spriteID;
                string publicName;
                string itemName;
                string type;
                int width;
                int length;
                double height;
                bool allowStack;
                bool allowWalk;
                bool allowSit;
                bool allowRecycle;
                bool allowTrade;
                bool allowMarketplace;
                bool allowGift;
                bool allowInventoryStack;
                InteractionType interactionType;
                int cycleCount;
                string vendingIDS;

                foreach (DataRow dRow in ItemData.Rows)
                {
                    try
                    {
                        id = Convert.ToUInt16(dRow[0]);
                        spriteID = (int)dRow[1];
                        publicName = (string)dRow[2];
                        itemName = (string)dRow[3];
                        type = (string)dRow[4];
                        width = (int)dRow[5];
                        length = (int)dRow[6];
                        height = Convert.ToDouble(dRow[7]);
                        allowStack = Convert.ToInt32(dRow[8]) == 1;
                        allowWalk = Convert.ToInt32(dRow[9]) == 1;
                        allowSit = Convert.ToInt32(dRow[10]) == 1;
                        allowRecycle = Convert.ToInt32(dRow[11]) == 1;
                        allowTrade = Convert.ToInt32(dRow[12]) == 1;
                        allowMarketplace = Convert.ToInt32(dRow[13]) == 1;
                        allowGift = Convert.ToInt32(dRow[14]) == 1;
                        allowInventoryStack = Convert.ToInt32(dRow[15]) == 1;
                        interactionType = InterractionTypes.GetTypeFromString((string)dRow[16]);
                        cycleCount = (int)dRow[17];
                        vendingIDS = (string)dRow[18];

                        Item item = new Item(id, spriteID, publicName, itemName, type, width, length, height, allowStack, allowWalk, allowSit, allowRecycle, allowTrade, allowMarketplace, allowGift, allowInventoryStack, interactionType, cycleCount, vendingIDS);
                        Items.Add(id, item);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        Console.ReadKey();
                        Logging.WriteLine("Could not load item #" + Convert.ToUInt32(dRow[0]) + ", please verify the data is okay.");
                    }
                }
            }
        }

        internal Boolean ContainsItem(uint Id)
        {
            return Items.ContainsKey(Id);
        }

        internal Item GetItem(uint Id)
        {
            if (ContainsItem(Id))
                return Items[Id];

            return null;
        }
    }
}
