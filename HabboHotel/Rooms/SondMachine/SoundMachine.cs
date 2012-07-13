//using System.Collections;
//using System.Collections.Generic;
//using Pici.HabboHotel.SoundMachine;
//using Pici.Messages;

//namespace Pici.HabboHotel.Rooms.SondMachine
//{
//    class SoundMachine
//    {
//        private Room room;
//        private Dictionary<uint, Song> songs;
//        private Queue playlist;
//        private Song currentPlayingSong;
//        private int pos;

//        public SoundMachine(Room room)
//        {
//            this.room = room;
//        }

//        internal void SetTracks(Dictionary<uint, Song> tracks)
//        {
//            songs = tracks;
//        }

//        private void BroadcastCurrentSong()
//        {
//            ServerMessage Message = new ServerMessage(327);

//            Message.Append(currentPlayingSong.template.id);
//            Message.Append(currentPlayingSong.songID);
//            Message.Append(currentPlayingSong.template.id);
//            Message.Append(0);
//            Message.Append(0);
//        }
//    }
//}
