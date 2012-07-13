using System.Collections.Generic;
using Butterfly.HabboHotel.GameClients;
using Butterfly.HabboHotel.Items;
using Butterfly.Messages;
using System.Collections;

namespace Butterfly.HabboHotel.SoundMachine.Composers
{
    class JukeboxComposer
    {
        internal static ServerMessage Compose(GameClient Session)
        {
            return Session.GetHabbo().GetInventoryComponent().SerializeMusicDiscs();
        }

        internal static ServerMessage Compose(int PlaylistCapacity, List<SongInstance> Playlist)
        {
            ServerMessage Message = new ServerMessage(334);
            Message.AppendInt32(PlaylistCapacity);
            Message.AppendInt32(Playlist.Count);

            foreach (SongInstance Song in Playlist)
            {
                Message.AppendUInt(Song.DiskItem.itemID);
                Message.AppendUInt(Song.SongData.Id);
            }

            return Message;
        }

        internal static ServerMessage Compose(uint SongId, int PlaylistItemNumber, int SyncTimestampMs)
        {
            ServerMessage Message = new ServerMessage(327);

            if (SongId == 0)
            {
                Message.AppendInt32(-1);
                Message.AppendInt32(-1);
                Message.AppendInt32(-1);
                Message.AppendInt32(-1);
                Message.AppendInt32(0);
            }
            else
            {
                Message.AppendUInt(SongId);
                Message.AppendInt32(PlaylistItemNumber);
                Message.AppendUInt(SongId);
                Message.AppendInt32(0);
                Message.AppendInt32(SyncTimestampMs);
            }

            return Message;
        }

        public static ServerMessage Compose(List<SongData> Songs)
        {
            ServerMessage Message = new ServerMessage(300);
            Message.AppendInt32(Songs.Count);

            foreach (SongData Song in Songs)
            {
                Message.AppendUInt(Song.Id);
                Message.AppendStringWithBreak(Song.Name);
                Message.AppendStringWithBreak(Song.Data);
                Message.AppendInt32(Song.LengthMiliseconds);
                Message.AppendStringWithBreak(Song.Artist);
            }

            return Message;
        }

        public static ServerMessage ComposePlayingComposer(uint SongId, int PlaylistItemNumber, int SyncTimestampMs)
        {
            ServerMessage Message = new ServerMessage(327);

            if (SongId == 0)
            {
                Message.AppendInt32(-1);
                Message.AppendInt32(-1);
                Message.AppendInt32(-1);
                Message.AppendInt32(-1);
                Message.AppendInt32(0);
            }
            else
            {
                Message.AppendUInt(SongId);
                Message.AppendInt32(PlaylistItemNumber);
                Message.AppendUInt(SongId);
                Message.AppendInt32(0);
                Message.AppendInt32(SyncTimestampMs);
            }

            return Message;
        }

        internal static ServerMessage SerializeSongInventory(Hashtable songs)
        {
            ServerMessage message = new ServerMessage(333);
            message.AppendInt32(songs.Count);

            foreach (UserItem userItem in songs.Values)
            {
                uint songID = (uint)TextHandling.Parse(userItem.ExtraData);

                message.AppendUInt(userItem.Id);
                message.AppendUInt(songID);
            }

            return message;
        }
    }
}
