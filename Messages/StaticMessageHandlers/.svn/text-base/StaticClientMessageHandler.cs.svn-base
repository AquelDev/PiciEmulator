using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Butterfly.Messages.StaticMessageHandlers
{
    class StaticClientMessageHandler
    {
        private delegate void StaticRequestHandler(GameClientMessageHandler handler);
        private static Hashtable handlers;

        internal static void Initialize()
        {
            handlers = new Hashtable(177);
            RegisterPacketLibary();
        }

        internal static void HandlePacket(GameClientMessageHandler handler, ClientMessage message)
        {
            if (handlers.ContainsKey(message.Id))
            {
                StaticRequestHandler currentHandler = (StaticRequestHandler)handlers[message.Id];
                currentHandler.Invoke(handler);
            }
        }

        #region Register
        internal static void RegisterPacketLibary()
        {
            handlers.Add(101, new StaticRequestHandler(SharedPacketLib.GetCatalogIndex));
            handlers.Add(102, new StaticRequestHandler(SharedPacketLib.GetCatalogPage));
            handlers.Add(129, new StaticRequestHandler(SharedPacketLib.RedeemVoucher));
            handlers.Add(100, new StaticRequestHandler(SharedPacketLib.HandlePurchase));
            handlers.Add(472, new StaticRequestHandler(SharedPacketLib.PurchaseGift));
            handlers.Add(412, new StaticRequestHandler(SharedPacketLib.GetRecyclerRewards));
            handlers.Add(3030, new StaticRequestHandler(SharedPacketLib.CanGift));
            handlers.Add(3011, new StaticRequestHandler(SharedPacketLib.GetCataData1));
            handlers.Add(473, new StaticRequestHandler(SharedPacketLib.GetCataData2));
            handlers.Add(3012, new StaticRequestHandler(SharedPacketLib.MarketplaceCanSell));
            handlers.Add(3010, new StaticRequestHandler(SharedPacketLib.MarketplacePostItem));
            handlers.Add(3019, new StaticRequestHandler(SharedPacketLib.MarketplaceGetOwnOffers));
            handlers.Add(3015, new StaticRequestHandler(SharedPacketLib.MarketplaceTakeBack));
            handlers.Add(3016, new StaticRequestHandler(SharedPacketLib.MarketplaceClaimCredits));
            handlers.Add(3018, new StaticRequestHandler(SharedPacketLib.MarketplaceGetOffers));
            handlers.Add(3014, new StaticRequestHandler(SharedPacketLib.MarketplacePurchase));
            handlers.Add(42, new StaticRequestHandler(SharedPacketLib.CheckPetName));
            handlers.Add(3007, new StaticRequestHandler(SharedPacketLib.PetRaces));
            handlers.Add(196, new StaticRequestHandler(SharedPacketLib.Pong));
            handlers.Add(231, new StaticRequestHandler(SharedPacketLib.GetGroupdetails));
            handlers.Add(206, new StaticRequestHandler(SharedPacketLib.SendSessionParams));
            handlers.Add(415, new StaticRequestHandler(SharedPacketLib.SSOLogin));
            handlers.Add(416, new StaticRequestHandler(SharedPacketLib.InitHelpTool));
            handlers.Add(417, new StaticRequestHandler(SharedPacketLib.GetHelpCategories));
            handlers.Add(418, new StaticRequestHandler(SharedPacketLib.ViewHelpTopic));
            handlers.Add(419, new StaticRequestHandler(SharedPacketLib.SearchHelpTopics));
            handlers.Add(420, new StaticRequestHandler(SharedPacketLib.GetTopicsInCategory));
            handlers.Add(453, new StaticRequestHandler(SharedPacketLib.SubmitHelpTicket));
            handlers.Add(238, new StaticRequestHandler(SharedPacketLib.DeletePendingCFH));
            handlers.Add(440, new StaticRequestHandler(SharedPacketLib.CallGuideBot));
            handlers.Add(200, new StaticRequestHandler(SharedPacketLib.ModSendRoomAlert));
            handlers.Add(450, new StaticRequestHandler(SharedPacketLib.ModPickTicket));
            handlers.Add(451, new StaticRequestHandler(SharedPacketLib.ModReleaseTicket));
            handlers.Add(452, new StaticRequestHandler(SharedPacketLib.ModCloseTicket));
            handlers.Add(454, new StaticRequestHandler(SharedPacketLib.ModGetUserInfo));
            handlers.Add(455, new StaticRequestHandler(SharedPacketLib.ModGetUserChatlog));
            handlers.Add(456, new StaticRequestHandler(SharedPacketLib.ModGetRoomChatlog));
            handlers.Add(457, new StaticRequestHandler(SharedPacketLib.ModGetTicketChatlog));
            handlers.Add(458, new StaticRequestHandler(SharedPacketLib.ModGetRoomVisits));
            handlers.Add(459, new StaticRequestHandler(SharedPacketLib.ModGetRoomTool));
            handlers.Add(460, new StaticRequestHandler(SharedPacketLib.ModPerformRoomAction));
            handlers.Add(461, new StaticRequestHandler(SharedPacketLib.ModSendUserCaution));
            handlers.Add(462, new StaticRequestHandler(SharedPacketLib.ModSendUserMessage));
            handlers.Add(463, new StaticRequestHandler(SharedPacketLib.ModKickUser));
            handlers.Add(464, new StaticRequestHandler(SharedPacketLib.ModBanUser));
            handlers.Add(12, new StaticRequestHandler(SharedPacketLib.InitMessenger));
            handlers.Add(15, new StaticRequestHandler(SharedPacketLib.FriendsListUpdate));
            handlers.Add(40, new StaticRequestHandler(SharedPacketLib.RemoveBuddy));
            handlers.Add(41, new StaticRequestHandler(SharedPacketLib.SearchHabbo));
            handlers.Add(33, new StaticRequestHandler(SharedPacketLib.SendInstantMessenger));
            handlers.Add(37, new StaticRequestHandler(SharedPacketLib.AcceptRequest));
            handlers.Add(38, new StaticRequestHandler(SharedPacketLib.DeclineRequest));
            handlers.Add(39, new StaticRequestHandler(SharedPacketLib.RequestBuddy));
            handlers.Add(262, new StaticRequestHandler(SharedPacketLib.FollowBuddy));
            handlers.Add(34, new StaticRequestHandler(SharedPacketLib.SendInstantInvite));
            handlers.Add(391, new StaticRequestHandler(SharedPacketLib.OpenFlat));
            handlers.Add(19, new StaticRequestHandler(SharedPacketLib.AddFavorite));
            handlers.Add(20, new StaticRequestHandler(SharedPacketLib.RemoveFavorite));
            handlers.Add(53, new StaticRequestHandler(SharedPacketLib.GoToHotelView));
            handlers.Add(151, new StaticRequestHandler(SharedPacketLib.GetFlatCats));
            handlers.Add(233, new StaticRequestHandler(SharedPacketLib.EnterInquiredRoom));
            handlers.Add(380, new StaticRequestHandler(SharedPacketLib.GetPubs));
            handlers.Add(385, new StaticRequestHandler(SharedPacketLib.GetRoomInfo));
            handlers.Add(430, new StaticRequestHandler(SharedPacketLib.GetPopularRooms));
            handlers.Add(431, new StaticRequestHandler(SharedPacketLib.GetHighRatedRooms));
            handlers.Add(432, new StaticRequestHandler(SharedPacketLib.GetFriendsRooms));
            handlers.Add(433, new StaticRequestHandler(SharedPacketLib.GetRoomsWithFriends));
            handlers.Add(434, new StaticRequestHandler(SharedPacketLib.GetOwnRooms));
            handlers.Add(435, new StaticRequestHandler(SharedPacketLib.GetFavoriteRooms));
            handlers.Add(436, new StaticRequestHandler(SharedPacketLib.GetRecentRooms));
            handlers.Add(439, new StaticRequestHandler(SharedPacketLib.GetEvents));
            handlers.Add(382, new StaticRequestHandler(SharedPacketLib.GetPopularTags));
            handlers.Add(437, new StaticRequestHandler(SharedPacketLib.PerformSearch));
            handlers.Add(438, new StaticRequestHandler(SharedPacketLib.PerformSearch2));
            handlers.Add(3101, new StaticRequestHandler(SharedPacketLib.OpenQuests));
            handlers.Add(3102, new StaticRequestHandler(SharedPacketLib.StartQuest));
            handlers.Add(3106, new StaticRequestHandler(SharedPacketLib.StopQuest));
            handlers.Add(3107, new StaticRequestHandler(SharedPacketLib.GetCurrentQuest));
            handlers.Add(182, new StaticRequestHandler(SharedPacketLib.GetAdvertisement));
            handlers.Add(388, new StaticRequestHandler(SharedPacketLib.GetPub));
            handlers.Add(2, new StaticRequestHandler(SharedPacketLib.OpenPub));
            handlers.Add(230, new StaticRequestHandler(SharedPacketLib.GetGroupBadges));
            handlers.Add(215, new StaticRequestHandler(SharedPacketLib.GetRoomData1));
            handlers.Add(390, new StaticRequestHandler(SharedPacketLib.GetRoomData2));
            handlers.Add(126, new StaticRequestHandler(SharedPacketLib.GetRoomData3));
            handlers.Add(52, new StaticRequestHandler(SharedPacketLib.Talk));
            handlers.Add(55, new StaticRequestHandler(SharedPacketLib.Shout));
            handlers.Add(56, new StaticRequestHandler(SharedPacketLib.Whisper));
            handlers.Add(75, new StaticRequestHandler(SharedPacketLib.Move));
            handlers.Add(387, new StaticRequestHandler(SharedPacketLib.CanCreateRoom));
            handlers.Add(29, new StaticRequestHandler(SharedPacketLib.CreateRoom));
            handlers.Add(400, new StaticRequestHandler(SharedPacketLib.GetRoomEditData));
            handlers.Add(386, new StaticRequestHandler(SharedPacketLib.SaveRoomIcon));
            handlers.Add(401, new StaticRequestHandler(SharedPacketLib.SaveRoomData));
            handlers.Add(96, new StaticRequestHandler(SharedPacketLib.GiveRights));
            handlers.Add(97, new StaticRequestHandler(SharedPacketLib.TakeRights));
            handlers.Add(155, new StaticRequestHandler(SharedPacketLib.TakeAllRights));
            handlers.Add(95, new StaticRequestHandler(SharedPacketLib.KickUser));
            handlers.Add(320, new StaticRequestHandler(SharedPacketLib.BanUser));
            handlers.Add(71, new StaticRequestHandler(SharedPacketLib.InitTrade));
            handlers.Add(384, new StaticRequestHandler(SharedPacketLib.SetHomeRoom));
            handlers.Add(23, new StaticRequestHandler(SharedPacketLib.DeleteRoom));
            handlers.Add(79, new StaticRequestHandler(SharedPacketLib.LookAt));
            handlers.Add(317, new StaticRequestHandler(SharedPacketLib.StartTyping));
            handlers.Add(318, new StaticRequestHandler(SharedPacketLib.StopTyping));
            handlers.Add(319, new StaticRequestHandler(SharedPacketLib.IgnoreUser));
            handlers.Add(322, new StaticRequestHandler(SharedPacketLib.UnignoreUser));
            handlers.Add(345, new StaticRequestHandler(SharedPacketLib.CanCreateRoomEvent));
            handlers.Add(346, new StaticRequestHandler(SharedPacketLib.StartEvent));
            handlers.Add(347, new StaticRequestHandler(SharedPacketLib.StopEvent));
            handlers.Add(348, new StaticRequestHandler(SharedPacketLib.EditEvent));
            handlers.Add(94, new StaticRequestHandler(SharedPacketLib.Wave));
            handlers.Add(263, new StaticRequestHandler(SharedPacketLib.GetUserTags));
            handlers.Add(159, new StaticRequestHandler(SharedPacketLib.GetUserBadges));
            handlers.Add(261, new StaticRequestHandler(SharedPacketLib.RateRoom));
            handlers.Add(93, new StaticRequestHandler(SharedPacketLib.Dance));
            handlers.Add(98, new StaticRequestHandler(SharedPacketLib.AnswerDoorbell));
            handlers.Add(59, new StaticRequestHandler(SharedPacketLib.ReqLoadRoomForUser));
            handlers.Add(66, new StaticRequestHandler(SharedPacketLib.ApplyRoomEffect));
            handlers.Add(90, new StaticRequestHandler(SharedPacketLib.PlaceItem));
            handlers.Add(67, new StaticRequestHandler(SharedPacketLib.TakeItem));
            handlers.Add(73, new StaticRequestHandler(SharedPacketLib.MoveItem));
            handlers.Add(91, new StaticRequestHandler(SharedPacketLib.MoveWallItem));
            handlers.Add(392, new StaticRequestHandler(SharedPacketLib.TriggerItem));
            handlers.Add(393, new StaticRequestHandler(SharedPacketLib.TriggerItem));
            handlers.Add(83, new StaticRequestHandler(SharedPacketLib.OpenPostit));
            handlers.Add(84, new StaticRequestHandler(SharedPacketLib.SavePostit));
            handlers.Add(85, new StaticRequestHandler(SharedPacketLib.DeletePostit));
            handlers.Add(78, new StaticRequestHandler(SharedPacketLib.OpenPresent));
            handlers.Add(341, new StaticRequestHandler(SharedPacketLib.GetMoodlight));
            handlers.Add(342, new StaticRequestHandler(SharedPacketLib.UpdateMoodlight));
            handlers.Add(343, new StaticRequestHandler(SharedPacketLib.SwitchMoodlightStatus));
            handlers.Add(72, new StaticRequestHandler(SharedPacketLib.OfferTradeItem));
            handlers.Add(405, new StaticRequestHandler(SharedPacketLib.TakeBackTradeItem));
            handlers.Add(70, new StaticRequestHandler(SharedPacketLib.StopTrade));
            handlers.Add(403, new StaticRequestHandler(SharedPacketLib.StopTrade));
            handlers.Add(69, new StaticRequestHandler(SharedPacketLib.AcceptTrade));
            handlers.Add(68, new StaticRequestHandler(SharedPacketLib.UnacceptTrade));
            handlers.Add(402, new StaticRequestHandler(SharedPacketLib.CompleteTrade));
            handlers.Add(371, new StaticRequestHandler(SharedPacketLib.GiveRespect));
            handlers.Add(372, new StaticRequestHandler(SharedPacketLib.ApplyEffect));
            handlers.Add(373, new StaticRequestHandler(SharedPacketLib.EnableEffect));
            handlers.Add(232, new StaticRequestHandler(SharedPacketLib.TriggerItem));
            handlers.Add(314, new StaticRequestHandler(SharedPacketLib.TriggerItem));
            handlers.Add(247, new StaticRequestHandler(SharedPacketLib.TriggerItem));
            handlers.Add(76, new StaticRequestHandler(SharedPacketLib.TriggerItem));
            handlers.Add(77, new StaticRequestHandler(SharedPacketLib.TriggerItemDiceSpecial));
            handlers.Add(414, new StaticRequestHandler(SharedPacketLib.RecycleItems));
            handlers.Add(183, new StaticRequestHandler(SharedPacketLib.RedeemExchangeFurni));
            handlers.Add(113, new StaticRequestHandler(SharedPacketLib.EnterInfobus));
            handlers.Add(441, new StaticRequestHandler(SharedPacketLib.KickBot));
            handlers.Add(3002, new StaticRequestHandler(SharedPacketLib.PlacePet));
            handlers.Add(3001, new StaticRequestHandler(SharedPacketLib.GetPetInfo));
            handlers.Add(3003, new StaticRequestHandler(SharedPacketLib.PickUpPet));
            handlers.Add(3004, new StaticRequestHandler(SharedPacketLib.CommandsPet));
            handlers.Add(3005, new StaticRequestHandler(SharedPacketLib.RespectPet));
            handlers.Add(3254, new StaticRequestHandler(SharedPacketLib.PlacePostIt));
            handlers.Add(480, new StaticRequestHandler(SharedPacketLib.SetLookTransfer));
            handlers.Add(3052, new StaticRequestHandler(SharedPacketLib.SaveWiredCondition));
            handlers.Add(3051, new StaticRequestHandler(SharedPacketLib.SaveWired));
            handlers.Add(3050, new StaticRequestHandler(SharedPacketLib.SaveWired));
            handlers.Add(221, new StaticRequestHandler(SharedPacketLib.GetMusicData));
            handlers.Add(255, new StaticRequestHandler(SharedPacketLib.AddPlaylistItem));
            handlers.Add(256, new StaticRequestHandler(SharedPacketLib.RemovePlaylistItem));
            handlers.Add(259, new StaticRequestHandler(SharedPacketLib.GetDisks));
            handlers.Add(258, new StaticRequestHandler(SharedPacketLib.GetPlaylists));
            handlers.Add(7, new StaticRequestHandler(SharedPacketLib.GetUserInfo));
            handlers.Add(8, new StaticRequestHandler(SharedPacketLib.GetBalance));
            handlers.Add(26, new StaticRequestHandler(SharedPacketLib.GetSubscriptionData));
            handlers.Add(157, new StaticRequestHandler(SharedPacketLib.GetBadges));
            handlers.Add(158, new StaticRequestHandler(SharedPacketLib.UpdateBadges));
            handlers.Add(370, new StaticRequestHandler(SharedPacketLib.GetAchievements));
            handlers.Add(44, new StaticRequestHandler(SharedPacketLib.ChangeLook));
            handlers.Add(484, new StaticRequestHandler(SharedPacketLib.ChangeMotto));
            handlers.Add(375, new StaticRequestHandler(SharedPacketLib.GetWardrobe));
            handlers.Add(376, new StaticRequestHandler(SharedPacketLib.SaveWardrobe));
            handlers.Add(404, new StaticRequestHandler(SharedPacketLib.GetInventory));
            handlers.Add(3000, new StaticRequestHandler(SharedPacketLib.GetPetsInventory));
        }
        #endregion
    }
}
