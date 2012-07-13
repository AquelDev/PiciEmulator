using Database_Manager.Database.Session_Details.Interfaces;

namespace Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces
{
    interface IWiredTrigger
    {
        void Dispose();
        void SaveToDatabase(IQueryAdapter dbClient);
        void LoadFromDatabase(IQueryAdapter dbClient, Room insideRoom);
        void DeleteFromDatabase(IQueryAdapter dbClient);
    }
}
