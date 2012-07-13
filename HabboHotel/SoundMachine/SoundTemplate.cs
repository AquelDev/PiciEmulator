namespace Butterfly.HabboHotel.SoundMachine
{
    struct SoundTemplate
    {
        internal readonly uint id;
        internal readonly string name;
        internal readonly string artist;
        internal readonly string songData;
        internal readonly double length;

        internal SoundTemplate(uint id, string name, string artist, string songData, double length)
        {
            this.id = id;
            this.name = name;
            this.artist = artist;
            this.songData = songData;
            this.length = length;
        }
    }
}
