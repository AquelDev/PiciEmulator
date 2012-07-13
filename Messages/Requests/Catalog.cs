using System;
using System.Collections.Generic;
using System.Data;
using Pici.HabboHotel.Catalogs;
using Pici.HabboHotel.Items;
using Pici.Core;
using Pici.Storage.Database.Session_Details.Interfaces;
using Pici.Messages.Interfaces;
using Pici.HabboHotel.GameClients;

namespace Pici.Messages
{
    partial class GameClientMessageHandler
    {
        internal void GetCatalogIndex()
        {
            Session.SendMessage(PiciEnvironment.GetGame().GetCatalog().GetIndexMessageForRank(Session.GetHabbo().Rank));
        }

        internal void GetCatalogPage()
        {
            CatalogPage Page = PiciEnvironment.GetGame().GetCatalog().GetPage(Request.PopWiredInt32());

            if (Page == null || !Page.Enabled || !Page.Visible || Page.MinRank > Session.GetHabbo().Rank)
            {
                return;
            }

            if (Page.ClubOnly && !Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
            {
                Session.SendNotif(LanguageLocale.GetValue("catalog.missingclubmembership"));
                return;
            }

            Session.SendMessage(Page.GetMessage);

            if (Page.Layout == "recycler")
            {
                GetResponse().Init(507);
                GetResponse().AppendBoolean(true);
                GetResponse().AppendBoolean(false);
                SendResponse();
            }
        }

        internal void RedeemVoucher()
        {
            VoucherHandler.TryRedeemVoucher(Session, Request.PopFixedString());
        }

        internal void HandleRowchase()
        {
            int PageId = Request.PopWiredInt32();
            uint ItemId = Request.PopWiredUInt();
            string ExtraData = Request.PopFixedString();

            for (int i = 0; i < Session.GetHabbo().buyItemLoop; i++)
            {
                PiciEnvironment.GetGame().GetCatalog().HandleRowchase(Session, PageId, ItemId, ExtraData, false, "", "");
            }
        }

        internal void RowchaseGift()
        {
            int PageId = Request.PopWiredInt32();
            uint ItemId = Request.PopWiredUInt();
            string ExtraData = Request.PopFixedString();
            string GiftUser = PiciEnvironment.FilterInjectionChars(Request.PopFixedString());
            string GiftMessage = PiciEnvironment.FilterInjectionChars(Request.PopFixedString());

            PiciEnvironment.GetGame().GetCatalog().HandleRowchase(Session, PageId, ItemId, ExtraData, true, GiftUser, GiftMessage);
        }

        internal void GetRecyclerRewards()
        {
            // GzQAQAXtGIsZJKPAPrIsXLKKPJKsY}JsXBKsX~JJPASCsX|JiXBPsZAKs[|JiYBPsZ}JsXAKsYAKsX}JsY|JsY~Js[{JiZAPs[JsZBKIIRAsX@KsYBKsZJs[@Ks[~JsZ|J

            GetResponse().Init(506);
            GetResponse().AppendInt32(5);

            for (uint i = 5; i >= 1; i--)
            {
                GetResponse().AppendUInt(i);

                if (i <= 1)
                {
                    GetResponse().AppendInt32(0);
                }
                else if (i == 2)
                {
                    GetResponse().AppendInt32(4);
                }
                else if (i == 3)
                {
                    GetResponse().AppendInt32(40);
                }
                else if (i == 4)
                {
                    GetResponse().AppendInt32(200);
                }
                else if (i >= 5)
                {
                    GetResponse().AppendInt32(2000);
                }

                List<EcotronReward> Rewards = PiciEnvironment.GetGame().GetCatalog().GetEcotronRewardsForLevel(i);

                GetResponse().AppendInt32(Rewards.Count);

                foreach (EcotronReward Reward in Rewards)
                {
                    GetResponse().AppendStringWithBreak(Reward.GetBaseItem().Type.ToString().ToLower());
                    GetResponse().AppendUInt(Reward.DisplayId);
                }
            }

            SendResponse();
        }

        internal void CanGift()
        {
            uint Id = Request.PopWiredUInt();

            CatalogItem Item = PiciEnvironment.GetGame().GetCatalog().FindItem(Id);

            if (Item == null)
            {
                return;
            }

            GetResponse().Init(622);
            GetResponse().AppendUInt(Item.Id);
            GetResponse().AppendBoolean(Item.GetBaseItem().AllowGift);
            SendResponse();
        }

        internal void GetCataData1()
        {
            GetResponse().Init(612);
            //  1 1 1 5 1 10000 48 7
            GetResponse().AppendInt32(1);
            GetResponse().AppendInt32(1);
            GetResponse().AppendInt32(1);
            GetResponse().AppendInt32(5);
            GetResponse().AppendInt32(1);
            GetResponse().AppendInt32(10000);
            GetResponse().AppendInt32(48);
            GetResponse().AppendInt32(7);
            SendResponse();
        }

        internal void GetCataData2()
        {
            GetResponse().Init(620);
            GetResponse().AppendInt32(1);
            GetResponse().AppendInt32(1);
            GetResponse().AppendInt32(10);
            for (int i = 3372; i < 3381; i++)
            {
                GetResponse().AppendInt32(i);
            }
            GetResponse().AppendInt32(7);
            GetResponse().AppendInt32(0);
            GetResponse().AppendInt32(1);
            GetResponse().AppendInt32(2);
            GetResponse().AppendInt32(3);
            GetResponse().AppendInt32(4);
            GetResponse().AppendInt32(5);
            GetResponse().AppendInt32(6);
            GetResponse().AppendInt32(11);
            GetResponse().AppendInt32(0);
            GetResponse().AppendInt32(1);
            GetResponse().AppendInt32(2);
            GetResponse().AppendInt32(3);
            GetResponse().AppendInt32(4);
            GetResponse().AppendInt32(5);
            GetResponse().AppendInt32(6);
            GetResponse().AppendInt32(7);
            GetResponse().AppendInt32(8);
            GetResponse().AppendInt32(9);
            GetResponse().AppendInt32(10);
            GetResponse().AppendInt32(1);
            SendResponse();
        }

        internal void MarketplaceCanSell()
        {
            GetResponse().Init(611);
            GetResponse().AppendBoolean(true);
            GetResponse().AppendInt32(99999);
            SendResponse();
        }

        internal void MarketplacePostItem()
        {
            if (Session.GetHabbo().GetInventoryComponent() == null)
            {
                return;
            }

            int sellingPrice = Request.PopWiredInt32();
            int junk = Request.PopWiredInt32();
            uint itemId = Request.PopWiredUInt();

            UserItem Item = Session.GetHabbo().GetInventoryComponent().GetItem(itemId);

            if (Item == null || !Item.GetBaseItem().AllowTrade)
            {
                return;
            }

            Marketplace.SellItem(Session, Item.Id, sellingPrice);
        }

        internal void MarketplaceGetOwnOffers()
        {
            Session.SendMessage(Marketplace.SerializeOwnOffers(Session.GetHabbo().Id));
        }

        internal void MarketplaceTakeBack()
        {
            uint ItemId = Request.PopWiredUInt();
            DataRow Row = null;

            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.setQuery("SELECT item_id, user_id, extra_data, offer_id state FROM catalog_marketplace_offers WHERE offer_id = " + ItemId + " LIMIT 1");
                Row = dbClient.getRow();
            }

            if (Row == null || Convert.ToUInt32(Row["user_id"]) != Session.GetHabbo().Id || (UInt32)Row["state"] != 1)
            {
                return;
            }

            Item Item = PiciEnvironment.GetGame().GetItemManager().GetItem(Convert.ToUInt32(Row["item_id"]));

            if (Item == null)
            {
                return;
            }

            PiciEnvironment.GetGame().GetCatalog().DeliverItems(Session, Item, 1, (String)Row["extra_data"]);

            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.runFastQuery("DELETE FROM catalog_marketplace_offers WHERE offer_id = " + ItemId + "");
            }

