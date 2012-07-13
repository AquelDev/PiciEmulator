using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Database_Manager.Database.Session_Details.Interfaces;
using Butterfly.HabboHotel.Users.UserDataManagement;

namespace Butterfly.HabboHotel.Users.Subscriptions
{
    class SubscriptionManager
    {
        private uint UserId;
        private Dictionary<string, Subscription> Subscriptions;

        internal List<string> SubList
        {
            get
            {
                List<string> List = new List<string>();

                    foreach (Subscription Subscription in Subscriptions.Values)
                    {
                        List.Add(Subscription.SubscriptionId);
                    }

                return List;
            }
        }

        internal SubscriptionManager(uint userID, UserData userData)
        {
            this.UserId = userID;
            Subscriptions = userData.subscriptions;
        }

        internal void Clear()
        {
            Subscriptions.Clear();
        }

        internal Subscription GetSubscription(string SubscriptionId)
        {
            if (Subscriptions.ContainsKey(SubscriptionId))
            {
                return Subscriptions[SubscriptionId];
            }

            return null;
        }

        internal Boolean HasSubscription(string SubscriptionId)
        {
            if (!Subscriptions.ContainsKey(SubscriptionId))
            {
                return false;
            }

            Subscription Sub = Subscriptions[SubscriptionId];

            if (Sub.IsValid())
            {
                return true;
            }

            return false;
        }

        internal void AddOrExtendSubscription(string SubscriptionId, int DurationSeconds)
        {
            SubscriptionId = SubscriptionId.ToLower();

            if (Subscriptions.ContainsKey(SubscriptionId))
            {
                Subscription Sub = Subscriptions[SubscriptionId];
                if (Sub.IsValid())
                    Sub.ExtendSubscription(DurationSeconds);
                else
                    Sub.SetEndTime(((int)ButterflyEnvironment.GetUnixTimestamp() + DurationSeconds));
                
                using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
                {
                    dbClient.setQuery("UPDATE user_subscriptions SET timestamp_expire = " + Sub.ExpireTime + " WHERE user_id = " + UserId + " AND subscription_id = @subcrbr");
                    dbClient.addParameter("subcrbr", SubscriptionId);
                    dbClient.runQuery();
                }

                return;
            }

            int TimeCreated = (int)ButterflyEnvironment.GetUnixTimestamp();
            int TimeExpire = ((int)ButterflyEnvironment.GetUnixTimestamp() + DurationSeconds);

            Subscription NewSub = new Subscription(SubscriptionId, TimeExpire);

            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.setQuery("INSERT INTO user_subscriptions (user_id,subscription_id,timestamp_activated,timestamp_expire) VALUES (" + UserId + ",@subcrbr," + TimeCreated + "," + TimeExpire + ")");
                dbClient.addParameter("subcrbr", SubscriptionId);
                dbClient.runQuery();
            }

            Subscriptions.Add(NewSub.SubscriptionId.ToLower(), NewSub);
        }
    }
}
