using Pici.Storage.Database.Session_Details.Interfaces;

namespace Pici.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces
{
    interface IWiredTrigger
    {
        void Dispose();
        void SaveToDatabase(IQueryAdapter dbClient);
        void LoadFromDatabase(IQueryAdapter dbClient, Room insideRoom);
        void DeleteFromDatabase(IQueryAdapter dbClient);
    }
}
