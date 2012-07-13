using System;
using System.Collections.Generic;
using System.Data;
using Pici.HabboHotel.Users;
using Pici.Storage.Database.Session_Details.Interfaces;
using Pici.Util;

namespace Pici.HabboHotel.Roles
{
    class RoleManager
    {
        //private Dictionary<uint, Role> Roles;
        //private Dictionary<string, uint> Rights;
        //private Dictionary<string, string> SubRights;
        private Dictionary<uint, string> Ranks = new Dictionary<uint, string>();
        private Dictionary<uint, List<string>> PermissionsUsersD = new Dictionary<uint, List<string>>();
        private Dictionary<uint, List<string>> PermissionsRanksD = new Dictionary<uint, List<string>>();
        private Dictionary<uint, int> PermissionsUsersR = new Dictionary<uint, int>();

        //internal RoleManager()
        //{
            //Roles = new Dictionary<uint,Role>();
            //Rights = new Dictionary<string, uint>();
            //SubRights = new Dictionary<string, string>();
        //}
    
        //internal void LoadRoles(DatabaseClient dbClient)
        //{
        //    ClearRoles();

        //    DataTable Data = dbClient.getTable("SELECT * FROM ranks ORDER BY id ASC;");

        //    if (Data != null)
        //    {
        //        foreach (DataRow Row in Data.Rows)
        //        {
        //            Roles.Add((uint)pur["id"], new Role((uint)pur["id"], (string)pur["name"]));
        //        }
        //    }
        //}

