﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Pici;
using Pici.Core;
using Pici.HabboHotel.GameClients;
using Pici.HabboHotel.Items;
using Pici.HabboHotel.Pets;
using Pici.HabboHotel.Users.Inventory;
using Pici.HabboHotel.Users.Subscriptions;
using Pici.Messages;
using Pici.Messages.Headers;
using Pici.Storage.Database;
using Pici.Storage.Database.Session_Details.Interfaces;

namespace Pici.HabboHotel.Catalogs
{
    class Catalog
    {
        internal Dictionary<int, CatalogPage> Pages;
        internal List<EcotronReward> EcotronRewards;
        private Hashtable gifts;

        private Marketplace Marketplace;

        private ServerMessage[] mCataIndexCache;
        //private Task mFurniIDCYcler;

        internal Catalog()
        {
            Marketplace = new Marketplace();
        }

        internal void Initialize(IQueryAdapter dbClient)
        {
            Pages = new Dictionary<int, CatalogPage>();
            EcotronRewards = new List<EcotronReward>();
            gifts = new Hashtable();

            dbClient.setQuery("SELECT * FROM catalog_pages ORDER BY order_num");
            DataTable Data = dbClient.getTable();

            dbClient.setQuery("SELECT * FROM ecotron_rewards ORDER BY item_id");
            DataTable EcoData = dbClient.getTable();

            Hashtable CataItems = new Hashtable();
            dbClient.setQuery("SELECT id,item_ids,catalog_name,cost_credits,cost_pixels,amount,page_id,cost_crystal,cost_oude_belcredits,song_id FROM catalog_items_copy");
            DataTable CatalogueItems = dbClient.getTable();

            if (CatalogueItems != null)
            {
                foreach (DataRow Row in CatalogueItems.Rows)
                {
                    if (string.IsNullOrEmpty(Row["item_ids"].ToString()) || (int)Row["amount"] <= 0)
                    {
                        continue;
                    }
                    CataItems.Add(Convert.ToUInt32(Row["id"]), new CatalogItem(Row));
                    //Items.Add(new CatalogItem((uint)Row["id"], (string)Row["catalog_name"], (string)Row["item_ids"], (int)Row["cost_credits"], (int)Row["cost_pixels"], (int)Row["amount"]));
                }
            }

            if (Data != null)
            {
                foreach (DataRow Row in Data.Rows)
                {
                    Boolean Visible = false;
                    Boolean Enabled = false;
                    Boolean ComingSoon = false;

                    if (Row["visible"].ToString() == "1")
                    {
                        Visible = true;
                    }

                    if (Row["enabled"].ToString() == "1")
                    {
                        Enabled = true;
                    }

                    if (Row["coming_soon"].ToString() == "1")
                    {
                        ComingSoon = true;
                    }

                    Pages.Add((int)Row["id"], new CatalogPage((int)Row["id"], (int)Row["parent_id"],
                        (string)Row["caption"], Visible, Enabled, ComingSoon, Convert.ToUInt32(Row["min_rank"]),
                        PiciEnvironment.EnumToBool(Row["club_only"].ToString()), (int)Row["icon_color"],
                        (int)Row["icon_image"], (string)Row["page_layout"], (string)Row["page_headline"],
                        (string)Row["page_teaser"], (string)Row["page_special"], (string)Row["page_text1"],
                        (string)Row["page_text2"], (string)Row["page_text_details"], (string)Row["page_text_teaser"], ref CataItems));
                }
            }

            if (EcoData != null)
            {
                foreach (DataRow Row in EcoData.Rows)
                {
                    EcotronRewards.Add(new EcotronReward(Convert.ToUInt32(Row["display_id"]), Convert.ToUInt32(Row["item_id"]), Convert.ToUInt32(Row["reward_level"])));
                }
            }

            RestackByFrontpage();
        }

        internal void RestackByFrontpage()
        {
            CatalogPage fronpage = Pages[1];
            Dictionary<int, CatalogPage> restOfCata = new Dictionary<int, CatalogPage>(Pages);

            restOfCata.Remove(1);
            Pages.Clear();

            Pages.Add(fronpage.PageId, fronpage);

            foreach (KeyValuePair<int, CatalogPage> pair in restOfCata)
                Pages.Add(pair.Key, pair.Value);
        }

        internal void InitCache()
        {
            mCataIndexCache = new ServerMessage[8]; //Max 7 ranks

            for (int i = 1; i < 8; i++)
            {
                mCataIndexCache[i] = SerializeIndexForCache(i);
            }

            foreach (CatalogPage Page in Pages.Values)
            {
                Page.InitMsg();
            }

            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.setQuery("SELECT * FROM items_base_gifts");
                DataTable dgifts = dbClient.getTable();
                foreach (DataRow dRow in dgifts.Rows)
                {
                    uint ID = Convert.ToUInt32(dRow[0]);
                    Item baseItem = PiciEnvironment.GetGame().GetItemManager().GetItem(ID);

                    if (baseItem == null)
                    {
                        Console.WriteLine("WARNING: Unknown gift ID: " + ID);
                        continue;
                    }

                    gifts.Add(ID, baseItem);
                }
            }
        }

        internal CatalogItem FindItem(uint ItemId)
        {
            foreach (CatalogPage Page in Pages.Values)
            {
                if (Page.Items.ContainsKey(ItemId))
                    return (CatalogItem)Page.Items[ItemId];
            }

            return null;
        }

        //internal Boolean IsItemInCatalog(uint BaseId)
        //{
        //    DataRow Row = null;

        //    using (DatabaseClient dbClient = ButterflyEnvironment.GetDatabase().GetClient())
        //    {
        //        Row = dbClient.getRow("SELECT id FROM catalog_items WHERE item_ids = '" + BaseId + "' LIMIT 1");
        //    }

        //    if (Row != null)
        //    {
        //        return true;
        //    }

        //    return false;
        //}

