using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

using Butterfly.HabboHotel.GameClients;
using Butterfly.Core;
using Database_Manager.Database.Session_Details.Interfaces;

namespace Butterfly.HabboHotel.Support
{
    class ModerationBanManager
    {
        private Hashtable bannedUsernames;
        private Hashtable bannedIPs;

        internal ModerationBanManager()
        {
            bannedUsernames = new Hashtable();
            bannedIPs = new Hashtable();
        }

        internal void LoadBans(IQueryAdapter dbClient)
        {
            bannedUsernames.Clear();
            bannedIPs.Clear();

            dbClient.setQuery("SELECT bantype,value,reason,expire FROM bans");
            DataTable BanData = dbClient.getTable();

            double timestmp = ButterflyEnvironment.GetUnixTimestamp();

            string value;
            string reason;
            string type;
            double expires;
            
            foreach (DataRow dRow in BanData.Rows)
            {
                value = (string)dRow["value"];
                reason = (string)dRow["reason"];
                expires = (double)dRow["expire"];
                type = (string)dRow["bantype"];


                ModerationBanType banType;
                if (type == "user")
                    banType = ModerationBanType.USERNAME;
                else
                    banType = ModerationBanType.IP;


                ModerationBan ban = new ModerationBan(banType, value, reason, expires);

                if (expires > timestmp)
                {
                    if (ban.Type == ModerationBanType.USERNAME)
                        if (!bannedUsernames.ContainsKey(value))
                            bannedUsernames.Add(value, ban);
                    else
                        if (!bannedIPs.ContainsKey(value))
                            bannedIPs.Add(value, ban);
                }
            }
        }

        internal string GetBanReason(string username, string ip)
        {
            if (bannedUsernames.ContainsKey(username))
            {
                ModerationBan ban = (ModerationBan)bannedUsernames[username];
                if (!ban.Expired)
                    return ban.ReasonMessage;
            }
            else if (bannedIPs.ContainsKey(ip))
            {
                ModerationBan ban = (ModerationBan)bannedIPs[username];
                if (!ban.Expired)
                    return ban.ReasonMessage;
            }

            return string.Empty;
        }

        // PENDING REWRITE
        internal void BanUser(GameClient Client, string Moderator, Double LengthSeconds, string Reason, Boolean IpBan)
        {
            ModerationBanType Type = ModerationBanType.USERNAME;
            string Var = Client.GetHabbo().Username;
            string RawVar = "user";
            Double Expire = ButterflyEnvironment.GetUnixTimestamp() + LengthSeconds;

            if (IpBan)
            {
                Type = ModerationBanType.IP;
                Var = Client.GetConnection().getIp();
                RawVar = "ip";
            }

            ModerationBan ban = new ModerationBan(Type, Var, Reason, Expire);

            if (ban.Type == ModerationBanType.IP)
            {
                if (bannedIPs.ContainsKey(Var))
                    bannedIPs[Var] = ban;
                else
                    bannedIPs.Add(Var, ban);
            }
            else
            {
                if (bannedUsernames.ContainsKey(Var))
                    bannedUsernames[Var] = ban;
                else
                    bannedUsernames.Add(Var, ban);
            }

            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.setQuery("INSERT INTO bans (bantype,value,reason,expire,added_by,added_date) VALUES (@rawvar,@var,@reason,'" + Expire + "',@mod,'" + DateTime.Now.ToLongDateString() + "')");
                dbClient.addParameter("rawvar", RawVar);
                dbClient.addParameter("var", Var);
                dbClient.addParameter("reason", Reason);
                dbClient.addParameter("mod", Moderator);
                dbClient.runQuery();                
            }

            if (IpBan)
            {
                DataTable UsersAffected = null;

                using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    dbClient.setQuery("SELECT id FROM users WHERE ip_last = @var");
                    dbClient.addParameter("var", Var);
                    UsersAffected = dbClient.getTable();
                }

                if (UsersAffected != null)
                {
                    using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                    {
                        foreach (DataRow Row in UsersAffected.Rows)
                        {
                            dbClient.runFastQuery("UPDATE user_info SET bans = bans + 1 WHERE user_id = " + Convert.ToUInt32(Row["id"]));
                        }
                    }
                }


                BanUser(Client, Moderator, LengthSeconds, Reason, false);
            }
            else
            {
                using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    dbClient.runFastQuery("UPDATE user_info SET bans = bans + 1 WHERE user_id = " + Client.GetHabbo().Id);
                }

                Client.SendBanMessage(LanguageLocale.GetValue("moderation.banned") + " " + Reason);
                Client.Disconnect();
            }
        }

        internal void UnbanUser(string usernameOrIP)
        {
            List<ModerationBan> toRemove = new List<ModerationBan>();

            bannedUsernames.Remove(usernameOrIP);
            bannedIPs.Remove(usernameOrIP);

            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.setQuery("DELETE FROM bans WHERE value = @userorip");
                dbClient.addParameter("userorip", usernameOrIP);
                dbClient.runQuery();
            }
        }
    }
}
