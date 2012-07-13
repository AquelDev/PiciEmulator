using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Butterfly.HabboHotel.Items;
using Butterfly.HabboHotel.Rooms;
using Butterfly.HabboHotel.SoundMachine.Composers;

namespace Butterfly.HabboHotel.SoundMachine
{
    class RoomMusicController
    {
        private Dictionary<uint, SongItem> mLoadedDisks;
        private SortedDictionary<int, SongInstance> mPlaylist;
        private SongInstance mSong;
        private int mSongQueuePosition;
        private bool mIsPlaying;
        private double mStartedPlayingTimestamp;
        private RoomItem mRoomOutputItem;
        private static bool mBroadcastNeeded;

        public SongInstance CurrentSong
        {
            get
            {
                return mSong;
            }
        }

        public bool IsPlaying
        {
            get
            {
                return mIsPlaying;
            }
        }

        public double TimePlaying
        {
            get
            {
                return ButterflyEnvironment.GetUnixTimestamp() - mStartedPlayingTimestamp;
            }
        }

        public int SongSyncTimestamp
        {
            get
            {
                if (!mIsPlaying || mSong == null)
                {
                    return 0;
                }

                if (TimePlaying >= mSong.SongData.LengthSeconds)
                {
                    return (int)mSong.SongData.LengthSeconds;
                }

                return (int)(TimePlaying * 1000);
            }
        }

        public SortedDictionary<int, SongInstance> Playlist
        {
            get
            {
                SortedDictionary<int, SongInstance> Copy = new SortedDictionary<int, SongInstance>();

                lock (mPlaylist)
                {
                    foreach (KeyValuePair<int, SongInstance> Data in mPlaylist)
                    {
                        Copy.Add(Data.Key, Data.Value);
                    }
                }

                return Copy;
            }
        }

        public int PlaylistCapacity
        {
            get
            {
                return 20;
            }
        }

        public int PlaylistSize
        {
            get
            {
                return mPlaylist.Count;
            }
        }

        public bool HasLinkedItem
        {
            get
            {
                return mRoomOutputItem != null;
            }
        }

        public uint LinkedItemId
        {
            get
            {
                return (mRoomOutputItem != null ? mRoomOutputItem.Id : 0);
            }
        }

        public int SongQueuePosition
        {
            get
            {
                return mSongQueuePosition;
            }
        }

        public RoomMusicController()
        {
            mLoadedDisks = new Dictionary<uint, SongItem>();
            mPlaylist = new SortedDictionary<int, SongInstance>();
        }

        public void LinkRoomOutputItem(RoomItem Item)
        {
            mRoomOutputItem = Item;
        }

        public int AddDisk(SongItem DiskItem)
        {
            uint SongId = (uint)DiskItem.songID;
            

            if (SongId == 0)
            {
                return -1;
            }

            SongData SongData = SongManager.GetSong(SongId);

            if (SongData == null)
            {
                return -1;
            }

            if (mLoadedDisks.ContainsKey(DiskItem.itemID))
                return -1;

            mLoadedDisks.Add(DiskItem.itemID, DiskItem);

            int NewPlaylistId = mPlaylist.Count;

            lock (mPlaylist)
            {
                mPlaylist.Add(NewPlaylistId, new SongInstance(DiskItem, SongData));
            }

            return NewPlaylistId;
        }

        public SongItem RemoveDisk(int PlaylistIndex)
        {
            SongInstance Instance = null;

            lock (mPlaylist)
            {
                if (!mPlaylist.ContainsKey(PlaylistIndex))
                {
                    return null;
                }

                Instance = mPlaylist[PlaylistIndex];
                mPlaylist.Remove(PlaylistIndex);
            }

            lock (mLoadedDisks)
            {
                mLoadedDisks.Remove(Instance.DiskItem.itemID);
            }

            RepairPlaylist();

            if (PlaylistIndex == mSongQueuePosition)
            {
                PlaySong();
            }

            return Instance.DiskItem;
        }

        public void Update(Room Instance)
        {
            if (mIsPlaying && (mSong == null || (TimePlaying >= (mSong.SongData.LengthSeconds + 1))))
            {
                if (mPlaylist.Count == 0)
                {
                    Stop();

                    mRoomOutputItem.ExtraData = "0";
                    mRoomOutputItem.UpdateState();
                }
                else
                {
                    SetNextSong();
                }

                mBroadcastNeeded = true;
            }

            if (mBroadcastNeeded)
            {
                BroadcastCurrentSongData(Instance);
                mBroadcastNeeded = false;
            }
        }

        public void RepairPlaylist()
        {
            List<SongItem> LoadedDiskCopy = null;

            lock (mLoadedDisks)
            {
                LoadedDiskCopy = mLoadedDisks.Values.ToList();
                mLoadedDisks.Clear();
            }

            lock (mPlaylist)
            {
                mPlaylist.Clear();
            }

            foreach (SongItem LoadedDisk in LoadedDiskCopy)
            {
                AddDisk(LoadedDisk);
            }
        }

        public void SetNextSong()
        {
            mSongQueuePosition++;
            PlaySong();
        }

        public void PlaySong()
        {
            if (mSongQueuePosition >= mPlaylist.Count)
            {
                mSongQueuePosition = 0;
            }

            if (mPlaylist.Count == 0)
            {
                Stop();
                return;
            }

            mSong = mPlaylist[mSongQueuePosition];
            mStartedPlayingTimestamp = ButterflyEnvironment.GetUnixTimestamp();
            mBroadcastNeeded = true;
        }

        public void Start()
        {
            mIsPlaying = true;
            mSongQueuePosition = -1;
            SetNextSong();
        }

        public void Stop()
        {
            mSong = null;
            mIsPlaying = false;
            mSongQueuePosition = -1;
            mBroadcastNeeded = true;
        }

        internal void BroadcastCurrentSongData(Room Instance)
        {
            if (mSong != null)
                Instance.SendMessage(JukeboxComposer.ComposePlayingComposer(mSong.SongData.Id, mSongQueuePosition, 0));
            else
                Instance.SendMessage(JukeboxComposer.ComposePlayingComposer(0, 0, 0));
        }

        internal void OnNewUserEnter(RoomUser user)
        {
            if (user.IsBot || user.GetClient() == null || mSong == null)
                return;

            user.GetClient().SendMessage(JukeboxComposer.ComposePlayingComposer(mSong.SongData.Id, mSongQueuePosition, SongSyncTimestamp));
        }

        public void Reset()
        {
            lock (mLoadedDisks)
            {
                mLoadedDisks.Clear();
            }

            lock (mPlaylist)
            {
                mPlaylist.Clear();
            }

            mRoomOutputItem = null;
            mSongQueuePosition = -1;
            mStartedPlayingTimestamp = 0;
        }

        internal void Destroy()
        {
            if (mLoadedDisks != null)
                mLoadedDisks.Clear();
            if (mPlaylist != null)
                mPlaylist.Clear();
            mPlaylist = null;
            mLoadedDisks = null;
            mSong = null;
            mRoomOutputItem = null;
        }
    }
}
