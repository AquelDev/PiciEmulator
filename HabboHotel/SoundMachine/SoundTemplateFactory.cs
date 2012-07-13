using System.Collections.Generic;
using System.Data;
using Database_Manager.Database.Session_Details.Interfaces;

namespace Butterfly.HabboHotel.SoundMachine
{
    class SoundTemplateFactory
    {
        internal static Dictionary<uint, SoundTemplate> GetTemplates()
        {
            Dictionary<uint, SoundTemplate> sounds = new Dictionary<uint, SoundTemplate>();

            DataTable dTable;
            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().getQueryreactor())
            {
                dbClient.setQuery("SELECT * FROM songs");
                dTable = dbClient.getTable();
            }

            uint id;
            string name;
            string artist;
            string songData;
            double length;
            foreach (DataRow dRow in dTable.Rows)
            {
                id = (uint)dRow[0];
                name = (string)dRow[1];
                artist = (string)dRow[2];
                songData = (string)dRow[3];
                length = (double)dRow[4];

                SoundTemplate template = new SoundTemplate(id, name, artist, songData, length);
                sounds.Add(id, template);
            }

            return sounds;
        }
    }
}
