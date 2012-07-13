using System;
using System.Collections.Generic;
using System.Data;
using Butterfly.HabboHotel.Users;
using Database_Manager.Database.Session_Details.Interfaces;

namespace Butterfly.HabboHotel.Roles
{
    class RoleManager
    {
        //private Dictionary<uint, Role> Roles;
        private Dictionary<string, uint> Rights;
        private Dictionary<string, string> SubRights;

        internal RoleManager()
        {
            //Roles = new Dictionary<uint,Role>();
            Rights = new Dictionary<string, uint>();
            SubRights = new Dictionary<string, string>();
        }

        //internal void LoadRoles(DatabaseClient dbClient)
        //{
        //    ClearRoles();

        //    DataTable Data = dbClient.getTable("SELECT * FROM ranks ORDER BY id ASC;");

        //    if (Data != null)
        //    {
        //        foreach (DataRow Row in Data.Rows)
        //        {
        //            Roles.Add((uint)Row["id"], new Role((uint)Row["id"], (string)Row["name"]));
        //        }
        //    }
        //}

        internal void LoadRights(IQueryAdapter dbClient)
        {
            ClearRights();

            dbClient.setQuery("SELECT fuse, rank FROM fuserights;");
            DataTable Data = dbClient.getTable();

            dbClient.setQuery("SELECT fuse, sub FROM fuserights_subs;");
            DataTable SubData = dbClient.getTable();

            if (Data != null)
            {
                foreach (DataRow Row in Data.Rows)
                {
                    Rights.Add((string)Row["fuse"], Convert.ToUInt32(Row["rank"]));
                }
            }

            if (SubData != null)
            {
                foreach (DataRow Row in SubData.Rows)
                {
                    SubRights.Add((string)Row["fuse"], (string)Row["sub"]);
                }
            }
        }

        internal Boolean RankHasRight(uint RankId, string Fuse)
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
        }

        internal bool SubHasRight(string Sub, string Fuse)
        {
            if (this.SubRights.ContainsKey(Fuse) && this.SubRights[Fuse] == Sub)
            {
                return true;
            }

            return false;
        }

        //internal Role GetRole(UInt32 Id)
        //{
        //    if (!ContainsRole(Id))
        //    {
        //        return null;
        //    }

        //    return Roles[Id];
        //}

        internal List<string> GetRightsForHabbo(Habbo Habbo)
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
        }

        internal List<string> GetRightsForRank(uint RankId)
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
        }

        internal List<string> GetRightsForSub(string SubId)
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
        }

        //internal Boolean ContainsRole(UInt32 Id)
        //{
        //    return Roles.ContainsKey(Id);
        //}

        internal Boolean ContainsRight(string Right)
        {
            return Rights.ContainsKey(Right);
        }

        //internal void ClearRoles()
        //{
        //    Roles.Clear();
        //}

        internal void ClearRights()
        {
            Rights.Clear();
        }
    }
}