        internal int GetTreeSize(int rank, int TreeId)
        {
            int i = 0;

            foreach (CatalogPage Page in Pages.Values)
            {
                if (Page.MinRank > rank)
                {
                    continue;
                }

                if (Page.ParentId == TreeId)
                {
                    i++;
                }
            }


            return i;
        }

        internal CatalogPage GetPage(int Page)
        {
            if (!Pages.ContainsKey(Page))
            {
                return null;
            }

            return Pages[Page];
        }

        internal void HandleRowchase(GameClient Session, int PageId, uint ItemId, string ExtraData, Boolean IsGift, string GiftUser, string GiftMessage)
        {
            CatalogPage Page;


            if (!Pages.TryGetValue(PageId, out Page))
                return;

            if (Page == null || Page.ComingSoon || !Page.Enabled || !Page.Visible || Session == null || Session.GetHabbo() == null)
            {
                return;
            }

            if (Page.ClubOnly && !Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
            {
                return;
            }
            if (Page.MinRank > Session.GetHabbo().Rank)
            {
                return;
            }
            CatalogItem Item = Page.GetItem(ItemId);

            if (Item == null)
            {
                return;
            }

            uint GiftUserId = 0;

            if (IsGift)
            {
                if (!Item.GetBaseItem().AllowGift)
                {
                    return;
                }

                DataRow dRow;
                using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    dbClient.setQuery("SELECT id FROM users WHERE username = @gift_user");
                    dbClient.addParameter("gift_user", GiftUser);


                    dRow = dbClient.getRow();
                }

                if (dRow == null)
                {
                    Session.GetMessageHandler().GetResponse().Init(76);
                    Session.GetMessageHandler().GetResponse().AppendBoolean(true);
                    Session.GetMessageHandler().GetResponse().AppendStringWithBreak(GiftUser);
                    Session.GetMessageHandler().SendResponse();

                    return;
                }

                GiftUserId = Convert.ToUInt32(dRow[0]);

                if (GiftUserId == 0)
                {
                    Session.GetMessageHandler().GetResponse().Init(76);
                    Session.GetMessageHandler().GetResponse().AppendBoolean(true);
                    Session.GetMessageHandler().GetResponse().AppendStringWithBreak(GiftUser);
                    Session.GetMessageHandler().SendResponse();

                    return;
                }
            }

            Boolean CreditsError = false;
            Boolean PixelError = false;

            if (Session.GetHabbo().Credits < Item.CreditsCost)
            {
                CreditsError = true;
            }

            if (Session.GetHabbo().ActivityPoints < Item.PixelsCost)
            {
                PixelError = true;
            }

            if (CreditsError || PixelError)
            {
                Session.GetMessageHandler().GetResponse().Init(68);
                Session.GetMessageHandler().GetResponse().AppendBoolean(CreditsError);
                Session.GetMessageHandler().GetResponse().AppendBoolean(PixelError);
                Session.GetMessageHandler().SendResponse();

                return;
            }

            if (IsGift && Item.GetBaseItem().Type == 'e')
            {
                Session.SendNotif(LanguageLocale.GetValue("catalog.gift.send.error"));
                return;
            }


            if (Item.CrystalCost > 0)
            {
                int userCrystals = 0;
                using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    dbClient.setQuery("SELECT crystals FROM users WHERE id = " + Session.GetHabbo().Id);
                    userCrystals = dbClient.getInteger();
                }

                if (Item.CrystalCost > userCrystals)
                {
                    Session.SendNotif(LanguageLocale.GetValue("catalog.crystalerror") + Item.CrystalCost);
                    return;
                }

                userCrystals = userCrystals - Item.CrystalCost;

                Session.GetMessageHandler().GetResponse().Init(MessageComposerIds.ActivityPointsMessageComposer);
                Session.GetMessageHandler().GetResponse().AppendInt32(2);
                Session.GetMessageHandler().GetResponse().AppendInt32(4);
                Session.GetMessageHandler().GetResponse().AppendInt32(userCrystals);
                Session.GetMessageHandler().SendResponse();

                using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    dbClient.runFastQuery("UPDATE users SET crystals = " + userCrystals + " WHERE id = " + Session.GetHabbo().Id);

                }

