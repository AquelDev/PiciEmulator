using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Butterfly.HabboHotel.SoundMachine;
using Butterfly.HabboHotel.SoundMachine.Composers;
using Butterfly.HabboHotel.Rooms;
using Butterfly.HabboHotel.Items;

namespace Butterfly.Messages
{
    partial class GameClientMessageHandler
    {
        internal void GetMusicData()
        {

            int Amount = Request.PopWiredInt32();
            List<SongData> Songs = new List<SongData>();

            for (int i = 0; i < Amount; i++)
            {
                SongData Song = SongManager.GetSong(Request.PopWiredUInt());

                if (Song == null)
                {
                    continue;
                }

                Songs.Add(Song);
            }


            Session.SendMessage(JukeboxComposer.Compose(Songs));

            Songs.Clear();
            Songs = null;
        }

        internal void AddPlaylistItem()
        {
            if (Session == null || Session.GetHabbo() == null || Session.GetHabbo().CurrentRoom == null)
                return;

            Room currentRoom = Session.GetHabbo().CurrentRoom;

            if (!currentRoom.CheckRights(Session, true))
                return;

            RoomMusicController musicController = currentRoom.GetRoomMusicController();


            if (musicController.PlaylistSize >= musicController.PlaylistCapacity)
                return;

            uint itemID = Request.PopWiredUInt();
            UserItem item = Session.GetHabbo().GetInventoryComponent().GetItem(itemID);
            if (item == null || item.GetBaseItem().InteractionType != InteractionType.musicdisc)
                return;

            //RoomItem roomItem = new RoomItem(item.Id, currentRoom.RoomId, item.BaseItem, item.ExtraData, 0, 0, 0, 0, currentRoom);
            SongItem sitem = new SongItem(item);

            int NewOrder = musicController.AddDisk(sitem);
            if (NewOrder < 0)
            {
                return;
            }
            sitem.SaveToDatabase(currentRoom.RoomId);
            Session.GetHabbo().GetInventoryComponent().RemoveItem(itemID, true);
            Session.SendMessage(JukeboxComposer.Compose(musicController.PlaylistCapacity, musicController.Playlist.Values.ToList()));
        }

        internal void RemovePlaylistItem()
        {
            if (Session == null || Session.GetHabbo() == null || Session.GetHabbo().CurrentRoom == null)
                return;

            Room currentRoom = Session.GetHabbo().CurrentRoom;

            if (!currentRoom.CheckRights(Session, true) || !currentRoom.GotMusicController())
                return;

            RoomMusicController musicController = currentRoom.GetRoomMusicController();

            SongItem item = musicController.RemoveDisk(Request.PopWiredInt32());
            if (item == null)
                return;

            item.RemoveFromDatabase();
            Session.GetHabbo().GetInventoryComponent().AddNewItem(item.itemID, item.baseItem.ItemId, item.songID.ToString(), true, true);
            Session.GetHabbo().GetInventoryComponent().UpdateItems(false);

            Session.SendMessage(JukeboxComposer.SerializeSongInventory(Session.GetHabbo().GetInventoryComponent().songDisks));
            Session.SendMessage(JukeboxComposer.Compose(musicController.PlaylistCapacity, musicController.Playlist.Values.ToList()));
        }

        internal void GetDisks()
        {
            if (Session == null || Session.GetHabbo() == null || Session.GetHabbo().GetInventoryComponent() == null)
                return;

            Session.SendMessage(JukeboxComposer.SerializeSongInventory(Session.GetHabbo().GetInventoryComponent().songDisks));
        }

        internal void GetPlaylists()
        {
            if (Session == null || Session.GetHabbo() == null || Session.GetHabbo().CurrentRoom == null)
                return;

            Room currentRoom = Session.GetHabbo().CurrentRoom;

            if (!currentRoom.CheckRights(Session, true) || !currentRoom.GotMusicController())
                return;

            RoomMusicController musicController = currentRoom.GetRoomMusicController();

            Session.SendMessage(JukeboxComposer.Compose(musicController.PlaylistCapacity, musicController.Playlist.Values.ToList()));
        }
    }
}
