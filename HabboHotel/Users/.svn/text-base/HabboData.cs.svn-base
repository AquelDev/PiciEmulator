//using System;
//using System.Data;
//using Database_Manager.Database.Session_Details.Interfaces;
//using Butterfly.Core;

//namespace Butterfly.HabboHotel
//{
//    class HabboData
//    {
//        #region Fields
//        private bool mUserFound;
//        private DataRow mUserInformation;
//        private DataTable mAchievementData;
//        private DataTable mUserFavouriteRooms;
//        private DataTable mUserIgnores;
//        private DataTable mUsertags;
//        private DataTable mSubscriptionData;
//        private DataTable mUserBadges;
//        private DataTable mUserInventory;
//        private DataTable mUserEffects;
//        private DataTable mUserFriends;
//        private DataTable mUserRequests;
//        private DataTable mUsersRooms;
//        private DataTable mUserPets;
//        #endregion

//        #region Return values
//        internal bool UserFound
//        {
//            get
//            {
//                return mUserFound;
//            }
//        }

//        internal DataRow GetHabboDataRow
//        {
//            get
//            {
//                return mUserInformation;
//            }
//        }

//        internal DataTable GetAchievementData
//        {
//            get
//            {
//                return mAchievementData;
//            }
//        }

//        internal DataTable GetUserFavouriteRooms
//        {
//            get
//            {
//                return mUserFavouriteRooms;
//            }
//        }

//        internal DataTable GetUserIgnores
//        {
//            get
//            {
//                return mUserIgnores;
//            }
//        }

//        internal DataTable GetUserTags
//        {
//            get
//            {
//                return mUsertags;
//            }
//        }

//        internal DataTable GetSupscriptionData
//        {
//            get
//            {
//                return mSubscriptionData;
//            }
//        }

//        internal DataTable GetUserBadges
//        {
//            get
//            {
//                return mUserBadges;
//            }
//        }

//        internal DataTable GetUserInventory
//        {
//            get
//            {
//                return mUserInventory;
//            }
//        }

//        internal DataTable GetUserEffects
//        {
//            get
//            {
//                return mUserEffects;
//            }
//        }

//        internal DataTable GetFriendList
//        {
//            get
//            {
//                return mUserFriends;
//            }
//        }

//        internal DataTable GetFriendReqs
//        {
//            get
//            {
//                return mUserRequests;
//            }
//        }

//        internal DataTable GetUsersRooms
//        {
//            get
//            {
//                return mUsersRooms;
//            }
//            set
//            {
//                mUsersRooms = value;
//            }
//        }
//        internal DataTable GetUserPets
//        {
//            get
//            {
//                return mUserPets;
//            }
//        }
//        #endregion

//        #region Constructor
//        internal HabboData(string pSSOTicket, string pIPAddress, bool LoadFull)
//        {
//            try
//            {
//                using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
//                {
//                    dbClient.setQuery("SELECT * FROM users WHERE auth_ticket = @auth_ticket AND ip_last = '" + pIPAddress + "'");
//                    dbClient.addParameter("auth_ticket", pSSOTicket);
//                    mUserInformation = dbClient.getRow();

//                    if (mUserInformation != null)
//                    {
//                        mUserFound = true;
//                        UInt32 UserID = (UInt32)mUserInformation["id"];

//                        if (LoadFull)
//                        {
//                            string Creds = (string)mUserInformation["lastdailycredits"];
//                            string Today = DateTime.Today.ToString("MM/dd");
//                            if (Creds != Today)
//                            {
//                                dbClient.runFastQuery("UPDATE users SET credits = credits + 3000, daily_respect_points = 3, lastdailycredits = '" + Today + "' WHERE id = " + UserID);
//                                mUserInformation["credits"] = (int)mUserInformation["credits"] + 3000;
//                            }
//                            dbClient.setQuery("SELECT achievement_id,achievement_level FROM user_achievements WHERE user_id = " + UserID);
//                            mAchievementData = dbClient.getTable();

//                            dbClient.setQuery("SELECT room_id FROM user_favorites WHERE user_id = " + UserID);
//                            mUserFavouriteRooms = dbClient.getTable();

//                            dbClient.setQuery("SELECT ignore_id FROM user_ignores WHERE user_id = " + UserID);
//                            mUserIgnores = dbClient.getTable();

//                            dbClient.setQuery("SELECT tag FROM user_tags WHERE user_id = " + UserID);
//                            mUsertags = dbClient.getTable();

//                            dbClient.setQuery("SELECT * FROM user_subscriptions WHERE user_id = " + UserID);
//                            mSubscriptionData = dbClient.getTable();

//                            dbClient.setQuery("SELECT * FROM user_badges WHERE user_id = " + UserID);
//                            mUserBadges = dbClient.getTable();

//                            dbClient.setQuery("SELECT * FROM user_items WHERE user_id = " + UserID);
//                            mUserInventory = dbClient.getTable();

//                            dbClient.setQuery("SELECT * FROM user_effects WHERE user_id =  " + UserID);
//                            mUserEffects = dbClient.getTable();