                Session.SendNotif(LanguageLocale.GetValue("catalog.crystalsbought") + userCrystals);
            }

            if (Item.Name.Contains("HABBO_CLUB"))
            {
                #region Purchase Club!
                // PAYmQHABBO_CLUB_BASIC_1_MONTHSCHHISGZvGSBSE[mQHABBO_CLUB_VIP_1_MONTHQFHIISGZvGSBSEXnQHABBO_CLUB_VIP_3_MONTHSPOHIKQW[vGIPFZmQHABBO_CLUB_BASIC_3_MONTHSQKHHKQW[vGIPF
                int TypeOfClub = 0;
                Subscription Sub;
                if (Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_vip"))
                    Sub = Session.GetHabbo().GetSubscriptionManager().GetSubscription("habbo_vip");
                else if (Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
                    Sub = Session.GetHabbo().GetSubscriptionManager().GetSubscription("habbo_club");
                else
                    Sub = null;

                if (Item.Name == "HABBO_CLUB_BASIC_1_MONTH")
                {
                    TypeOfClub = 1;
                }
                else if (Item.Name.Contains("HABBO_CLUB_BASIC_3"))
                {
                    TypeOfClub = 2;
                }
                else if (Item.Name == "HABBO_CLUB_VIP_1_MONTH")
                {
                    TypeOfClub = 3;
                }
                else if (Item.Name.Contains("HABBO_CLUB_VIP_3"))
                {
                    TypeOfClub = 4;
                }
                else if (Item.Name == "HABBO_CLUB_UPGRADE_1")
                {
                    TypeOfClub = 5;
                }
                else if (Item.Name == "HABBO_CLUB_UPGRADE_3")
                {
                    TypeOfClub = 6;
                }

                if (TypeOfClub == 1)
                {
                    Session.GetHabbo().GetSubscriptionManager().AddOrExtendSubscription("habbo_club", (60 * 60 * 24 * 31));
                    ServerMessage FuseRight = new ServerMessage(2);
                    FuseRight.AppendInt32(1);
                    Session.SendMessage(FuseRight);
                    Session.GetMessageHandler().GetSubscriptionData();
                }
                else if (TypeOfClub == 2)
                {
                    Session.GetHabbo().GetSubscriptionManager().AddOrExtendSubscription("habbo_club", (60 * 60 * 24 * 31 * 3));
                    ServerMessage FuseRight = new ServerMessage(2);
                    FuseRight.AppendInt32(1);
                    Session.SendMessage(FuseRight);
                    Session.GetMessageHandler().GetSubscriptionData();
                }
                else if (TypeOfClub == 3)
                {
                    Session.GetHabbo().GetSubscriptionManager().AddOrExtendSubscription("habbo_vip", (60 * 60 * 24 * 31));
                    ServerMessage FuseRight = new ServerMessage(2);
                    FuseRight.AppendInt32(2);
                    Session.SendMessage(FuseRight);
                    Session.GetMessageHandler().GetSubscriptionData();
                }
                else if (TypeOfClub == 4)
                {
                    Session.GetHabbo().GetSubscriptionManager().AddOrExtendSubscription("habbo_vip", (60 * 60 * 24 * 31 * 3));
                    ServerMessage FuseRight = new ServerMessage(2);
                    FuseRight.AppendInt32(2);
                    Session.SendMessage(FuseRight);
                    Session.GetMessageHandler().GetSubscriptionData();
                }
                else if (TypeOfClub == 5)
                {
                    Session.GetHabbo().GetSubscriptionManager().AddOrExtendSubscription("habbo_vip", (60 * 60 * 24 * 31));
                    ServerMessage FuseRight = new ServerMessage(2);
                    FuseRight.AppendInt32(2);
                    Session.SendMessage(FuseRight);
                    Session.GetMessageHandler().GetSubscriptionData();
                }
                else if (TypeOfClub == 6)
                {
                    Session.GetHabbo().GetSubscriptionManager().AddOrExtendSubscription("habbo_vip", (60 * 60 * 24 * 31 * 3));
                    ServerMessage FuseRight = new ServerMessage(2);
                    FuseRight.AppendInt32(2);
                    Session.SendMessage(FuseRight);
                    Session.GetMessageHandler().GetSubscriptionData();
                }

                bool CreditsFail = false;
                bool PixelsFail = false;

                if (Session.GetHabbo().Credits < Item.CreditsCost)
                {
                    CreditsFail = true;
                }

                if (CreditsFail || PixelsFail)
                {
                    ServerMessage Failed = new ServerMessage(68);
                    Failed.AppendBoolean(CreditsFail);
                    Failed.AppendBoolean(PixelsFail);
                    Session.SendMessage(Failed);
                    return;
                }

                if (Item.CreditsCost > 0)
                {
                    Session.GetHabbo().Credits -= Item.CreditsCost;
                    Session.GetHabbo().UpdateCreditsBalance();
                }

                ServerMessage PurchaseClub = new ServerMessage(67);
                //AC[mQHABBO_CLUB_VIP_1_MONTH{2}QFHHH{1}
                PurchaseClub.AppendUInt(Item.Id);
                PurchaseClub.AppendStringWithBreak(Item.Name);
                PurchaseClub.AppendInt32(Item.CreditsCost);
                PurchaseClub.AppendBoolean(true);
                PurchaseClub.AppendBoolean(false);
                PurchaseClub.AppendBoolean(false);
                Session.SendMessage(PurchaseClub);
                #endregion
                return;
            }

            if (Item.OudeCredits > 0)
            {
                int oudeCredits = 0;
                using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    dbClient.setQuery("SELECT belcredits FROM users WHERE id = " + Session.GetHabbo().Id);
                    oudeCredits = dbClient.getInteger();
                }

                if (Item.OudeCredits > oudeCredits)
                {
                    Session.SendNotif(LanguageLocale.GetValue("catalog.oudebelcreditserror") + Item.OudeCredits);
                    return;
                }

                oudeCredits = oudeCredits - Item.OudeCredits;
                using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    dbClient.runFastQuery("UPDATE users SET belcredits = " + oudeCredits + " WHERE id = " + Session.GetHabbo().Id);
                }

                Session.SendNotif(LanguageLocale.GetValue("catalog.oudebelcreditsok") + oudeCredits);
            }

            //Console.WriteLine(Item.GetBaseItem().ItemId);
            //Console.WriteLine(Item.GetBaseItem().InteractionType.ToLower());
            // Extra Data is _NOT_ filtered at this point and MUST BE VERIFIED BELOW:
            switch (Item.GetBaseItem().InteractionType)
            {
                case InteractionType.none:
                    ExtraData = "";
                    break;

                case InteractionType.musicdisc:
                    ExtraData = Item.songID.ToString();
                    break;

                #region Pet handling
                case InteractionType.pet0:
                case InteractionType.pet1:
                case InteractionType.pet2:
                case InteractionType.pet3:
                case InteractionType.pet4:
                case InteractionType.pet5:
                case InteractionType.pet6:
                case InteractionType.pet7:
                case InteractionType.pet8:
                case InteractionType.pet9:
                case InteractionType.pet10:
                case InteractionType.pet11:
                case InteractionType.pet12:
                case InteractionType.pet14:
                case InteractionType.pet15:
                    try
                    {

                        //uint count = 0;
                        //using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                        //{
                        //    dbClient.setQuery("SELECT COUNT(*) FROM user_pets WHERE user_id = " + Session.GetHabbo().Id);
                        //    count = uint.Parse(dbClient.getString());
                        //}

                        //if (count > 5)
                        //{
                        //    Session.SendNotif(LanguageLocale.GetValue("catalog.pets.maxpets"));
                        //    return;
                        //}

                        string[] Bits = ExtraData.Split('\n');
                        string PetName = Bits[0];
                        string Race = Bits[1];
                        string Color = Bits[2];

                        int.Parse(Race); // to trigger any possible errors

                        if (!CheckPetName(PetName))
                            return;

                        if (Race.Length != 1)
                            return;

                        if (Color.Length != 6)
                            return;
                    }
                    catch (Exception e) { 
                        //Logging.WriteLine(e.ToString()); 
                        Logging.HandleException(e, "Catalog.HandleRowchase");
                        return; 
                    }

                    break;

                #endregion

                case InteractionType.roomeffect:

                    Double Number = 0;

                    try
                    {
                        if (string.IsNullOrEmpty(ExtraData))
                            Number = 0;
                        else
                            Number = Double.Parse(ExtraData, PiciEnvironment.cultureInfo);
                    }
                    catch (Exception e) { Logging.HandleException(e, "Catalog.HandleRowchase: " + ExtraData); }

                    ExtraData = Number.ToString().Replace(',', '.');
                    break; // maintain extra data // todo: validate

                case InteractionType.postit:
                    ExtraData = "FFFF33";
                    break;

                case InteractionType.dimmer:
                    ExtraData = "1,1,1,#000000,255";
                    break;

                case InteractionType.trophy:
                    ExtraData = Session.GetHabbo().Username + Convert.ToChar(9) + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + Convert.ToChar(9) + PiciEnvironment.FilterInjectionChars(ExtraData, true);
                    break;

                default:
                    ExtraData = "";
                    break;
            }

            if (Item.CreditsCost > 0)
            {
                Session.GetHabbo().Credits -= Item.CreditsCost;
                Session.GetHabbo().UpdateCreditsBalance();
            }

            if (Item.PixelsCost > 0)
            {
                Session.GetHabbo().ActivityPoints -= Item.PixelsCost;
                Session.GetHabbo().UpdateActivityPointsBalance(true);
            }

            Session.GetMessageHandler().GetResponse().Init(101);
            Session.GetMessageHandler().SendResponse();

            Session.GetMessageHandler().GetResponse().Init(67);
            Session.GetMessageHandler().GetResponse().AppendUInt(Item.GetBaseItem().ItemId);
            Session.GetMessageHandler().GetResponse().AppendStringWithBreak(Item.GetBaseItem().Name);
            Session.GetMessageHandler().GetResponse().AppendInt32(Item.CreditsCost);
            Session.GetMessageHandler().GetResponse().AppendInt32(Item.PixelsCost);
            Session.GetMessageHandler().GetResponse().AppendInt32(0);
            Session.GetMessageHandler().GetResponse().AppendInt32(1);
            Session.GetMessageHandler().GetResponse().AppendStringWithBreak(Item.GetBaseItem().Type.ToString().ToLower());
            Session.GetMessageHandler().GetResponse().AppendInt32(Item.GetBaseItem().SpriteId);
            Session.GetMessageHandler().GetResponse().AppendStringWithBreak("");
            Session.GetMessageHandler().GetResponse().AppendInt32(1);
            Session.GetMessageHandler().GetResponse().AppendInt32(0);
            Session.GetMessageHandler().SendResponse();

            if (IsGift)
            {
                uint itemID;
                //uint GenId = GenerateItemId();
                Item Present = GeneratePresent();

                using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    if (dbClient.dbType == DatabaseType.MSSQL)
                        dbClient.setQuery("INSERT INTO items (base_id) OUTPUT INSERTED.* VALUES (" + Present.ItemId + ")");
                    else
                        dbClient.setQuery("INSERT INTO items (base_id) VALUES (" + Present.ItemId + ")");
                    itemID = (uint)dbClient.insertQuery();

                    dbClient.runFastQuery("INSERT INTO items_users VALUES (" + itemID + "," + GiftUserId + ")");

                    if (!string.IsNullOrEmpty(GiftMessage))
                    {
                        dbClient.setQuery("INSERT INTO items_extradata VALUES (" + itemID + ",@data)");
                        dbClient.addParameter("data", GiftMessage);
                        dbClient.runQuery();
                    }

                    dbClient.setQuery("INSERT INTO user_presents (item_id,base_id,amount,extra_data) VALUES (" + itemID + "," + Item.GetBaseItem().ItemId + "," + Item.Amount + ",@extra_data)");
                    dbClient.addParameter("gift_message", "!" + GiftMessage);
                    dbClient.addParameter("extra_data", ExtraData);
                    dbClient.runQuery();
                }

                GameClient Receiver = PiciEnvironment.GetGame().GetClientManager().GetClientByUserID(GiftUserId);

                if (Receiver != null)
                {
                    Receiver.SendNotif(LanguageLocale.GetValue("catalog.gift.received") + Session.GetHabbo().Username);
                    Receiver.GetHabbo().GetInventoryComponent().AddNewItem(itemID, Present.ItemId, ExtraData, false, false, 0);
                    Receiver.GetHabbo().GetInventoryComponent().SendFloorInventoryUpdate();

                    InventoryComponent targetInventory = Receiver.GetHabbo().GetInventoryComponent();
                    if (targetInventory != null)
                        targetInventory.RunDBUpdate();
                }

                Session.SendNotif(LanguageLocale.GetValue("catalog.gift.sent"));
            }
            else
            {
                DeliverItems(Session, Item.GetBaseItem(), Item.Amount, ExtraData, Item.songID);
            }
        }

        internal static bool CheckPetName(string PetName)
        {
            if (PetName.Length < 1 || PetName.Length > 16)
            {
                return false;
            }

            if (!PiciEnvironment.IsValidAlphaNumeric(PetName))
            {
                return false;
            }

            return true;
        }

        internal void DeliverItems(GameClient Session, Item Item, int Amount, String ExtraData, uint songID = 0)
        {
            switch (Item.Type.ToString())
            {
                case "i":
                case "s":
                    for (int i = 0; i < Amount; i++)
                    {
                        //uint GeneratedId = GenerateItemId();
                        switch (Item.InteractionType)
                        {
                            case InteractionType.pet0:


                                string[] PetData = ExtraData.Split('\n');

                                Pet GeneratedPet = CreatePet(Session.GetHabbo().Id, PetData[0], 0, PetData[1], PetData[2]);

                                Session.GetHabbo().GetInventoryComponent().AddPet(GeneratedPet);
                                Session.GetHabbo().GetInventoryComponent().AddNewItem(0, 320, "0", true, false, 0);

                                break;

                            case InteractionType.pet1:

                                string[] PetData1 = ExtraData.Split('\n');

                                Pet GeneratedPet1 = CreatePet(Session.GetHabbo().Id, PetData1[0], 1, PetData1[1], PetData1[2]);

                                Session.GetHabbo().GetInventoryComponent().AddPet(GeneratedPet1);
                                Session.GetHabbo().GetInventoryComponent().AddNewItem(0, 320, "0", true, false, 0);

                                break;

                            case InteractionType.pet2:

                                string[] PetData5 = ExtraData.Split('\n');

                                Pet GeneratedPet5 = CreatePet(Session.GetHabbo().Id, PetData5[0], 2, PetData5[1], PetData5[2]);

                                Session.GetHabbo().GetInventoryComponent().AddPet(GeneratedPet5);
                                Session.GetHabbo().GetInventoryComponent().AddNewItem(0, 320, "0", true, false, 0);

                                break;

                            case InteractionType.pet3:

                                string[] PetData2 = ExtraData.Split('\n');

                                Pet GeneratedPet2 = CreatePet(Session.GetHabbo().Id, PetData2[0], 3, PetData2[1], PetData2[2]);

                                Session.GetHabbo().GetInventoryComponent().AddPet(GeneratedPet2);
                                Session.GetHabbo().GetInventoryComponent().AddNewItem(0, 320, "0", true, false, 0);

                                break;

                            case InteractionType.pet4:

                                string[] PetData3 = ExtraData.Split('\n');

                                Pet GeneratedPet3 = CreatePet(Session.GetHabbo().Id, PetData3[0], 4, PetData3[1], PetData3[2]);

                                Session.GetHabbo().GetInventoryComponent().AddPet(GeneratedPet3);
                                Session.GetHabbo().GetInventoryComponent().AddNewItem(0, 320, "0", true, false, 0);

                                break;

                            case InteractionType.pet5:

                                string[] PetData7 = ExtraData.Split('\n');

                                Pet GeneratedPet7 = CreatePet(Session.GetHabbo().Id, PetData7[0], 5, PetData7[1], PetData7[2]);

                                Session.GetHabbo().GetInventoryComponent().AddPet(GeneratedPet7);
                                Session.GetHabbo().GetInventoryComponent().AddNewItem(0, 320, "0", true, false, 0);

                                break;

                            case InteractionType.pet6:
                                string[] PetData4 = ExtraData.Split('\n');

                                Pet GeneratedPet4 = CreatePet(Session.GetHabbo().Id, PetData4[0], 6, PetData4[1], PetData4[2]);

                                Session.GetHabbo().GetInventoryComponent().AddPet(GeneratedPet4);
                                Session.GetHabbo().GetInventoryComponent().AddNewItem(0, 320, "0", true, false, 0);

                                break;

                            case InteractionType.pet7:

                                string[] PetData6 = ExtraData.Split('\n');

                                Pet GeneratedPet6 = CreatePet(Session.GetHabbo().Id, PetData6[0], 7, PetData6[1], PetData6[2]);

                                Session.GetHabbo().GetInventoryComponent().AddPet(GeneratedPet6);
                                Session.GetHabbo().GetInventoryComponent().AddNewItem(0, 320, "0", true, false, 0);

                                break;

                            case InteractionType.pet8:

                                string[] PetData8 = ExtraData.Split('\n');

                                Pet GeneratedPet8 = CreatePet(Session.GetHabbo().Id, PetData8[0], 8, PetData8[1], PetData8[2]);

                                Session.GetHabbo().GetInventoryComponent().AddPet(GeneratedPet8);
                                Session.GetHabbo().GetInventoryComponent().AddNewItem(0, 320, "0", true, false, 0);

                                break;

                            case InteractionType.pet9:

                                string[] PetData9 = ExtraData.Split('\n');

                                Pet GeneratedPet9 = CreatePet(Session.GetHabbo().Id, PetData9[0], 9, PetData9[1], PetData9[2]);

                                Session.GetHabbo().GetInventoryComponent().AddPet(GeneratedPet9);
                                Session.GetHabbo().GetInventoryComponent().AddNewItem(0, 320, "0", true, false, 0);

                                break;
                            case InteractionType.pet10:

                                string[] PetData10 = ExtraData.Split('\n');

                                Pet GeneratedPet10 = CreatePet(Session.GetHabbo().Id, PetData10[0], 10, PetData10[1], PetData10[2]);

                                Session.GetHabbo().GetInventoryComponent().AddPet(GeneratedPet10);
                                Session.GetHabbo().GetInventoryComponent().AddNewItem(0, 320, "0", true, false, 0);

                                break;
                            case InteractionType.pet11:

                                string[] PetData11 = ExtraData.Split('\n');

                                Pet GeneratedPet11 = CreatePet(Session.GetHabbo().Id, PetData11[0], 11, PetData11[1], PetData11[2]);

                                Session.GetHabbo().GetInventoryComponent().AddPet(GeneratedPet11);
                                Session.GetHabbo().GetInventoryComponent().AddNewItem(0, 320, "0", true, false, 0);

                                break;
                            case InteractionType.pet12:
                                string[] PetData12 = ExtraData.Split('\n');

                                Pet GeneratedPet12 = CreatePet(Session.GetHabbo().Id, PetData12[0], 12, PetData12[1], PetData12[2]);

                                Session.GetHabbo().GetInventoryComponent().AddPet(GeneratedPet12);
                                Session.GetHabbo().GetInventoryComponent().AddNewItem(0, 320, "0", true, false, 0);

                                break;
                            case InteractionType.pet14:
                                string[] PetData14 = ExtraData.Split('\n');

                                Pet GeneratedPet14 = CreatePet(Session.GetHabbo().Id, PetData14[0], 14, PetData14[1], PetData14[2]);

                                Session.GetHabbo().GetInventoryComponent().AddPet(GeneratedPet14);
                                Session.GetHabbo().GetInventoryComponent().AddNewItem(0, 320, "0", true, false, 0);

                                break;
                            case InteractionType.pet15:
                                string[] PetData15 = ExtraData.Split('\n');

                                Pet GeneratedPet15 = CreatePet(Session.GetHabbo().Id, PetData15[0], 15, PetData15[1], PetData15[2]);

                                Session.GetHabbo().GetInventoryComponent().AddPet(GeneratedPet15);
                                Session.GetHabbo().GetInventoryComponent().AddNewItem(0, 320, "0", true, false, 0);

                                break;
                            case InteractionType.teleport:

                                uint idOne = Session.GetHabbo().GetInventoryComponent().AddNewItem(0, Item.ItemId, "0", true, false, 0).Id;
                                uint idTwo = Session.GetHabbo().GetInventoryComponent().AddNewItem(0, Item.ItemId, "0", true, false, 0).Id;

                                using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
                                {
                                    dbClient.runFastQuery("INSERT INTO items_tele_links (tele_one_id,tele_two_id) VALUES (" + idOne + "," + idTwo + ")");
                                    dbClient.runFastQuery("INSERT INTO items_tele_links (tele_one_id,tele_two_id) VALUES (" + idTwo + "," + idOne + ")");
                                }

                                break;

                            case InteractionType.dimmer:

                                uint id = Session.GetHabbo().GetInventoryComponent().AddNewItem(0, Item.ItemId, ExtraData, true, false, 0).Id;
                                using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
                                {
                                    dbClient.runFastQuery("INSERT INTO items_moodlight (item_id,enabled,current_preset,preset_one,preset_two,preset_three) VALUES (" + id + ",0,1,'#000000,255,0','#000000,255,0','#000000,255,0')");
                                }


                                break;

                            case InteractionType.musicdisc:
                                {
                                    Session.GetHabbo().GetInventoryComponent().AddNewItem(0, Item.ItemId, songID.ToString(), true, false, songID);
                                    break;
                                }

                            default:

                                Session.GetHabbo().GetInventoryComponent().AddNewItem(0, Item.ItemId, ExtraData, true, false, songID);
                                break;
                        }
                    }

                    Session.GetHabbo().GetInventoryComponent().UpdateItems(false);
                    break;

                case "e":

                    for (int i = 0; i < Amount; i++)
                    {
                        Session.GetHabbo().GetAvatarEffectsInventoryComponent().AddEffect(Item.SpriteId, 3600);
                    }

                    break;

                case "h":

                    for (int i = 0; i < Amount; i++)
                    {
                        Session.GetHabbo().GetSubscriptionManager().AddOrExtendSubscription("habbo_club", 2678400);
                    }

                    if (!Session.GetHabbo().GetBadgeComponent().HasBadge("HC1"))
                    {
                        Session.GetHabbo().GetBadgeComponent().GiveBadge("HC1", true);
                    }

                    Session.GetMessageHandler().GetResponse().Init(7);
                    Session.GetMessageHandler().GetResponse().AppendStringWithBreak("habbo_club");

                    if (Session.GetHabbo().GetSubscriptionManager().HasSubscription("habbo_club"))
                    {
                        Double Expire = Session.GetHabbo().GetSubscriptionManager().GetSubscription("habbo_club").ExpireTime;
                        Double TimeLeft = Expire - PiciEnvironment.GetUnixTimestamp();
                        int TotalDaysLeft = (int)Math.Ceiling(TimeLeft / 86400);
                        int MonthsLeft = TotalDaysLeft / 31;

                        if (MonthsLeft >= 1) MonthsLeft--;

                        Session.GetMessageHandler().GetResponse().AppendInt32(TotalDaysLeft - (MonthsLeft * 31));
                        Session.GetMessageHandler().GetResponse().AppendBoolean(true);
                        Session.GetMessageHandler().GetResponse().AppendInt32(MonthsLeft);
                    }
                    else
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            Session.GetMessageHandler().GetResponse().AppendInt32(0);
                        }
                    }

                    Session.GetMessageHandler().SendResponse();

                    //List<string> Rights = PiciEnvironment.GetGame().GetRoleManager().GetRightsForHabbo(Session.GetHabbo());

                    Session.GetMessageHandler().GetResponse().Init(2);
                    Session.GetMessageHandler().GetResponse().AppendInt32(2);

                    if (Session.GetHabbo().HasRight("acc_anyroomowner"))
                        Session.GetMessageHandler().GetResponse().AppendInt32(7);
                    else if (Session.GetHabbo().HasRight("acc_anyroomrights"))
                        Session.GetMessageHandler().GetResponse().AppendInt32(5);
                    else if (Session.GetHabbo().HasRight("acc_supporttool"))
                        Session.GetMessageHandler().GetResponse().AppendInt32(4);
                    else
                        Session.GetMessageHandler().GetResponse().AppendInt32(0);

                    Session.GetMessageHandler().SendResponse();
                    PiciEnvironment.GetGame().GetAchievementManager().ProgressUserAchievement(Session, "ACH_BasicClub", 1); //ACH_VipClub
                    break;

                default:

                    Session.SendNotif(LanguageLocale.GetValue("catalog.buyerror"));
                    break;
            }
        }

        internal Item GeneratePresent()
        {
            int count = gifts.Count;

            int countID = PiciEnvironment.GetRandomNumber(0, count);
            int countAmount = 0;

            if (count == 0)
                return null;

            foreach (Item item in gifts.Values)
            {
                if (item == null)
                    continue;
                if (countAmount == countID)
                {
                    return item;
                }

                countAmount++;
            }

            return null;
        }

        internal static Pet CreatePet(uint UserId, string Name, int Type, string Race, string Color)
        {
            Pet pet = new Pet(404, UserId, 0, Name, (uint)Type, Race, Color, 0, 100, 100, 0, PiciEnvironment.GetUnixTimestamp(), 0, 0, 0.0);
            pet.DBState = DatabaseUpdateState.NeedsUpdate;

            using (IQueryAdapter dbClient = PiciEnvironment.GetDatabaseManager().getQueryreactor())
            {
                if (dbClient.dbType == DatabaseType.MSSQL)
                    dbClient.setQuery("INSERT INTO user_pets (user_id,name,type,race,color,expirience,energy,createstamp,nutrition,respect,x,y,z) OUTPUT INSERTED.* VALUES (" + pet.OwnerId + ",@" + pet.PetId + "name," + pet.Type + ",@" + pet.PetId + "race,@" + pet.PetId + "color,0,100,'" + pet.CreationStamp + "',0,0,0,0,0)");
                else
                    dbClient.setQuery("INSERT INTO user_pets (user_id,name,type,race,color,expirience,energy,createstamp) VALUES (" + pet.OwnerId + ",@" + pet.PetId + "name," + pet.Type + ",@" + pet.PetId + "race,@" + pet.PetId + "color,0,100,'" + pet.CreationStamp + "')");
                dbClient.addParameter(pet.PetId + "name", pet.Name);
                dbClient.addParameter(pet.PetId + "race", pet.Race);
                dbClient.addParameter(pet.PetId + "color", pet.Color);
                pet.PetId = (uint)dbClient.insertQuery();
            }
            return pet;
        }

        internal static Pet GeneratePetFromRow(DataRow Row)
        {
            if (Row == null)
            {
                return null;
            }

            return new Pet(Convert.ToUInt32(Row["id"]), Convert.ToUInt32(Row["user_id"]), Convert.ToUInt32(Row["room_id"]), (string)Row["name"], Convert.ToUInt32(Row["type"]), (string)Row["race"], (string)Row["color"], (int)Row["expirience"], (int)Row["energy"], (int)Row["nutrition"], (int)Row["respect"], (double)Row["createstamp"], (int)Row["x"], (int)Row["y"], (double)Row["z"]);
        }

        //internal Pet GeneratePetFromRow(DataRow Row, uint PetID)
        //{
        //    if (Row == null)
        //        return null;

        //    return new Pet(PetID, (uint)Row["user_id"], (uint)Row["room_id"], (string)Row["name"], (uint)Row["type"], (string)Row["race"], (string)Row["color"], (int)Row["expirience"], (int)Row["energy"], (int)Row["nutrition"], (int)Row["respect"], (double)Row["createstamp"], (int)Row["x"], (int)Row["y"], (double)Row["z"]);
        //}

        //internal uint GenerateItemId()
        //{
        //    //uint i = 0;

        //    //using (DatabaseClient dbClient = ButterflyEnvironment.GetDatabase().GetClient())
        //    //{
        //    //    i = mCacheID++;
        //    //    dbClient.runFastQuery("UPDATE item_id_generator SET id_generator = '" + mCacheID + "' LIMIT 1");
        //    //}

        //    return mCacheID++;
        //}

        internal EcotronReward GetRandomEcotronReward()
        {
            uint Level = 1;

            if (PiciEnvironment.GetRandomNumber(1, 2000) == 2000)
            {
                Level = 5;
            }
            else if (PiciEnvironment.GetRandomNumber(1, 200) == 200)
            {
                Level = 4;
            }
            else if (PiciEnvironment.GetRandomNumber(1, 40) == 40)
            {
                Level = 3;
            }
            else if (PiciEnvironment.GetRandomNumber(1, 4) == 4)
            {
                Level = 2;
            }

            List<EcotronReward> PossibleRewards = GetEcotronRewardsForLevel(Level);

            if (PossibleRewards != null && PossibleRewards.Count >= 1)
            {
                return PossibleRewards[PiciEnvironment.GetRandomNumber(0, (PossibleRewards.Count - 1))];
            }
            else
            {
                return new EcotronReward(0, 1479, 0); // eco lamp two :D
            }
        }

        internal List<EcotronReward> GetEcotronRewardsForLevel(uint Level)
        {
            List<EcotronReward> Rewards = new List<EcotronReward>();

            foreach (EcotronReward R in EcotronRewards)
            {
                if (R.RewardLevel == Level)
                {
                    Rewards.Add(R);
                }
            }


            return Rewards;
        }

        internal ServerMessage SerializeIndexForCache(int rank)
        {
            //ServerMessage Index = new ServerMessage(126);
            //Index.AppendBoolean(false);
            //Index.Append(0);
            //Index.Append(0);
            //Index.Append(-1);
            //Index.Append("");
            //Index.AppendBoolean(false);
            ServerMessage Index = new ServerMessage(126); //Fix for r61
            Index.AppendStringWithBreak("IHHM");
            Index.AppendInt32(GetTreeSize(rank, -1));

            foreach (CatalogPage Page in Pages.Values)
            {
                if (Page.ParentId != -1 || Page.MinRank > rank)
                    continue;

                Page.Serialize(rank, Index);

                foreach (CatalogPage _Page in Pages.Values)
                {
                    if (_Page.ParentId != Page.PageId)
                        continue;

                    _Page.Serialize(rank, Index);
                }
            }

            return Index;
        }

        internal ServerMessage GetIndexMessageForRank(uint Rank)
        {
            if (Rank < 1)
                Rank = 1;
            if (Rank > 7)
                Rank = 7;

            return mCataIndexCache[Rank];
        }

        internal static ServerMessage SerializePage(CatalogPage Page)
        {
            ServerMessage PageData = new ServerMessage(127);
            PageData.AppendInt32(Page.PageId);

            switch (Page.Layout)
            {
                case "frontpage":

                    PageData.AppendStringWithBreak("frontpage3");
                    PageData.AppendInt32(3);
                    PageData.AppendStringWithBreak(Page.LayoutHeadline);
                    PageData.AppendStringWithBreak(Page.LayoutTeaser);
                    PageData.AppendStringWithBreak("");
                    PageData.AppendInt32(11);
                    PageData.AppendStringWithBreak(Page.Text1);
                    PageData.AppendStringWithBreak("");
                    PageData.AppendStringWithBreak(Page.Text2);
                    PageData.AppendStringWithBreak(Page.TextDetails);
                    PageData.AppendStringWithBreak("");
                    PageData.AppendStringWithBreak("#FAF8CC");
                    PageData.AppendStringWithBreak("#FAF8CC");
                    PageData.AppendStringWithBreak(LanguageLocale.GetValue("catalog.waystogetcredits"));
                    PageData.AppendStringWithBreak("magic.credits");

                    break;

                case "recycler_info":

                    PageData.AppendStringWithBreak(Page.Layout);
                    PageData.AppendInt32(2);
                    PageData.AppendStringWithBreak(Page.LayoutHeadline);
                    PageData.AppendStringWithBreak(Page.LayoutTeaser);
                    PageData.AppendInt32(3);
                    PageData.AppendStringWithBreak(Page.Text1);
                    PageData.AppendStringWithBreak(Page.Text2);
                    PageData.AppendStringWithBreak(Page.TextDetails);

                    break;

                case "recycler_prizes":

                    // Ac@aArecycler_prizesIcatalog_recycler_headline3IDe Ecotron geeft altijd een van deze beloningen:H
                    PageData.AppendStringWithBreak("recycler_prizes");
                    PageData.AppendInt32(1);
                    PageData.AppendStringWithBreak("catalog_recycler_headline3");
                    PageData.AppendInt32(1);
                    PageData.AppendStringWithBreak(Page.Text1);

                    break;

                case "spaces_new":

                    PageData.AppendStringWithBreak(Page.Layout);
                    PageData.AppendInt32(1);
                    PageData.AppendStringWithBreak(Page.LayoutHeadline);
                    PageData.AppendInt32(1);
                    PageData.AppendStringWithBreak(Page.Text1);

                    break;

                case "recycler":

                    PageData.AppendStringWithBreak(Page.Layout);
                    PageData.AppendInt32(2);
                    PageData.AppendStringWithBreak(Page.LayoutHeadline);
                    PageData.AppendStringWithBreak(Page.LayoutTeaser);
                    PageData.AppendInt32(1);
                    PageData.AppendStringWithBreak(Page.Text1, 10);
                    PageData.AppendStringWithBreak(Page.Text2);
                    PageData.AppendStringWithBreak(Page.TextDetails);

                    break;

                case "trophies":

                    PageData.AppendStringWithBreak("trophies");
                    PageData.AppendInt32(1);
                    PageData.AppendStringWithBreak(Page.LayoutHeadline);
                    PageData.AppendInt32(2);
                    PageData.AppendStringWithBreak(Page.Text1);
                    PageData.AppendStringWithBreak(Page.TextDetails);

                    break;

                case "pets":

                    PageData.AppendStringWithBreak("pets");
                    PageData.AppendInt32(2);
                    PageData.AppendStringWithBreak(Page.LayoutHeadline);
                    PageData.AppendStringWithBreak(Page.LayoutTeaser);
                    PageData.AppendInt32(4);
                    PageData.AppendStringWithBreak(Page.Text1);
                    PageData.AppendStringWithBreak(LanguageLocale.GetValue("catalog.pickname"));
                    PageData.AppendStringWithBreak(LanguageLocale.GetValue("catalog.pickcolor"));
                    PageData.AppendStringWithBreak(LanguageLocale.GetValue("catalog.pickrace"));

                    break;

                case "soundmachine":

                    PageData.AppendStringWithBreak(Page.Layout);
                    PageData.AppendInt32(2);
                    PageData.AppendStringWithBreak(Page.LayoutHeadline);
                    PageData.AppendStringWithBreak(Page.LayoutTeaser);
                    PageData.AppendInt32(2);
                    PageData.AppendStringWithBreak(Page.Text1);
                    PageData.AppendStringWithBreak(Page.TextDetails);
                    break;


                default:

                    PageData.AppendStringWithBreak(Page.Layout);
                    PageData.AppendInt32(3);
                    PageData.AppendStringWithBreak(Page.LayoutHeadline);
                    PageData.AppendStringWithBreak(Page.LayoutTeaser);
                    PageData.AppendStringWithBreak(Page.LayoutSpecial);
                    PageData.AppendInt32(3);
                    PageData.AppendStringWithBreak(Page.Text1);
                    PageData.AppendStringWithBreak(Page.TextDetails);
                    PageData.AppendStringWithBreak(Page.TextTeaser);

                    break;
            }

            PageData.AppendInt32(Page.Items.Count);

            foreach (CatalogItem Item in Page.Items.Values)
            {
                Item.Serialize(PageData);
            }

            PageData.AppendInt32(-1);

            return PageData;
        }

        //internal ServerMessage SerializeTestIndex()
        //{
        //    ServerMessage Message = new ServerMessage(126);

        //    Message.Append(0);
        //    Message.Append(0);
        //    Message.Append(0);
        //    Message.Append(-1);
        //    Message.Append("");
        //    Message.Append(0);
        //    Message.Append(100);

        //    for (int i = 1; i <= 150; i++)
        //    {
        //        Message.Append(1);
        //        Message.Append(i);
        //        Message.Append(i);
        //        Message.Append(i);
        //        Message.Append("#" + i);
        //        Message.Append(0);
        //        Message.Append(0);
        //    }

        //    return Message;

        //   
        //}

        //internal VoucherHandler GetVoucherHandler()
        //{
        //    return VoucherHandler;
        //}

        internal Marketplace GetMarketplace()
        {
            return Marketplace;
        }
    }
}
