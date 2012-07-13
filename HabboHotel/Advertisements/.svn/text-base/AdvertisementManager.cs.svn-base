using System.Collections.Generic;
using System.Data;

using Database_Manager.Database.Session_Details.Interfaces;
using System;

namespace Butterfly.HabboHotel.Advertisements
{
    class AdvertisementManager
    {
        internal List<RoomAdvertisement> RoomAdvertisements;

        internal AdvertisementManager()
        {
            RoomAdvertisements = new List<RoomAdvertisement>();
        }

        internal void LoadRoomAdvertisements(IQueryAdapter dbClient)
        {
            RoomAdvertisements.Clear();

            dbClient.setQuery("SELECT * FROM room_ads WHERE enabled = 1");
            DataTable Data = dbClient.getTable();

            if (Data == null)
            {
                return;
            }

            foreach (DataRow Row in Data.Rows)
            {
                RoomAdvertisements.Add(new RoomAdvertisement(Convert.ToUInt32(Row["id"]), (string)Row["ad_image"],
                    (string)Row["ad_link"], (int)Row["views"], (int)Row["views_limit"]));
            }
        }
        internal RoomAdvertisement GetRandomRoomAdvertisement()
        {
            if (RoomAdvertisements.Count <= 0)
            {
                return null;
            }

            while (true)
            {
                int RndId = ButterflyEnvironment.GetRandomNumber(0, (RoomAdvertisements.Count - 1));

                if (RoomAdvertisements[RndId] != null && !RoomAdvertisements[RndId].ExceededLimit)
                {
                    return RoomAdvertisements[RndId];
                }
            }
        }
    }
}