//                            dbClient.setQuery("SELECT users.id,users.username,users.motto,users.look,users.last_online FROM users JOIN messenger_friendships ON users.id = messenger_friendships.user_two_id WHERE messenger_friendships.user_one_id = " + UserID);
//                            mUserFriends = dbClient.getTable();

//                            dbClient.setQuery("SELECT messenger_requests.id,messenger_requests.from_id,users.username FROM users JOIN messenger_requests ON users.id = messenger_requests.from_id WHERE messenger_requests.to_id = '" + UserID + "'");
//                            mUserRequests = dbClient.getTable();

//                            dbClient.setQuery("SELECT * FROM rooms WHERE owner = @name ORDER BY id ASC");
//                            dbClient.addParameter("name", (string)mUserInformation["username"]);
//                            mUsersRooms = dbClient.getTable();

//                            dbClient.setQuery("SELECT * FROM user_pets WHERE user_id = " + UserID + " AND room_id = 0");
//                            mUserPets = dbClient.getTable();

//                            dbClient.runFastQuery("UPDATE users SET online = '1', auth_ticket = '', ip_last = '" + pIPAddress + "' WHERE id = " + UserID + " LIMIT 1; " +
//                                                  "UPDATE user_info SET login_timestamp = '" + ButterflyEnvironment.GetUnixTimestamp() + "' WHERE user_id = " + UserID + " LIMIT 1;");
//                        }

//                        //Logging.WriteLine("Userdata for user [" + UserID + "] fetched");
//                    }
//                    else
//                    {
//                        mUserFound = false;
//                    }
//                }
//            }
//            catch (Exception e)
//            {
//                Logging.LogException("FATAL ERROR AT LOGIN: " + Environment.NewLine + pSSOTicket + Environment.NewLine + pIPAddress + Environment.NewLine + e.ToString());
//                throw e;
//            }
//        }

//        internal HabboData(string Username, bool LoadFull)
//        {
//            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
//            {
//                dbClient.setQuery("SELECT * FROM users WHERE username = @username");
//                dbClient.addParameter("username", Username);
//                mUserInformation = dbClient.getRow();

//                if (mUserInformation != null)
//                {
//                    mUserFound = true;
//                    UInt32 UserID = (UInt32)mUserInformation["id"];

//                    if (LoadFull)
//                    {
//                        dbClient.setQuery("SELECT achievement_id,achievement_level FROM user_achievements WHERE user_id = " + UserID);
//                        mAchievementData = dbClient.getTable();

//                        dbClient.setQuery("SELECT room_id FROM user_favorites WHERE user_id = " + UserID);
//                        mUserFavouriteRooms = dbClient.getTable();

//                        dbClient.setQuery("SELECT ignore_id FROM user_ignores WHERE user_id = " + UserID);
//                        mUserIgnores = dbClient.getTable();

//                        dbClient.setQuery("SELECT tag FROM user_tags WHERE user_id = " + UserID);
//                        mUsertags = dbClient.getTable();

//                        dbClient.setQuery("SELECT * FROM user_subscriptions WHERE user_id = " + UserID);
//                        mSubscriptionData = dbClient.getTable();

//                        dbClient.setQuery("SELECT * FROM user_badges WHERE user_id = " + UserID);
//                        mUserBadges = dbClient.getTable();

//                        dbClient.setQuery("SELECT * FROM user_items WHERE user_id = " + UserID);
//                        mUserInventory = dbClient.getTable();

//                        dbClient.setQuery("SELECT * FROM user_effects WHERE user_id = " + UserID);
//                        mUserEffects = dbClient.getTable();

//                        dbClient.setQuery("SELECT users.id,users.username,users.motto,users.look,users.last_online FROM users JOIN messenger_friendships ON users.id = messenger_friendships.user_two_id WHERE messenger_friendships.user_one_id = " + UserID);
//                        mUserFriends = dbClient.getTable();

//                        dbClient.setQuery("SELECT messenger_requests.id,messenger_requests.from_id,users.username FROM users JOIN messenger_requests ON users.id = messenger_requests.from_id WHERE messenger_requests.to_id = " + UserID);
//                        mUserRequests = dbClient.getTable();

//                        dbClient.setQuery("SELECT * FROM rooms WHERE owner = @name ORDER BY id ASC");
//                        dbClient.addParameter("name", (string)mUserInformation["username"]);
//                        mUsersRooms = dbClient.getTable();
//                    }
//                }
//                else
//                {
//                    mUserFound = false;
//                }
//            }
//        }

//        #endregion

//        internal void Clear()
//        {
//            mUserInformation.Delete();
//            mAchievementData.Clear();
//            mUserFavouriteRooms.Clear();
//            mUserIgnores.Clear();
//            mUsertags.Clear();
//            mSubscriptionData.Clear();
//            mUserBadges.Clear();
//            mUserInventory.Clear();
//            mUserEffects.Clear();
//            //mUserFriends.Clear();
//            //mUserRequests.Clear();
//            mUsersRooms.Clear();
//            mUserPets.Clear();
//        }
//    }
//}
