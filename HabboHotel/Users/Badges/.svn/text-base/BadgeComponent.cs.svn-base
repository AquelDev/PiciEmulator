using System;
using System.Collections.Generic;
using System.Data;
using Butterfly.Messages;
using Database_Manager.Database.Session_Details.Interfaces;
using Butterfly.HabboHotel.Users.UserDataManagement;
using System.Collections;

namespace Butterfly.HabboHotel.Users.Badges
{
    class BadgeComponent
    {
        //private List<Badge> Badges;
        private Hashtable Badges;
        private UInt32 UserId;

        internal int Count
        {
            get
            {
                return Badges.Count;
            }
        }

        internal int EquippedCount
        {
            get
            {
                int i = 0;

                foreach (Badge Badge in Badges.Values)
                {
                    if (Badge.Slot <= 0)
                    {
                        continue;
                    }

                    i++;
                }

                return i;
            }
        }

        internal Hashtable BadgeList
        {
            get
            {
                return Badges;
            }
        }

        internal BadgeComponent(uint userId, UserData data)
        {
            this.Badges = new Hashtable();
            foreach (Badge badge in data.badges)
            {
                if (!Badges.ContainsKey(badge.Code))
                    Badges.Add(badge.Code, badge);
            }

            this.UserId = userId;
        }

        internal Badge GetBadge(string Badge)
        {
            if (Badges.ContainsKey(Badge))
                return (Badge)Badges[Badge];

            return null;
        }

        internal Boolean HasBadge(string Badge)
        {
            return Badges.ContainsKey(Badge);
        }

        internal void GiveBadge(string Badge, Boolean InDatabase)
        {
            GiveBadge(Badge, 0, InDatabase);
        }

        internal void GiveBadge(string Badge, int Slot, Boolean InDatabase)
        {
            if (HasBadge(Badge))
            {
                return;
            }

            if (InDatabase)
            {
                using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    dbClient.setQuery("INSERT INTO user_badges (user_id,badge_id,badge_slot) VALUES (" + UserId + ",@badge," + Slot + ")");
                    dbClient.addParameter("badge", Badge);
                    dbClient.runQuery();
                }
            }

            Badges.Add(Badge, new Badge(Badge, Slot));
        }

        //internal void SetBadgeSlot(string Badge, int Slot)
        //{
        //    Badge B = GetBadge(Badge);

        //    if (B == null)
        //    {
        //        return;
        //    }

        //    B.Slot = Slot;
        //}

        internal void ResetSlots()
        {
            foreach (Badge Badge in Badges.Values)
            {
                Badge.Slot = 0;
            }
        }

        internal void RemoveBadge(string Badge)
        {
            if (!HasBadge(Badge))
            {
                return;
            }

            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.setQuery("DELETE FROM user_badges WHERE badge_id = @badge AND user_id = " + UserId + " LIMIT 1");
                dbClient.addParameter("badge", Badge);
                dbClient.runQuery();
            }

            Badges.Remove(GetBadge(Badge));
        }


        internal ServerMessage Serialize()
        {
            var EquippedBadges = new List<Badge>();

            ServerMessage Message = new ServerMessage(229);
            Message.AppendInt32(Count);

            foreach (Badge Badge in Badges.Values)
            {
                Message.AppendInt32(0);
                Message.AppendStringWithBreak(Badge.Code);

                if (Badge.Slot > 0)
                {
                    EquippedBadges.Add(Badge);

                }
            }

            Message.AppendInt32(EquippedBadges.Count);

            foreach (Badge Badge in EquippedBadges)
            {
                Message.AppendInt32(Badge.Slot);
                Message.AppendString(Badge.Code);
            }

            return Message;
        }
    }
}
