using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Database_Manager.Database.Session_Details.Interfaces;

namespace Butterfly.HabboHotel.RoomBots
{
    class BotManager
    {
        private List<RoomBot> Bots;

        internal BotManager()
        {
            Bots = new List<RoomBot>();
        }

        internal void LoadBots(IQueryAdapter dbClient)
        {
            Bots = new List<RoomBot>();

            dbClient.setQuery("SELECT id, room_id, ai_type, walk_mode, name, motto, look, x, y, z, rotation, min_x, min_y, max_x, max_y FROM bots;");
            DataTable Data = dbClient.getTable();

            dbClient.setQuery("SELECT id, bot_id, keywords, response_text, mode, serve_id FROM bots_responses;");
            DataTable BotResponses = dbClient.getTable();

            dbClient.setQuery("SELECT text, shout, bot_id FROM bots_speech;");
            DataTable BotSpeech = dbClient.getTable();

            List<BotResponse> Responses = new List<BotResponse>();
            List<RandomSpeech> Speeches = new List<RandomSpeech>();

            foreach (DataRow Row in BotResponses.Rows)
            {
                Responses.Add(new BotResponse(Convert.ToUInt32(Row["bot_id"]), (string)Row["keywords"], (string)Row["response_text"], Row["mode"].ToString(), (int)Row["serve_id"]));
            }

            foreach (DataRow Row in BotSpeech.Rows)
            {
                Speeches.Add(new RandomSpeech((string)Row["text"], ButterflyEnvironment.EnumToBool(Row["shout"].ToString()), Convert.ToUInt32(Row["bot_id"])));
            }


            if (Data == null)
            {
                return;
            }

            foreach (DataRow Row in Data.Rows)
            {
                string BotAI = (string)Row["ai_type"];
                AIType BotAIType;
                switch (BotAI)
                {
                    case "generic":
                        BotAIType = AIType.Generic;
                        break;
                    case "guide":
                        BotAIType = AIType.Guide;
                        break;
                    case "pet":
                        BotAIType = AIType.Pet;
                        break;
                    default:
                        BotAIType = AIType.Generic;
                        break;
                }

                Bots.Add(new RoomBot(Convert.ToUInt32(Row["id"]), Convert.ToUInt32(Row["room_id"]), BotAIType, (string)Row["walk_mode"],
                    (String)Row["name"], (string)Row["motto"], (String)Row["look"], (int)Row["x"], (int)Row["y"], (int)Row["z"],
                    (int)Row["rotation"], (int)Row["min_x"], (int)Row["min_y"], (int)Row["max_x"], (int)Row["max_y"], ref Speeches, ref Responses));
            }
        }

        //internal bool RoomHasBots(UInt32 RoomId)
        //{
        //    return (GetBotsForRoom(RoomId).Count >= 1);
        //}

        internal List<RoomBot> GetBotsForRoom(UInt32 RoomId)
        {
            return new List<RoomBot>(from p in Bots where p.RoomId == RoomId select p);
        }

        internal RoomBot GetBot(UInt32 BotId)
        {
            return (from p in Bots where p.BotId == BotId select p).SingleOrDefault();
        }
    }
}
