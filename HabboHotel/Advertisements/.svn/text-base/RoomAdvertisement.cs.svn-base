using System;

using Database_Manager.Database.Session_Details.Interfaces;

namespace Butterfly.HabboHotel.Advertisements
{
    class RoomAdvertisement
    {
        internal uint Id;
        internal string AdImage;
        internal string AdLink;
        internal int Views;
        internal int ViewsLimit;

        internal Boolean ExceededLimit
        {
            get
            {
                if (ViewsLimit <= 0)
                {
                    return false;
                }

                if (Views >= ViewsLimit)
                {
                    return true;
                }

                return false;
            }
        }

        internal RoomAdvertisement(uint Id, string AdImage, string AdLink, int Views, int ViewsLimit)
        {
            this.Id = Id;
            this.AdImage = AdImage;
            this.AdLink = AdLink;
            this.Views = Views;
            this.ViewsLimit = ViewsLimit;
        }

        internal void OnView()
        {
            this.Views++;

            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.runFastQuery("UPDATE room_ads SET views = views + 1 WHERE id = " + Id);
            }
        }
    }
}
