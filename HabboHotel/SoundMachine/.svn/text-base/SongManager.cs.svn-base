using System.Collections.Generic;
using System.Data;
using System.Linq;
using Butterfly.HabboHotel.GameClients;
using Butterfly.HabboHotel.Items;
using Butterfly.HabboHotel.Rooms;
using Butterfly.HabboHotel.SoundMachine.Composers;
using Butterfly.Messages;
using Database_Manager.Database.Session_Details.Interfaces;
using System;

namespace Butterfly.HabboHotel.SoundMachine
{
    class SongManager
    {
        private const int CACHE_LIFETIME = 180;

        private static Dictionary<uint, SongData> songs;
        private static Dictionary<uint, double> cacheTimer;

        internal static void Initialize()
        {
            songs = new Dictionary<uint, SongData>();
            cacheTimer = new Dictionary<uint, double>();

            DataTable dTable;
            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.setQuery("SELECT * FROM songs");
                dTable = dbClient.getTable();
            }

            foreach (DataRow dRow in dTable.Rows)
            {
                SongData song = GetSongFromDataRow(dRow);
                songs.Add(song.Id, song);
            }
        }

        internal static void ProcessThread()
        {
            double CurrentTime = ButterflyEnvironment.GetUnixTimestamp();

            List<uint> ToRemove = new List<uint>();

            foreach (KeyValuePair<uint, double> CacheData in cacheTimer)
            {
                if (CurrentTime - CacheData.Value >= CACHE_LIFETIME)
                {
                    ToRemove.Add(CacheData.Key);
                }
            }

            foreach (uint RemoveId in ToRemove)
            {
                songs.Remove(RemoveId);
                cacheTimer.Remove(RemoveId);
            }
        }

        internal static SongData GetSongFromDataRow(DataRow dRow)
        {
            return new SongData(Convert.ToUInt32(dRow["id"]), (string)dRow["name"], (string)dRow["artist"], (string)dRow["song_data"],
                (double)dRow["length"]);
        }

        internal static SongData GetSong(uint SongId)
        {
            SongData song = null;
            songs.TryGetValue(SongId, out song);
            return song;
        }

        private static void GetSongData(GameClient Session, ClientMessage Message)
        {
            int Amount = Message.PopWiredInt32();
            List<SongData> Songs = new List<SongData>();

            for (int i = 0; i < Amount; i++)
            {
                SongData Song = GetSong(Message.PopWiredUInt());

                if (Song == null)
                    continue;

                Songs.Add(Song);
            }

            Session.SendMessage(JukeboxComposer.Compose(Songs));
        }

        private static void AddToPlaylist(GameClient Session, ClientMessage Message)
        {
            Room Instance = Session.GetHabbo().CurrentRoom;

            if (Instance == null || !Instance.CheckRights(Session, true) || !Instance.GotMusicController() ||
                Instance.GetRoomMusicController().PlaylistSize >= Instance.GetRoomMusicController().PlaylistCapacity)
            {
                return;
            }

            UserItem DiskUserItem = Session.GetHabbo().GetInventoryComponent().GetItem(Message.PopWiredUInt());

            if (DiskUserItem == null || DiskUserItem.GetBaseItem().InteractionType != InteractionType.musicdisc)
            {
                return;
            }

            SongItem item = new SongItem(DiskUserItem);

            int NewOrder = Instance.GetRoomMusicController().AddDisk(item);

            if (NewOrder < 0)
            {
                return;
            }

            Session.GetHabbo().GetInventoryComponent().RemoveItem(item.itemID, true);

            Session.SendMessage(JukeboxComposer.Compose(Instance.GetRoomMusicController().PlaylistCapacity,
                Instance.GetRoomMusicController().Playlist.Values.ToList()));
        }

        private static void RemoveFromPlaylist(GameClient Session, ClientMessage Message)
        {
            Room Instance = Session.GetHabbo().CurrentRoom;

            if (Instance == null ||  !Instance.GotMusicController() || !Instance.CheckRights(Session, true))
            {
                return;
            }

            SongItem TakenItem = Instance.GetRoomMusicController().RemoveDisk(Message.PopWiredInt32());
            // playlist will skip to the next item automatically if it has to

            if (TakenItem == null)
            {
                return;
            }

            Session.GetHabbo().GetInventoryComponent().AddNewItem(TakenItem.itemID, TakenItem.baseItem.ItemId, TakenItem.songID.ToString(), true, true);


            Session.SendMessage(JukeboxComposer.Compose(Session));
            Session.SendMessage(JukeboxComposer.Compose(Instance.GetRoomMusicController().PlaylistCapacity,
                Instance.GetRoomMusicController().Playlist.Values.ToList()));
        }

        private static void GetDisks(GameClient Session, ClientMessage Message)
        {
            Session.SendMessage(JukeboxComposer.Compose(Session));
        }

        private static void GetPlaylist(GameClient Session, ClientMessage Message)
        {
            Room Instance = Session.GetHabbo().CurrentRoom;

            if (Instance == null || !Instance.CheckRights(Session, true))
            {
                return;
            }

            Session.SendMessage(JukeboxComposer.Compose(Instance.GetRoomMusicController().PlaylistCapacity,
                Instance.GetRoomMusicController().Playlist.Values.ToList()));
        }
    }
}
