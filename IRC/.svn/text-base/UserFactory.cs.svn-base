//using System.Collections.Generic;
//using System.Data;
//using Butterfly;
//using Butterfly.Collections;
//using Database_Manager.Database.Session_Details.Interfaces;

//namespace Butterfly.IRC
//{
//    class UserFactory
//    {
//        private static SafeDictionary<string, User> users;

//        internal static void Init()
//        {
//            users = new SafeDictionary<string, User>();
//            foreach (User user in GetUsers())
//                users.Add(user.ircuser, user);
//        }

//        internal static List<User> GetUsers()
//        {
//            List<User> users = new List<User>();

//            DataTable dTable;
//            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
//            {
//                dbClient.setQuery("SELECT * FROM irc_sessions");
//                dTable = dbClient.getTable();
//            }

//            string habbousername;
//            string ircusername;
//            int rank;
//            foreach (DataRow dRow in dTable.Rows)
//            {
//                habbousername = (string)dRow[0];
//                ircusername = (string)dRow[1];
//                rank = (int)dRow[2];

//                User user = new User(ircusername, habbousername, rank);
//                users.Add(user);
//            }
            
//            return users;
//        }

//        internal static void Register(string habbousername, string ircusername)
//        {
//            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
//            {
//                dbClient.setQuery("DELETE FROM irc_sessions WHERE irc_username = @ircname OR vg_username = @username");
//                dbClient.addParameter("ircname", ircusername);
//                dbClient.addParameter("username", habbousername);
//                dbClient.runQuery();

//                dbClient.setQuery("REPLACE INTO irc_sessions (irc_username,vg_username) VALUES (@ircusername,@habbousername)");
//                dbClient.addParameter("habbousername", habbousername);
//                dbClient.addParameter("ircusername", ircusername);
//                dbClient.runQuery();
//            }

//            User user = new User(ircusername, habbousername, 1);

//            List<User> toRemove = new List<User>();
//            foreach (User tUser in users.Values)
//            {
//                if (tUser.habbouser == habbousername || tUser.ircuser == ircusername)
//                {
//                    toRemove.Add(tUser);
//                }
//            }

//            foreach (User userRemove in toRemove)
//            {
//                users.Remove(userRemove.ircuser);
//            }
//            users.Add(user.ircuser, user);
//        }

//        internal static User GetUser(string ircusername)
//        {
//            if (users.ContainsKey(ircusername))
//                return users[ircusername];
//            return new User();
//        }

//        internal static bool IsRegistered(string username)
//        {
//            return users.ContainsKey(username);
//        }

//        internal static bool GotUser(string habboUsername)
//        {
//            foreach (User user in users.Values)
//            {
//                if (user.habbouser == habboUsername)
//                    return true;
//            }

//            return false;
//        }

//        internal static User GetUserByHabboname(string habboname)
//        {
//            foreach (User user in users.Values)
//            {
//                if (user.habbouser == habboname)
//                    return user;
//            }

//            return new User();
//        }

//    }
//}