        internal void LoadRights(IQueryAdapter dbClient)
        {
            ClearRights();

            /*            DataTable table = class1_0.method_3("SELECT * FROM ranks ORDER BY id ASC;");
            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    this.dictionary_2.Add((uint) pur["id"], pur["badgeid"].ToString());
                }
            }*/
            List<string> list;
            dbClient.setQuery("SELECT * FROM ranks ORDER BY id ASC;");
            DataTable Ranks = dbClient.getTable();

            if (Ranks != null)
            {
                foreach (DataRow RankRow in Ranks.Rows)
                {
                    this.Ranks.Add((uint)RankRow["id"], RankRow["badgeid"].ToString());
                }
            }

            dbClient.setQuery("SELECT * FROM permissions_users ORDER BY userid ASC;");
            DataTable PermissionsUsers = dbClient.getTable();
            foreach (DataRow pur in PermissionsUsers.Rows)
            {
                list = new List<string>();

                if (PiciEnvironment.EnumToBool(pur["cmd_update_settings"].ToString()))
                {
                    list.Add("cmd_update_settings");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_update_bans"].ToString()))
                {
                    list.Add("cmd_update_bans");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_update_bots"].ToString()))
                {
                    list.Add("cmd_update_bots");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_update_catalogue"].ToString()))
                {
                    list.Add("cmd_update_catalogue");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_update_navigator"].ToString()))
                {
                    list.Add("cmd_update_navigator");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_update_items"].ToString()))
                {
                    list.Add("cmd_update_items");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_award"].ToString()))
                {
                    list.Add("cmd_award");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_coords"].ToString()))
                {
                    list.Add("cmd_coords");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_override"].ToString()))
                {
                    list.Add("cmd_override");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_coins"].ToString()))
                {
                    list.Add("cmd_coins");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_pixels"].ToString()))
                {
                    list.Add("cmd_pixels");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_ha"].ToString()))
                {
                    list.Add("cmd_ha");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_hal"].ToString()))
                {
                    list.Add("cmd_hal");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_freeze"].ToString()))
                {
                    list.Add("cmd_freeze");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_enable"].ToString()))
                {
                    list.Add("cmd_enable");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_roommute"].ToString()))
                {
                    list.Add("cmd_roommute");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_setspeed"].ToString()))
                {
                    list.Add("cmd_setspeed");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_masscredits"].ToString()))
                {
                    list.Add("cmd_masscredits");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_globalcredits"].ToString()))
                {
                    list.Add("cmd_globalcredits");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_masspixels"].ToString()))
                {
                    list.Add("cmd_masspixels");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_globalpixels"].ToString()))
                {
                    list.Add("cmd_globalpixels");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_roombadge"].ToString()))
                {
                    list.Add("cmd_roombadge");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_massbadge"].ToString()))
                {
                    list.Add("cmd_massbadge");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_userinfo"].ToString()))
                {
                    list.Add("cmd_userinfo");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_shutdown"].ToString()))
                {
                    list.Add("cmd_shutdown");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_givebadge"].ToString()))
                {
                    list.Add("cmd_givebadge");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_removebadge"].ToString()))
                {
                    list.Add("cmd_removebadge");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_summon"].ToString()))
                {
                    list.Add("cmd_summon");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_invisible"].ToString()))
                {
                    list.Add("cmd_invisible");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_ban"].ToString()))
                {
                    list.Add("cmd_ban");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_superban"].ToString()))
                {
                    list.Add("cmd_superban");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_roomkick"].ToString()))
                {
                    list.Add("cmd_roomkick");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_roomalert"].ToString()))
                {
                    list.Add("cmd_roomalert");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_mute"].ToString()))
                {
                    list.Add("cmd_mute");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_unmute"].ToString()))
                {
                    list.Add("cmd_unmute");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_alert"].ToString()))
                {
                    list.Add("cmd_alert");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_motd"].ToString()))
                {
                    list.Add("cmd_motd");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_kick"].ToString()))
                {
                    list.Add("cmd_kick");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_update_filter"].ToString()))
                {
                    list.Add("cmd_update_filter");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_update_permissions"].ToString()))
                {
                    list.Add("cmd_update_permissions");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_sa"].ToString()))
                {
                    list.Add("cmd_sa");
                }
                if (PiciEnvironment.EnumToBool(pur["receive_sa"].ToString()))
                {
                    list.Add("receive_sa");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_ipban"].ToString()))
                {
                    list.Add("cmd_ipban");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_spull"].ToString()))
                {
                    list.Add("cmd_spull");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_disconnect"].ToString()))
                {
                    list.Add("cmd_disconnect");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_update_achievements"].ToString()))
                {
                    list.Add("cmd_update_achievements");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_update_texts"].ToString()))
                {
                    list.Add("cmd_update_texts");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_teleport"].ToString()))
                {
                    list.Add("cmd_teleport");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_points"].ToString()))
                {
                    list.Add("cmd_points");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_masspoints"].ToString()))
                {
                    list.Add("cmd_masspoints");
                }
                if (PiciEnvironment.EnumToBool(pur["cmd_globalpoints"].ToString()))
                {
                    list.Add("cmd_globalpoints");
                }
                if (PiciEnvironment.EnumToBool(pur["acc_anyroomrights"].ToString()))
                {
                    list.Add("acc_anyroomrights");
                }
                if (PiciEnvironment.EnumToBool(pur["acc_anyroomowner"].ToString()))
                {
                    list.Add("acc_anyroomowner");
                }
                if (PiciEnvironment.EnumToBool(pur["acc_supporttool"].ToString()))
                {
                    list.Add("acc_supporttool");
                }
                if (PiciEnvironment.EnumToBool(pur["acc_chatlogs"].ToString()))
                {
                    list.Add("acc_chatlogs");
                }
                if (PiciEnvironment.EnumToBool(pur["acc_enter_fullrooms"].ToString()))
                {
                    list.Add("acc_enter_fullrooms");
                }
                if (PiciEnvironment.EnumToBool(pur["acc_enter_anyroom"].ToString()))
                {
                    list.Add("acc_enter_anyroom");
                }
                if (PiciEnvironment.EnumToBool(pur["acc_restrictedrooms"].ToString()))
                {
                    list.Add("acc_restrictedrooms");
                }
                if (PiciEnvironment.EnumToBool(pur["acc_unkickable"].ToString()))
                {
                    list.Add("acc_unkickable");
                }
                if (PiciEnvironment.EnumToBool(pur["acc_unbannable"].ToString()))
                {
                    list.Add("acc_unbannable");
                }
                if (PiciEnvironment.EnumToBool(pur["ignore_friendsettings"].ToString()))
                {
                    list.Add("ignore_friendsettings");
                }
                this.PermissionsUsersD.Add((uint)pur["userid"], list);
            }

            dbClient.setQuery("SELECT * FROM permissions_ranks ORDER BY rank ASC;");
            DataTable PermissionsRanks = dbClient.getTable();
            if (PermissionsRanks != null)
            {
                foreach (DataRow pur1 in PermissionsRanks.Rows)
                {
                    this.PermissionsUsersR.Add((uint) pur1["rank"], (int) pur1["floodtime"]);
                }
                foreach (DataRow pur in PermissionsRanks.Rows)
                {
                    list = new List<string>();
                    if (PiciEnvironment.EnumToBool(pur["cmd_update_settings"].ToString()))
                    {
                        list.Add("cmd_update_settings");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_update_bans"].ToString()))
                    {
                        list.Add("cmd_update_bans");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_update_bots"].ToString()))
                    {
                        list.Add("cmd_update_bots");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_update_catalogue"].ToString()))
                    {
                        list.Add("cmd_update_catalogue");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_update_navigator"].ToString()))
                    {
                        list.Add("cmd_update_navigator");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_update_items"].ToString()))
                    {
                        list.Add("cmd_update_items");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_award"].ToString()))
                    {
                        list.Add("cmd_award");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_coords"].ToString()))
                    {
                        list.Add("cmd_coords");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_override"].ToString()))
                    {
                        list.Add("cmd_override");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_coins"].ToString()))
                    {
                        list.Add("cmd_coins");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_pixels"].ToString()))
                    {
                        list.Add("cmd_pixels");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_ha"].ToString()))
                    {
                        list.Add("cmd_ha");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_hal"].ToString()))
                    {
                        list.Add("cmd_hal");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_freeze"].ToString()))
                    {
                        list.Add("cmd_freeze");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_enable"].ToString()))
                    {
                        list.Add("cmd_enable");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_roommute"].ToString()))
                    {
                        list.Add("cmd_roommute");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_setspeed"].ToString()))
                    {
                        list.Add("cmd_setspeed");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_masscredits"].ToString()))
                    {
                        list.Add("cmd_masscredits");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_globalcredits"].ToString()))
                    {
                        list.Add("cmd_globalcredits");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_masspixels"].ToString()))
                    {
                        list.Add("cmd_masspixels");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_globalpixels"].ToString()))
                    {
                        list.Add("cmd_globalpixels");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_roombadge"].ToString()))
                    {
                        list.Add("cmd_roombadge");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_massbadge"].ToString()))
                    {
                        list.Add("cmd_massbadge");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_userinfo"].ToString()))
                    {
                        list.Add("cmd_userinfo");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_shutdown"].ToString()))
                    {
                        list.Add("cmd_shutdown");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_givebadge"].ToString()))
                    {
                        list.Add("cmd_givebadge");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_removebadge"].ToString()))
                    {
                        list.Add("cmd_removebadge");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_summon"].ToString()))
                    {
                        list.Add("cmd_summon");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_invisible"].ToString()))
                    {
                        list.Add("cmd_invisible");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_ban"].ToString()))
                    {
                        list.Add("cmd_ban");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_superban"].ToString()))
                    {
                        list.Add("cmd_superban");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_roomkick"].ToString()))
                    {
                        list.Add("cmd_roomkick");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_roomalert"].ToString()))
                    {
                        list.Add("cmd_roomalert");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_mute"].ToString()))
                    {
                        list.Add("cmd_mute");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_unmute"].ToString()))
                    {
                        list.Add("cmd_unmute");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_alert"].ToString()))
                    {
                        list.Add("cmd_alert");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_motd"].ToString()))
                    {
                        list.Add("cmd_motd");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_kick"].ToString()))
                    {
                        list.Add("cmd_kick");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_update_filter"].ToString()))
                    {
                        list.Add("cmd_update_filter");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_update_permissions"].ToString()))
                    {
                        list.Add("cmd_update_permissions");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_sa"].ToString()))
                    {
                        list.Add("cmd_sa");
                    }
                    if (PiciEnvironment.EnumToBool(pur["receive_sa"].ToString()))
                    {
                        list.Add("receive_sa");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_ipban"].ToString()))
                    {
                        list.Add("cmd_ipban");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_spull"].ToString()))
                    {
                        list.Add("cmd_spull");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_disconnect"].ToString()))
                    {
                        list.Add("cmd_disconnect");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_update_achievements"].ToString()))
                    {
                        list.Add("cmd_update_achievements");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_update_texts"].ToString()))
                    {
                        list.Add("cmd_update_texts");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_teleport"].ToString()))
                    {
                        list.Add("cmd_teleport");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_points"].ToString()))
                    {
                        list.Add("cmd_points");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_masspoints"].ToString()))
                    {
                        list.Add("cmd_masspoints");
                    }
                    if (PiciEnvironment.EnumToBool(pur["cmd_globalpoints"].ToString()))
                    {
                        list.Add("cmd_globalpoints");
                    }
                    if (PiciEnvironment.EnumToBool(pur["acc_anyroomrights"].ToString()))
                    {
                        list.Add("acc_anyroomrights");
                    }
                    if (PiciEnvironment.EnumToBool(pur["acc_anyroomowner"].ToString()))
                    {
                        list.Add("acc_anyroomowner");
                    }
                    if (PiciEnvironment.EnumToBool(pur["acc_supporttool"].ToString()))
                    {
                        list.Add("acc_supporttool");
                    }
                    if (PiciEnvironment.EnumToBool(pur["acc_chatlogs"].ToString()))
                    {
                        list.Add("acc_chatlogs");
                    }
                    if (PiciEnvironment.EnumToBool(pur["acc_enter_fullrooms"].ToString()))
                    {
                        list.Add("acc_enter_fullrooms");
                    }
                    if (PiciEnvironment.EnumToBool(pur["acc_enter_anyroom"].ToString()))
                    {
                        list.Add("acc_enter_anyroom");
                    }
                    if (PiciEnvironment.EnumToBool(pur["acc_restrictedrooms"].ToString()))
                    {
                        list.Add("acc_restrictedrooms");
                    }
                    if (PiciEnvironment.EnumToBool(pur["acc_unkickable"].ToString()))
                    {
                        list.Add("acc_unkickable");
                    }
                    if (PiciEnvironment.EnumToBool(pur["acc_unbannable"].ToString()))
                    {
                        list.Add("acc_unbannable");
                    }
                    if (PiciEnvironment.EnumToBool(pur["ignore_friendsettings"].ToString()))
                    {
                        list.Add("ignore_friendsettings");
                    }

                    this.PermissionsRanksD.Add((uint)pur["rank"], list);
                }


                dbClient.setQuery("SELECT * FROM permissions_vip;");
                DataTable table = dbClient.getTable();
                if (table != null)
                {
                    Class19.Boolean_8 = false;
                    Class19.Boolean_10 = false;
                    Class19.Boolean_11 = false;
                    Class19.Boolean_12 = false;
                    Class19.Boolean_13 = false;
                    Class19.Boolean_9 = false;
                    foreach (DataRow row in table.Rows)
                    {
                        if (PiciEnvironment.EnumToBool(row["cmdPush"].ToString()))
                        {
                            Class19.Boolean_8 = true;
                        }
                        if (PiciEnvironment.EnumToBool(row["cmdPull"].ToString()))
                        {
                            Class19.Boolean_10 = true;
                        }
                        if (PiciEnvironment.EnumToBool(row["cmdFlagme"].ToString()))
                        {
                            Class19.Boolean_11 = true;
                        }
                        if (PiciEnvironment.EnumToBool(row["cmdMimic"].ToString()))
                        {
                            Class19.Boolean_12 = true;
                        }
                        if (PiciEnvironment.EnumToBool(row["cmdMoonwalk"].ToString()))
                        {
                            Class19.Boolean_13 = true;
                        }
                        if (PiciEnvironment.EnumToBool(row["cmdFollow"].ToString()))
                        {
                            Class19.Boolean_9 = true;
                        }
                    }
                }
            }


           /* dbClient.setQuery("SELECT fuse, rank FROM fuserights;");
            DataTable Data = dbClient.getTable();

            dbClient.setQuery("SELECT fuse, sub FROM fuserights_subs;");
            DataTable SubData = dbClient.getTable();

            if (Data != null)
            {
                foreach (DataRow Row in Data.Rows)
                {
                    Rights.Add((string)pur["fuse"], Convert.ToUInt32(pur["rank"]));
                }
            }

            if (SubData != null)
            {
                foreach (DataRow Row in SubData.Rows)
                {
                    SubRights.Add((string)pur["fuse"], (string)pur["sub"]);
                }
            }*/
        }

        /*internal Boolean RankHasRight(uint RankId, string Fuse)
        {
            if (!ContainsRight(Fuse))
            {
                return false;
            }

            uint MinRank = Rights[Fuse];

            if (RankId >= MinRank)
            {
                return true;
            }

            return false;
        }*/

        //internal bool SubHasRight(string Sub, string Fuse)
        //{
            //if (this.SubRights.ContainsKey(Fuse) && this.SubRights[Fuse] == Sub)
            //{
            //    //return true;
            //}
            //
            //return false;
        //}

        //internal Role GetRole(UInt32 Id)
        //{
        //    if (!ContainsRole(Id))
        //    {
        //        return null;
        //    }

        //    return Roles[Id];
        //}

        internal bool CheckRank(uint UInt)
        {
            return this.rankCheck(UInt);
        }

        public bool CheckRanks(uint UInt, string Rank)
        {
            if (this.CheckRanksD(UInt))
            {
                List<string> list = this.PermissionsRanksD[UInt];
                if (!list.Contains(Rank))
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public bool CheckUser(uint UInt, string Rank)
        {
            if (!this.rankCheck(UInt))
            {
                return false;
            }
            List<string> list = this.PermissionsUsersD[UInt];
            return list.Contains(Rank);
        }

        internal bool rankCheck(uint UInt)
        {
            return this.PermissionsUsersD.ContainsKey(UInt);
        }

        public bool CheckRanksD(uint UInt)
        {
            return this.PermissionsRanksD.ContainsKey(UInt);
        }

        /*internal List<string> GetRightsForHabbo(Habbo Habbo)
        {
            List<string> UserRights = new List<string>();

            if (Habbo == null)
                return UserRights;

            UserRights.AddRange(GetRightsForRank(Habbo.Rank));

            foreach (string SubscriptionId in Habbo.GetSubscriptionManager().SubList)
            {
                UserRights.AddRange(GetRightsForSub(SubscriptionId));
            }

            return UserRights;
        }*/

        /*internal List<string> GetRightsForRank(uint RankId)
        {
            List<string> UserRights = new List<string>();

                foreach (KeyValuePair<string, uint> Data in Rights)
                {
                    if (RankId >= Data.Value && !UserRights.Contains(Data.Key))
                    {
                        UserRights.Add(Data.Key);
                    }
                }

            return UserRights;
        }*/

        /*internal List<string> GetRightsForSub(string SubId)
        {
            List<string> UserRights = new List<string>();

                foreach (KeyValuePair<string, string> Data in SubRights)
                {
                    if (Data.Value == SubId)
                    {
                        UserRights.Add(Data.Key);
                    }
                }

            return UserRights;
        }*/

        //internal Boolean ContainsRole(UInt32 Id)
        //{
        //    return Roles.ContainsKey(Id);
        //}

        //internal Boolean ContainsRight(string Right)
        //{
        //    //return Rights.ContainsKey(Right);
        //}

        //internal void ClearRoles()
        //{
        //    Roles.Clear();
        //}

        internal void ClearRights()
        {
            this.PermissionsRanksD.Clear();
            this.PermissionsUsersD.Clear();
            this.PermissionsUsersR.Clear();
            this.Ranks.Clear();
        }
    }
}