            GetResponse().Init(614);
            GetResponse().AppendUInt(Convert.ToUInt32(Row["offer_id"]));
            GetResponse().AppendBoolean(true);
            SendResponse();
        }

        internal void MarketplaceClaimCredits()
        {
            DataTable Results = null;

            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.setQuery("SELECT asking_price FROM catalog_marketplace_offers WHERE user_id = " + Session.GetHabbo().Id + " AND state = 2");
                Results = dbClient.getTable();
            }

            if (Results == null)
            {
                return;
            }

            int Profit = 0;

            foreach (DataRow Row in Results.Rows)
            {
                Profit += (int)Row["asking_price"];
            }

            if (Profit >= 1)
            {
                Session.GetHabbo().Credits += Profit;
                Session.GetHabbo().UpdateCreditsBalance();
            }

            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.runFastQuery("DELETE FROM catalog_marketplace_offers WHERE user_id = " + Session.GetHabbo().Id + " AND state = 2");
            }
        }

        internal void MarketplaceGetOffers()
        {
            int MinPrice = Request.PopWiredInt32();
            int MaxPrice = Request.PopWiredInt32();
            string SearchQuery = Request.PopFixedString();
            int FilterMode = Request.PopWiredInt32();

            Session.SendMessage(Marketplace.SerializeOffers(MinPrice, MaxPrice, SearchQuery, FilterMode));
        }

        internal void MarketplaceRowchase()
        {
            uint ItemId = Request.PopWiredUInt();
            DataRow Row = null;

            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.setQuery("SELECT state, timestamp, total_price, extra_data, item_id FROM catalog_marketplace_offers WHERE offer_id = " + ItemId + " ");
                Row = dbClient.getRow();
            }

            if (Row == null || (string)Row["state"] != "1" || (double)Row["timestamp"] <= Marketplace.FormatTimestamp())
            {
                Session.SendNotif(LanguageLocale.GetValue("catalog.offerexpired"));
                return;
            }

            Item Item = PiciEnvironment.GetGame().GetItemManager().GetItem(Convert.ToUInt32(Row["item_id"]));

            if (Item == null)
            {
                return;
            }

            int prize = (int)Row["total_price"];
            if ((int)Row["total_price"] >= 1)
            {
                Session.GetHabbo().Credits -= prize;
                Session.GetHabbo().UpdateCreditsBalance();
            }

            PiciEnvironment.GetGame().GetCatalog().DeliverItems(Session, Item, 1, (String)Row["extra_data"]);
            Session.GetHabbo().GetInventoryComponent().RunDBUpdate();

            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.runFastQuery("UPDATE catalog_marketplace_offers SET state = 2 WHERE offer_id = " + ItemId + "");
            }


            Session.GetMessageHandler().GetResponse().Init(67);
            Session.GetMessageHandler().GetResponse().AppendUInt(Item.ItemId);
            Session.GetMessageHandler().GetResponse().AppendStringWithBreak(Item.Name);
            Session.GetMessageHandler().GetResponse().AppendInt32(prize);
            Session.GetMessageHandler().GetResponse().AppendInt32(0);
            Session.GetMessageHandler().GetResponse().AppendInt32(0);
            Session.GetMessageHandler().GetResponse().AppendInt32(1);
            Session.GetMessageHandler().GetResponse().AppendStringWithBreak(Item.Type.ToString());
            Session.GetMessageHandler().GetResponse().AppendInt32(Item.SpriteId);
            Session.GetMessageHandler().GetResponse().AppendStringWithBreak("");
            Session.GetMessageHandler().GetResponse().AppendInt32(1);
            Session.GetMessageHandler().GetResponse().AppendInt32(0);
            Session.GetMessageHandler().SendResponse();

            Session.SendMessage(Marketplace.SerializeOffers(-1, -1, "", 1));
        }

        internal void CheckPetName()
        {
            Session.GetMessageHandler().GetResponse().Init(36);
            Session.GetMessageHandler().GetResponse().Append(Catalog.CheckPetName(Request.PopFixedString()) ? 0 : 2);
            Session.GetMessageHandler().SendResponse();
        }

        internal void PetRaces()
        {
            string PetType = Request.PopFixedString();

            int count = 0, petid = 0;
            GetResponse().Init(827);

            switch (PetType)
            {
                case "a0 pet0":
                    GetResponse().Append("a0 pet0");
                    count = 25;
                    petid = 0;
                    break;

                case "a0 pet1":
                    GetResponse().Append("a0 pet1");
                    count = 25;
                    petid = 1;
                    break;

                case "a0 pet2":
                    GetResponse().Append("a0 pet2");
                    count = 12;
                    petid = 2;
                    break;

                case "a0 pet3":
                    GetResponse().Append("a0 pet3");
                    count = 7;
                    petid = 3;
                    break;

                case "a0 pet4":
                    GetResponse().Append("a0 pet4");
                    count = 4;
                    petid = 4;
                    break;

                case "a0 pet5":
                    GetResponse().Append("a0 pet5");
                    count = 7;
                    petid = 5;
                    break;

                case "a0 pet6":
                    GetResponse().Append("a0 pet6");
                    count = 13;
                    petid = 6;
                    break;

                case "a0 pet7":
                    GetResponse().Append("a0 pet7");
                    count = 8;
                    petid = 7;
                    break;

                case "a0 pet8":
                    GetResponse().Append("a0 pet8");
                    count = 13;
                    petid = 8;
                    break;

                case "a0 pet9":
                    GetResponse().Append("a0 pet9");
                    count = 14;
                    petid = 9;
                    break;

                case "a0 pet10":
                    GetResponse().Append("a0 pet10");
                    count = 1;
                    petid = 10;
                    break;

                case "a0 pet11":
                    GetResponse().Append("a0 pet11");
                    count = 14;
                    petid = 11;
                    break;

                case "a0 pet12":
                    GetResponse().Append("a0 pet12");
                    count = 8;
                    petid = 12;
                    break;

                case "a0 pet14":
                    GetResponse().Append("a0 pet14");
                    count = 9;
                    petid = 14;
                    break;

                case "a0 pet15":
                    GetResponse().Append("a0 pet15");
                    count = 16;
                    petid = 15;
                    break;
            }

            GetResponse().Append(count);
            for (int i = 0; i < count; i++)
            {
                GetResponse().Append(petid); // pet id
                GetResponse().Append(i); // race id
                GetResponse().Append(1); // active
                GetResponse().Append(0); // off
            }
            SendResponse();
        }

        //internal void RegisterCatalog()
        //{
        //    RequestHandlers.Add(101, new RequestHandler(GetCatalogIndex));
        //    RequestHandlers.Add(102,new RequestHandler(GetCatalogPage));
        //    RequestHandlers.Add(129,new RequestHandler(RedeemVoucher));
        //    RequestHandlers.Add(100,new RequestHandler(HandleRowchase));
        //    RequestHandlers.Add(472,new RequestHandler(RowchaseGift));
        //    RequestHandlers.Add(412,new RequestHandler(GetRecyclerRewards));
        //    RequestHandlers.Add(3030,new RequestHandler(CanGift));
        //    RequestHandlers.Add(3011,new RequestHandler(GetCataData1));
        //    RequestHandlers.Add(473,new RequestHandler(GetCataData2));
        //    RequestHandlers.Add(3012,new RequestHandler(MarketplaceCanSell));
        //    RequestHandlers.Add(3010,new RequestHandler(MarketplacePostItem));
        //    RequestHandlers.Add(3019,new RequestHandler(MarketplaceGetOwnOffers));
        //    RequestHandlers.Add(3015,new RequestHandler(MarketplaceTakeBack));
        //    RequestHandlers.Add(3016,new RequestHandler(MarketplaceClaimCredits));
        //    RequestHandlers.Add(3018,new RequestHandler(MarketplaceGetOffers));
        //    RequestHandlers.Add(3014,new RequestHandler(MarketplaceRowchase));
        //    RequestHandlers.Add(42, new RequestHandler(CheckPetName));
        //    RequestHandlers.Add(3007, new RequestHandler(PetRaces));
        //}

        //internal void UnregisterCatalog()
        //{
        //    RequestHandlers.Remove(101);
        //    RequestHandlers.Remove(102);
        //    RequestHandlers.Remove(129);
        //    RequestHandlers.Remove(100);
        //    RequestHandlers.Remove(472);
        //    RequestHandlers.Remove(412);
        //    RequestHandlers.Remove(3030);
        //    RequestHandlers.Remove(3011);
        //    RequestHandlers.Remove(473);
        //    RequestHandlers.Remove(3012);
        //    RequestHandlers.Remove(3010);
        //    RequestHandlers.Remove(3019);
        //    RequestHandlers.Remove(3015);
        //    RequestHandlers.Remove(3016);
        //    RequestHandlers.Remove(3018);
        //    RequestHandlers.Remove(3014);
        //    RequestHandlers.Remove(3007);
        //    RequestHandlers.Remove(42);
        //}
    }
}
